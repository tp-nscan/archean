namespace archean.core
open Microsoft.FSharp.Collections
open Sorting
open Sorter


module StagedSorter =

    // returns early if a sort fails on any of the sortables
    let GetSwitchUsagesIfStagedSorterAlwaysWorks 
                (switchTracker:SwitchTracker)
                (stagedSorterDef:StagedSorterDef)
                (startPos:int)
                (sortableSeq: seq<int[]>) =

        let rs (sortable:int[]) = 
            RunSwitchSequenceOnSortable
                stagedSorterDef.sorterDef 
                switchTracker
                { startPos .. (stagedSorterDef.sorterDef.switches.Length - 1) }
                sortable
            
        let allGood = sortableSeq |> Seq.map(rs)
                                  |> Seq.forall(Combinatorics.IsSorted)

        if allGood then
             (allGood, Some (SwitchUsage.CollectTheUsedSwitches 
                                    stagedSorterDef.sorterDef switchTracker))
        else (allGood, None)


    let RunWeightedOnStage
                     (stagedSorterDef:StagedSorterDef) 
                     (switchTracker:SwitchTracker) 
                     (stageIndex:int) 
                     (sortableSeq: seq<int[] * int>) =

            RunSwitchSequenceOnWeightedSortableSeq
                         stagedSorterDef.sorterDef
                         switchTracker 
                         (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageIndex)
                         sortableSeq


    let GetStagePerfAndSwitchUsage (switchTracker:SwitchTracker)
                                   (stagedSorterDef:StagedSorterDef) 
                                   (stageIndexToTest:int)
                                   (sortableSeq: seq<int[] * int>) =

            let SeqFuncFromWghtedArray (wa:(int[]*int)[]) =
                wa |> Array.map(fun a -> Array.copy (fst a))
                   |> Array.toSeq

            let (_, weightedRes) = 
                RunWeightedOnStage stagedSorterDef switchTracker stageIndexToTest sortableSeq 

            let sortablesStage1 =  SeqFuncFromWghtedArray weightedRes
            let suffixStart = stagedSorterDef.stageIndexes.[stageIndexToTest + 1]

            let res = GetSwitchUsagesIfStagedSorterAlwaysWorks 
                            switchTracker
                            stagedSorterDef
                            suffixStart
                            sortablesStage1

            match (snd res) with
            | Some switchUseArray -> (true, Some(weightedRes |> Array.length, switchUseArray))
            | None -> (false, None)
