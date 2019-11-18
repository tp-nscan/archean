namespace archean.core
open Microsoft.FSharp.Collections
open Sorting
open Sorter


module StagedSorter =

    let TruncateStages 
                    (remainingStageCount:int) 
                    (stagedSorterDef:StagedSorterDef) =
        { 
            StagedSorterDef.sorterDef = 
                {SorterDef.order = stagedSorterDef.sorterDef.order; 
                 SorterDef.switches = stagedSorterDef.sorterDef.switches
                                      |> Seq.take stagedSorterDef.stageIndexes.[remainingStageCount]
                                      |> Seq.toArray
                 };

            StagedSorterDef.stageIndexes = stagedSorterDef.stageIndexes
                                           |> Seq.take remainingStageCount
                                           |> Seq.toArray
        }

    let RunSorterStageOnSortable
                (stagedSorterDef:StagedSorterDef) 
                (switchTracker:SwitchTracker) 
                (stageIndex:int) 
                (sortable: int[]) =

            RunSwitchSeqOnSortable
                            stagedSorterDef.sorterDef
                            switchTracker 
                            (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageIndex)
                            sortable


    let CondenseSortableSeqUsingStage
                (stagedSorterDef:StagedSorterDef) 
                (switchTracker:SwitchTracker) 
                (stageIndex:int) 
                (sortableSeq: seq<int[]>) =

            CondenseSortableSeqWithSwitchSeq
                         stagedSorterDef.sorterDef
                         switchTracker 
                         (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageIndex)
                         sortableSeq


    let ShowSortableProgressByStage
                (stagedSorterDef:StagedSorterDef) 
                (sortable:int[]) =
                
        let switchTracker = SwitchTracker.Make stagedSorterDef.sorterDef.switches.Length

        let runStage ((sortable, sortableStageHist): int[] * int[] list) 
                     (stageDex:int) =
            let res = 
                RunSorterStageOnSortable
                    stagedSorterDef
                    switchTracker
                    stageDex
                    (sortable |> Array.copy)
            (res, res::sortableStageHist)

        let res = {0 .. ( (StagedSorterDef.GetStageCount stagedSorterDef) - 1)}
                  |> Seq.fold(fun acc i -> runStage acc i) (sortable, [])

        (res, switchTracker)

 

    let GetStagePerfAndSwitchUsage (switchTracker:SwitchTracker)
                                   (stagedSorterDef:StagedSorterDef)
                                   (stageIndexToTest:int)
                                   (sortableSeq: seq<int[]>) =

            let SeqFuncFromArray (wa:int[][]) =
                wa |> Array.map(fun a -> Array.copy a)
                   |> Array.toSeq

            let (_, weightedRes) = 
                CondenseSortableSeqUsingStage stagedSorterDef switchTracker stageIndexToTest sortableSeq 

            let sortablesStage1 =  SeqFuncFromArray weightedRes
            let suffixStart = stagedSorterDef.stageIndexes.[stageIndexToTest + 1]

            let res = Sorter.UpdateSwitchUses
                                stagedSorterDef.sorterDef
                                switchTracker
                                suffixStart
                                sortablesStage1

            match (snd res) with
            | Some switchUseArray -> (true, Some(weightedRes |> Array.length, switchUseArray))
            | None -> (false, None)


    let StageWisePerf (firstStage:int) (stagedSorterDef:StagedSorterDef) (sortableSeq:seq<int[]>) =

        let switchTracker = SwitchTracker.Make stagedSorterDef.sorterDef.switches.Length
        let startState = (sortableSeq |> Seq.toArray, [])

        let runStage ((wgtSortSeq, counts): seq<int[]> * int list) 
                     (stageDex:int) =
            let res = 
                CondenseSortableSeqUsingStage stagedSorterDef switchTracker stageDex wgtSortSeq
                    |> snd
                    |> Seq.toArray
            (res, res.Length::counts)

        let (sortables, stageUseList) = 
            {firstStage .. ( (StagedSorterDef.GetStageCount stagedSorterDef) - 1)}
                    |> Seq.fold(fun acc i -> runStage acc i) startState

        (switchTracker, stageUseList |> List.rev)


    let StageWisePerf0 (stagedSorterDef:StagedSorterDef) =
        let sortableSeq = SortableIntArray.SortableSeqAllBinary 
                                        stagedSorterDef.sorterDef.order
        StageWisePerf 0 stagedSorterDef sortableSeq


    let StageWisePerf4 (stagedSorterDef:StagedSorterDef) =
        let sortableSeq = ComboCounter.Filtered2Stage4BlockArrays 
                                        stagedSorterDef.sorterDef.order
        StageWisePerf 0 stagedSorterDef sortableSeq


    let StageWisePerf8 (stagedSorterDef:StagedSorterDef) =
        let sortableSeq = ComboCounter.Filtered3Stage8BlockArrays 
                                        stagedSorterDef.sorterDef.order
        StageWisePerf 3 stagedSorterDef sortableSeq


    let equalsOn f x (yobj:obj) =
        match yobj with
        | T as y -> (f x = f y)