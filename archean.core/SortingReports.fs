namespace archean.core
open System
open archean.core.Sorting
open archean.core.SortersFromData
open archean.core.Sorter

module SortingReports =

    type HistoItem = {stageCount:int; switchCount:int; minSwitchables:int; avgSwitchables:float; sorterCount:int}
    
    let SortableTestCases (randGenerationMode:RandGenerationMode) =
        let prefixSorter = CreatePrefixedSorter randGenerationMode
        let switchTracker = SwitchTracker.Make prefixSorter.switches.Length
        let (_, sortableRes) = RunSwitchesAndGetResults 
                                   switchTracker 
                                   prefixSorter 
                                   (SortableIntArray.SortableFuncAllBinary prefixSorter.order)
        sortableRes |> Set.toSeq


    let WeightedSortableTestCases (randGenerationMode:RandGenerationMode) =
        let prefixSorter = CreatePrefixedSorter randGenerationMode
        let switchTracker = SwitchTracker.Make prefixSorter.switches.Length
        let (_, sortableRes) = RunWeightedOnSorter 
                                   switchTracker 
                                   prefixSorter 
                                   (SortableIntArray.WeightedSortableFuncAllBinary prefixSorter.order)
        sortableRes |> Array.toSeq


    let MakeHistoItem 
            (stageCount:int) 
            (switchCount:int) 
            (res:seq<int*SorterResult>) =
        let aa = res |> Seq.map(fun i -> (fst i)) |> Seq.toArray
        {
            stageCount=stageCount; 
            switchCount=switchCount; 
            minSwitchables= aa |> Array.min; 
            avgSwitchables= aa |> Array.averageBy(fun aa -> float aa);
            sorterCount=aa.Length;
        }


    let MakeStagePerfHistogram8 
                    (randGenerationMode : RandGenerationMode)
                    (sorterCount:int) 
                    (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases (randGenerationMode |> RemoveLastRefStage)) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> (Array.copy a, 1))
                |> Array.toSeq

        let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
            let res = StagedSorter.GetStagePerfAndSwitchUsage 
                                (SwitchTracker.MakePrefixed completeSorterLength prefixedSorterLength)
                                stagedSorterDef
                                ((randGenerationMode |> To_PrefixStageCount))
                                TestSortablesSeq
            (res, stagedSorterDef)
        
        
        
    //type HistoItem = {stageCount:int; switchCount:int; minSwitchables:int; avgSwitchables:float; sorterCount:int}

        let MakeHistoLine (tt:HistoItem) = 
            sprintf "%s\t%d\t%d\t%d\t%f\t%d"
                    (randGenerationMode |> To_RefSorter_PrefixStages)  
                    tt.stageCount tt.switchCount tt.minSwitchables tt.avgSwitchables
                    tt.sorterCount

        let rnd = new Random(seed)
        let h1 =
                {1 .. sorterCount}
                    |> Seq.map(fun i -> (CreateRandomStagedSorterDef randGenerationMode rnd))
                    |> Seq.toArray

        let histogram = 
            h1
            |> Array.Parallel.map(fun stagedSorterDef -> MakeStagedSorterResults stagedSorterDef)
            |> Seq.filter(fun ((success, _ ), _ ) -> success)
            |> Seq.map(fun (( _ , switchUsages), stagedSorterDef) -> 
                            (fst switchUsages.Value, SorterResult.MakeSorterResult stagedSorterDef.sorterDef (snd switchUsages.Value)))
            |> Seq.groupBy(fun (searchableCount, sorterResult) -> SorterResult.SwitchAndStageCountsIn sorterResult.stageResults)
            |> Seq.filter(fun ((stageCount, switchCount), sorterResults) -> (stageCount<14) && (switchCount < 65))
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> sorterResults |> Seq.map(fun sr-> snd sr))
        
        let rep = seq {for sh in histogram do yield! sh} 
                    |> Seq.map(fun sr -> SorterResult.GetSwitchReport sr)
                    |> Seq.toArray

        ("ralph", rep)

        //    |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> MakeHistoItem stageCount switchCount sorterResults)
        //    |> Seq.toArray
   
        //let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
        //                        prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
        //                        seed sorterCount (histogram|>Array.sumBy(fun a -> a.sorterCount))
        //(summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))
   


    let MakeStagePerfHistogram0 
                    (randGenerationMode : RandGenerationMode)
                    (sorterCount:int) 
                    (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases (randGenerationMode |> RemoveLastRefStage)) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> (Array.copy a, 1))
                |> Array.toSeq

        let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
            let res = StagedSorter.GetStagePerfAndSwitchUsage 
                        (SwitchTracker.MakePrefixed completeSorterLength prefixedSorterLength)
                        stagedSorterDef
                        (randGenerationMode |> To_PrefixStageCount)
                        TestSortablesSeq
            (res, stagedSorterDef)
        
        
        
    //type HistoItem = {stageCount:int; switchCount:int; minSwitchables:int; avgSwitchables:float; sorterCount:int}

        let MakeHistoLine (tt:HistoItem) = 

            sprintf "%s\t%d\t%d\t%d\t%f\t%d"
                    (randGenerationMode |> To_RefSorter_PrefixStages)
                    tt.stageCount tt.switchCount tt.minSwitchables tt.avgSwitchables
                    tt.sorterCount

        let rnd = new Random(seed)
        let histogram =
            {1 .. sorterCount}
            |> Seq.map(fun i -> (CreateRandomSorterDef randGenerationMode rnd) |> StagedSorterDef.ToStagedSorterDef )
            |> Seq.toArray
            |> Array.Parallel.map(fun stagedSorterDef -> MakeStagedSorterResults stagedSorterDef)
            |> Seq.filter(fun ((success, _ ), _ ) -> success)
            |> Seq.map(fun (( _ , switchUsages), stagedSorterDef) -> 
                            (fst switchUsages.Value, SorterResult.MakeSorterResult stagedSorterDef.sorterDef (snd switchUsages.Value)))
            |> Seq.groupBy(fun (searchableCount, sorterResult) -> SorterResult.SwitchAndStageCountsIn sorterResult.stageResults)
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> MakeHistoItem stageCount switchCount sorterResults)
            |> Seq.toArray
   
        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
                                prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
                                seed sorterCount (histogram|>Array.sumBy(fun a -> a.sorterCount))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))
   

    let MakeStagePerfHistogram 
                    (randGenerationMode : RandGenerationMode)
                    (sorterCount:int) 
                    (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases (randGenerationMode |> RemoveLastRefStage)) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> (Array.copy a, 1))
                |> Array.toSeq

        let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
            let res = StagedSorter.GetStagePerfAndSwitchUsage 
                        (SwitchTracker.MakePrefixed completeSorterLength prefixedSorterLength)
                        stagedSorterDef
                        (randGenerationMode |> To_PrefixStageCount)
                        TestSortablesSeq
            (res, stagedSorterDef)
        
        
        
    //type HistoItem = {stageCount:int; switchCount:int; minSwitchables:int; avgSwitchables:float; sorterCount:int}

        let MakeHistoLine (tt:HistoItem) = 
            sprintf "%s\t%d\t%d\t%d\t%f\t%d"
                    (randGenerationMode |> To_RefSorter_PrefixStages)
                    tt.stageCount tt.switchCount tt.minSwitchables tt.avgSwitchables
                    tt.sorterCount

        let rnd = new Random(seed)
        let histogram =
            {1 .. sorterCount}
            |> Seq.map(fun i -> (CreateRandomStagedSorterDef randGenerationMode rnd))
            |> Seq.toArray
            |> Array.Parallel.map(fun stagedSorterDef -> MakeStagedSorterResults stagedSorterDef)
            |> Seq.filter(fun ((success, _ ), _ ) -> success)
            |> Seq.map(fun (( _ , switchUsages), stagedSorterDef) -> 
                            (fst switchUsages.Value, SorterResult.MakeSorterResult stagedSorterDef.sorterDef (snd switchUsages.Value)))
            |> Seq.groupBy(fun (searchableCount, sorterResult) -> SorterResult.SwitchAndStageCountsIn sorterResult.stageResults)
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> MakeHistoItem stageCount switchCount sorterResults)
            |> Seq.toArray
   
        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
                                prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
                                seed sorterCount (histogram|>Array.sumBy(fun a -> a.sorterCount))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))
   





     //returns (NumStagesUsed, NumSwitchesUsed, Count)  
    let MakeStageAndSwitchUseHistogram 
                        (randGenerationMode : RandGenerationMode)
                        (sorterCount:int) 
                        (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases randGenerationMode) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> Array.copy a)
                |> Array.toSeq

        let MakeSorterResults (sorterDef:SorterDef) =
            let res = Sorter.GetSwitchUsagesIfSorterAlwaysWorks 
                                (SwitchTracker.MakePrefixed completeSorterLength prefixedSorterLength)
                                sorterDef
                                prefixedSorterLength
                                TestSortablesSeq
            (res, sorterDef)

        let MakeHistoLine (tt:int[]) = 
            sprintf "%s\t%d\t%d\t%d"
                    (randGenerationMode |> To_RefSorter_PrefixStages)  tt.[0] tt.[1] tt.[2]

        let rnd = new Random(seed)
        let histogram =
            {1 .. sorterCount}
            |> Seq.map(fun i -> CreateRandomSorterDef randGenerationMode rnd)
            |> Seq.toArray
            |> Array.Parallel.map(fun sorterDef -> MakeSorterResults sorterDef)
            |> Seq.filter(fun ((success, _ ), _ ) -> success)
            |> Seq.map(fun (( _ , switchUsages), sorterDef) -> SorterResult.MakeSorterResult sorterDef switchUsages.Value)
            |> Seq.groupBy(fun sorterResult -> SorterResult.SwitchAndStageCountsIn sorterResult.stageResults)
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> [| stageCount; switchCount; sorterResults|> Seq.length |])
            |> Seq.toArray

        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
                                prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
                                seed sorterCount (histogram|>Array.sumBy(fun a -> a.[2]))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))