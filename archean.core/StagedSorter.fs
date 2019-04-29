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
                (sortableGen: seq<int[]>) =

        let rs (sortable:int[]) = 
            RunSwitchSequence
                startPos (stagedSorterDef.sorterDef.switches.Length - 1)
                stagedSorterDef.sorterDef switchTracker sortable
            
        let allGood = sortableGen |> Seq.map(rs)
                                    |> Seq.forall(Combinatorics.IsSorted)
        if allGood then
             (allGood, Some (SwitchUsage.CollectTheUsedSwitches stagedSorterDef.sorterDef switchTracker))
        else (allGood, None)


    let RunWeightedOnStage
                     (stagedSorterDef:StagedSorterDef) 
                     (switchTracker:SwitchTracker) 
                     (stageNum:int) (sortableGen: seq<int[] * int>) =

            let yuk = (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageNum)
                                |> Seq.toArray

            RunWeightedOnSwitches
                         switchTracker 
                         stagedSorterDef.sorterDef
                         (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageNum)
                         sortableGen


    let GetStagePerfAndSwitchUsage (switchTracker:SwitchTracker)
                                   (stagedSorterDef:StagedSorterDef) 
                                   (stageToTest:int)
                                   (sortableGen: seq<int[] * int>) =

            let SeqFuncFromWghtedArray (wa:(int[]*int)[]) =
                wa |> Array.map(fun a -> Array.copy (fst a))
                   |> Array.toSeq

            let (_, weightedRes) = 
                RunWeightedOnStage stagedSorterDef switchTracker stageToTest sortableGen 

            let sortablesStage1 =  SeqFuncFromWghtedArray weightedRes
            let suffixStart = stagedSorterDef.stageIndexes.[stageToTest + 1]

            let res = GetSwitchUsagesIfStagedSorterAlwaysWorks 
                            switchTracker
                            stagedSorterDef
                            suffixStart
                            sortablesStage1

            match (snd res) with
            | Some switchUseArray -> (true, Some(weightedRes |> Array.length, switchUseArray))
            | None -> (false, None)
