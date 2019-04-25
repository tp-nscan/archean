namespace archean.core
open System
open archean.core.Sorting
open archean.core.Sorting.SortableIntArray
open archean.core.SortersFromData
open SorterB

module SortingReports =

    type HistoItem = {stageCount:int; switchCount:int; minSwitchables:int; avgSwitchables:float; sorterCount:int}

    let PrefixedSwitchTracker (totalSwitches: int) (prefixSwitches: int) =
        Array.init totalSwitches (fun i -> if (i<prefixSwitches) then 1 else 0)

    let SortableTestCases (randGenerationMode:RandGenerationMode) =
        let prefixSorter = CreatePrefixedSorter randGenerationMode
        let switchTracker = Array.init prefixSorter.switches.Length (fun i -> 0)
        let (_, sortableRes) = RunSwitchesAndGetResults switchTracker 
                                   prefixSorter (SortableFuncAllBinary prefixSorter.order)
        sortableRes |> Set.toSeq

    let WeightedSortableTestCases (randGenerationMode:RandGenerationMode) =
        let prefixSorter = CreatePrefixedSorter randGenerationMode
        let switchTracker = Array.init prefixSorter.switches.Length (fun i -> 0)
        let (_, sortableRes) = RunWeightedOnSorter switchTracker 
                                   prefixSorter (WeightedSortableFuncAllBinary prefixSorter.order)
        sortableRes |> Array.toSeq

        
    //type HistoItem = {stageCount:int; switchCount:int; minSwitchables:int; avgSwitchables:float; sorterCount:int}

    let MakeHistoItem (stageCount:int) (switchCount:int) (res:seq<int*SorterResult>) =
        let aa = res |> Seq.map(fun i -> (fst i)) |> Seq.toArray
        {
            stageCount=stageCount; 
            switchCount=switchCount; 
            minSwitchables= aa |> Array.min; 
            avgSwitchables= aa |> Array.averageBy(fun aa -> float aa);
            sorterCount=aa.Length;
        }


    let MakeStagePerfHistogram8 (randGenerationMode : RandGenerationMode)
                                       (sorterCount:int) (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases randGenerationMode) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> (Array.copy a, 1))
                |> Array.toSeq

        let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
            let res = SorterB.GetStagePerfAndSwitchUsage 
                                (PrefixedSwitchTracker completeSorterLength prefixedSorterLength)
                                stagedSorterDef
                                ((randGenerationMode |> To_PrefixStageCount) + 1)
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
                |> Seq.map(fun i -> (CreateRandomSorterDef randGenerationMode rnd) |> StagedSorterDef.ToStagedSorterDef )
                |> Seq.toArray

        let histogram = 
            h1
            |> Array.Parallel.map(fun stagedSorterDef -> MakeStagedSorterResults stagedSorterDef)
            |> Seq.filter(fun ((success, _ ), _ ) -> success)
            |> Seq.map(fun (( _ , switchResults), stagedSorterDef) -> 
                            (fst switchResults.Value, SorterResult.MakeSorterResult stagedSorterDef.sorterDef (snd switchResults.Value)))
            |> Seq.groupBy(fun (searchableCount, sorterResult) -> SorterResult.TotalNumberOfSwitchResultsIn sorterResult.stageResults)
            |> Seq.filter(fun ((stageCount, switchCount), sorterResults) -> (stageCount=9) && (switchCount=60))
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> sorterResults |> Seq.map(fun sr-> snd sr))
        
        let rep = seq {for sh in histogram do yield! sh} 
                    |> Seq.map(fun sr -> SorterResult.GetSorterString sr)
                    |> Seq.toArray

        ("ralph", rep)

        //    |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> MakeHistoItem stageCount switchCount sorterResults)
        //    |> Seq.toArray
   
        //let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
        //                        prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
        //                        seed sorterCount (histogram|>Array.sumBy(fun a -> a.sorterCount))
        //(summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))
   


    let MakeStagePerfHistogram (randGenerationMode : RandGenerationMode)
                                       (sorterCount:int) (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases randGenerationMode) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> (Array.copy a, 1))
                |> Array.toSeq

        let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
            let res = SorterB.GetStagePerfAndSwitchUsage 
                                (PrefixedSwitchTracker completeSorterLength prefixedSorterLength)
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
            |> Seq.map(fun (( _ , switchResults), stagedSorterDef) -> 
                            (fst switchResults.Value, SorterResult.MakeSorterResult stagedSorterDef.sorterDef (snd switchResults.Value)))
            |> Seq.groupBy(fun (searchableCount, sorterResult) -> SorterResult.TotalNumberOfSwitchResultsIn sorterResult.stageResults)
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> MakeHistoItem stageCount switchCount sorterResults)
            |> Seq.toArray
   
        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
                                prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
                                seed sorterCount (histogram|>Array.sumBy(fun a -> a.sorterCount))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))
   

     //returns (NumStagesUsed, NumSwitchesUsed, Count)  
    let MakeStageAndSwitchUseHistogram (randGenerationMode : RandGenerationMode)
                                       (sorterCount:int) (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases randGenerationMode) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> Array.copy a)
                |> Array.toSeq

        let MakeSorterResults (sorterDef:SorterDef) =
            let res = SorterB.GetSwitchUsagesIfSorterAlwaysWorks 
                                (PrefixedSwitchTracker completeSorterLength prefixedSorterLength)
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
            |> Seq.map(fun (( _ , switchResults), sorterDef) -> SorterResult.MakeSorterResult sorterDef switchResults.Value)
            |> Seq.groupBy(fun sorterResult -> SorterResult.TotalNumberOfSwitchResultsIn sorterResult.stageResults)
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> [| stageCount; switchCount; sorterResults|> Seq.length |])
            |> Seq.toArray

        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
                                prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
                                seed sorterCount (histogram|>Array.sumBy(fun a -> a.[2]))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))