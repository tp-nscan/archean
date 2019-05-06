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


    let RunWeightedOnStage
                     (stagedSorterDef:StagedSorterDef) 
                     (switchTracker:SwitchTracker) 
                     (stageIndex:int) 
                     (weightedsortableSeq: seq<int[] * int>) =

            RunSwitchSeqOnWeightedSortableSeq
                         stagedSorterDef.sorterDef
                         switchTracker 
                         (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageIndex)
                         weightedsortableSeq


    let RunOnStage
                (stagedSorterDef:StagedSorterDef) 
                (switchTracker:SwitchTracker) 
                (stageIndex:int) 
                (weightedsortableSeq: seq<int[]>) =

            RunSwitchSeqOnSortableSeq
                         stagedSorterDef.sorterDef
                         switchTracker 
                         (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageIndex)
                         weightedsortableSeq


    let GetStagePerfAndSwitchUsage (switchTracker:SwitchTracker)
                                   (stagedSorterDef:StagedSorterDef)
                                   (stageIndexToTest:int)
                                   (weightedSortableSeq: seq<int[] * int>) =

            let SeqFuncFromWghtedArray (wa:(int[]*int)[]) =
                wa |> Array.map(fun a -> Array.copy (fst a))
                   |> Array.toSeq

            let (_, weightedRes) = 
                RunWeightedOnStage stagedSorterDef switchTracker stageIndexToTest weightedSortableSeq 

            let sortablesStage1 =  SeqFuncFromWghtedArray weightedRes
            let suffixStart = stagedSorterDef.stageIndexes.[stageIndexToTest + 1]

            let res = Sorter.UpdateSwitchUses
                            stagedSorterDef.sorterDef
                            switchTracker
                            suffixStart
                            sortablesStage1

            match (snd res) with
            | Some switchUseArray -> (true, Some(weightedRes |> Array.length, switchUseArray))
            | None -> (false, None)



    let GetStageWisePerf (stagedSorterDef:StagedSorterDef) =

        let switchTracker = SwitchTracker.Make stagedSorterDef.sorterDef.switches.Length
        let sortableSeq = SortableIntArray.SortableSeqAllBinary 
                                        stagedSorterDef.sorterDef.order

        let startState = (sortableSeq |> Seq.toArray, [])

        let runStage ((wss, counts): seq<int[]> * int list) (stageDex:int) =
            let res = 
                RunOnStage stagedSorterDef switchTracker stageDex wss
                    |> snd
                    |> Seq.toArray
            (res, res.Length::counts)

        let (sortables, stageUseList) = 
            {0 .. (stagedSorterDef.stageIndexes.Length - 2)}
                    |> Seq.fold(fun acc i -> runStage acc i) startState

        (switchTracker, stageUseList |> List.rev)


    let GetStageWiseWeightedPerf (stagedSorterDef:StagedSorterDef) =

        let switchTracker = SwitchTracker.Make stagedSorterDef.sorterDef.switches.Length
        let weightedSortableSeq = SortableIntArray.WeightedSortableSeqAllBinary 
                                        stagedSorterDef.sorterDef.order

        let startState = (weightedSortableSeq |> Seq.toArray, [])

        let runStage ((wss, counts): seq<int[]*int> * int list) (stageDex:int) =
            let res = 
                RunWeightedOnStage stagedSorterDef switchTracker stageDex wss
                    |> snd
                    |> Seq.toArray
            (res, res.Length::counts)

        let (sortables, stageUseList) = 
            {0 .. (stagedSorterDef.stageIndexes.Length - 2)}
                    |> Seq.fold(fun acc i -> runStage acc i) startState

        (switchTracker, stageUseList |> List.rev)



    let GetStageWisePerf0 (stagedSorterDef:StagedSorterDef) =
        let switchTracker = SwitchTracker.Make stagedSorterDef.sorterDef.switches.Length
        let weightedSortableSeq = SortableIntArray.WeightedSortableSeqAllBinary 
                                        stagedSorterDef.sorterDef.order

        let weightedRes = 
                RunWeightedOnStage stagedSorterDef switchTracker 0 weightedSortableSeq
                |> snd
                |> SortableIntArray.NormWeightedSortableSeq
                |> Seq.toArray

        let weightedRes2 = 
                RunWeightedOnStage stagedSorterDef switchTracker 1 weightedRes
                |> snd
                |> SortableIntArray.NormWeightedSortableSeq
                |> Seq.toArray

        let weightedRes3 = 
                RunWeightedOnStage stagedSorterDef switchTracker 2 weightedRes2
                |> snd
                |> SortableIntArray.NormWeightedSortableSeq
                |> Seq.toArray

        true