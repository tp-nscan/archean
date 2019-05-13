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
                                   (weightedSortableSeq: seq<int[]>) =

            let SeqFuncFromArray (wa:int[][]) =
                wa |> Array.map(fun a -> Array.copy a)
                   |> Array.toSeq

            let (_, weightedRes) = 
                RunOnStage stagedSorterDef switchTracker stageIndexToTest weightedSortableSeq 

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

        let runStage ((wss, counts): seq<int[]> * int list) 
                     (stageDex:int) =
            let res = 
                RunOnStage stagedSorterDef switchTracker stageDex wss
                    |> snd
                    |> Seq.toArray
            (res, res.Length::counts)

        let (sortables, stageUseList) = 
            {firstStage .. (stagedSorterDef.stageIndexes.Length - 2)}
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


        | _ -> false