namespace archean.core
open System
open archean.core.Sorting
open archean.core.SortersFromData
open archean.core.Sorter

module SortingReports =


    let StageWisePerf (refSorter:RefSorter) = 
        let rfsst = RefSorter.CreateRefStagedSorter refSorter
        let res = StagedSorter.StageWisePerf 0 rfsst
        res


    type HistoItem = {stageCount:int; switchCount:int; minSwitchables:int; avgSwitchables:float; sorterCount:int}
    type HistoItem2 = {stageCount:int; switchCount:int; switchableCount:int; sorterCount:int}

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

    let MakeHistoItem2 
        (stageCount:int)
        (switchCount:int)
        (switchableCount:int)
        (res:seq<int*SorterResult>) =

        let aa = res |> Seq.map(fun i -> (fst i)) |> Seq.toArray
        {
            stageCount=stageCount; 
            switchCount=switchCount; 
            switchableCount=switchableCount; 
            sorterCount=aa.Length;
        }


    let MakeStagePerfHistogram 
                     (randGenerationMode : SorterGenerationMode)
                     (sorterCount:int) 
                     (seed : int) =

         let rgmForFiltering = (randGenerationMode |> RemoveRandomStages)
         let filteringSorter = CreatePrefixedSorter rgmForFiltering
         let filteringSorterLength = filteringSorter.switches.Length

         let prefixedSorter = CreatePrefixedSorter randGenerationMode
         let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
         let testSortables = (SortableTestCases rgmForFiltering) 
                             |> Seq.toArray

         let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
             let res = StagedSorter.GetStagePerfAndSwitchUsage 
                         (SwitchTracker.MakePrefixed completeSorterLength filteringSorterLength)
                         stagedSorterDef
                         (randGenerationMode |> To_PrefixStageCount)
                         (SortableIntArray.SortableSeq testSortables)
             (res, stagedSorterDef)

         let MakeHistoLine2 (tt:HistoItem2) = 
             sprintf "%s\t%d\t%d\t%d\t%d"
                     (randGenerationMode |> To_RefSorter_PrefixStages)
                     tt.stageCount tt.switchCount tt.switchableCount
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
             |> Seq.groupBy(fun (switchableCount, sorterResult) -> ((SorterResult.SwitchAndStageCountsIn sorterResult.stageResults), switchableCount))
             |> Seq.map(fun (((stageCount, switchCount), switchableCount), sorterResults) -> MakeHistoItem2 stageCount switchCount switchableCount sorterResults)
             |> Seq.toArray

         let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
                                 prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
                                 seed sorterCount (histogram|>Array.sumBy(fun a -> a.sorterCount))
         (summary, histogram |> Array.map(fun i-> (MakeHistoLine2 i)))


    let MakeStagePerfHistogram2
                    (randGenerationMode : SorterGenerationMode)
                    (sorterCount:int) 
                    (seed : int) =

        let rgmForFiltering = (randGenerationMode |> RemoveRandomStages)
        let filteringSorter = CreatePrefixedSorter rgmForFiltering
        let filteringSorterLength = filteringSorter.switches.Length

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases rgmForFiltering) 
                            |> Seq.toArray

        let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
            let res = StagedSorter.GetStagePerfAndSwitchUsage 
                        (SwitchTracker.MakePrefixed completeSorterLength filteringSorterLength)
                        stagedSorterDef
                        (randGenerationMode |> To_PrefixStageCount)
                        (SortableIntArray.SortableSeq testSortables)
            (res, stagedSorterDef)

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
            |> Seq.groupBy(fun (switchableCount, sorterResult) -> SorterResult.SwitchAndStageCountsIn sorterResult.stageResults)
            |> Seq.map(fun ((stageCount, switchCount), sorterResults) -> MakeHistoItem stageCount switchCount sorterResults)
            |> Seq.toArray
   
        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%s seed:%d sorterCount:%d resultCount:%d" 
                                prefixedSorter.order completeSorterLength (randGenerationMode |> To_RefSorter_PrefixStages) 
                                seed sorterCount (histogram|>Array.sumBy(fun a -> a.sorterCount))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))
   


     //returns (NumStagesUsed, NumSwitchesUsed, Count)  
    let MakeStageAndSwitchUseHistogram 
                        (randGenerationMode : SorterGenerationMode)
                        (sorterCount:int) 
                        (seed : int) =

        let prefixedSorter = CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases randGenerationMode) 
                            |> Seq.toArray

        let MakeSorterResults (sorterDef:SorterDef) =
            let res = Sorter.UpdateSwitchUses
                                sorterDef
                                (SwitchTracker.MakePrefixed completeSorterLength prefixedSorterLength)
                                prefixedSorterLength
                                (SortableIntArray.SortableSeq testSortables)
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
