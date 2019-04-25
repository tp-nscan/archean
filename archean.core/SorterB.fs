namespace archean.core
open Microsoft.FSharp.Collections
open Sorting

module SorterB =

    let MakeSwitchTracker (sorterDef: SorterDef) =
        Array.init sorterDef.switches.Length (fun i -> 0)

    let runSwitch (switch:Switch) (switchTracker:int[])
                  (sortable:int[]) (switchDex:int) =
        let lv = sortable.[switch.low]
        let hv = sortable.[switch.hi]
        if(lv > hv) then
            sortable.[switch.hi] <- lv
            sortable.[switch.low] <- hv
            switchTracker.[switchDex] <- switchTracker.[switchDex] + 1


    let runWeightedSortables (switch:Switch) (switchTracker:int[]) 
                             (sortable:int[], weight:int) (switchDex:int) =
        let lv = sortable.[switch.low]
        let hv = sortable.[switch.hi]
        if(lv > hv) then
            sortable.[switch.hi] <- lv
            sortable.[switch.low] <- hv
            switchTracker.[switchDex] <- switchTracker.[switchDex] + weight


    let runSorterSwitchSequence (startPos:int) (endPos:int) 
                                (sorterDef:SorterDef) (switchTracker:int[]) 
                                (sortable:int[])  =
            {startPos .. endPos } 
            |> Seq.iteri(fun i -> (runSwitch sorterDef.switches.[i] switchTracker sortable))
            sortable

            
    let runSorter (sorterDef:SorterDef) (switchTracker:int[]) 
                        (sortable:int[]) =
            runSorterSwitchSequence 0 (sorterDef.switches.Length - 1) 
                                    sorterDef switchTracker sortable


    let GetSwitchResultsForSorter (switchTracker:int[]) (sorterDef:SorterDef) 
                                  (sortableGen: _ -> seq<int[]>) =
        let rs (sortable:int[]) = 
            runSorter sorterDef switchTracker sortable
            |> ignore

        sortableGen() |> Seq.iter(rs)
        switchTracker
    

    // returns early if a sort fails on any of the sortables
    let GetSwitchUsagesIfSorterAlwaysWorks 
                (switchTracker:int[]) (sorterDef:SorterDef)
                (startPos:int)
                (sortableGen: _ -> seq<int[]>) =

        let rs (sortable:int[]) = 
            runSorterSwitchSequence
                startPos (sorterDef.switches.Length - 1)
                sorterDef switchTracker sortable
            
        let allGood = sortableGen() |> Seq.map(rs)
                                    |> Seq.forall(Combinatorics.IsSorted)
        if allGood then
             (allGood, Some (SwitchUsage.CollectTheUsedSwitches sorterDef switchTracker))
        else (allGood, None)


    let RunSwitchesAndGetResults (switchTracker:int[]) 
                (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) = 

        let rs (sortable:int[]) = 
            runSorter sorterDef switchTracker sortable

        let sortedItemsList = sortableGen()
                                |> Seq.map(rs)
                                |> Seq.toList

        (switchTracker, sortedItemsList |> Set.ofList)

    
    let RunWeightedOnSwitches
                 (switchTracker:int[]) (sorterDef:SorterDef) 
                 (switchIndexes:seq<int>)
                 (sortableGen: _ -> seq<int[] * int>) = 

        let runSorter (sortable:int[]*int) =
            switchIndexes 
            |> Seq.iter(fun i -> (runWeightedSortables sorterDef.switches.[i] switchTracker sortable i))
            fst sortable
        
        let sortedItemsList = sortableGen()
                                |> Seq.map(runSorter)
                                |> Seq.countBy id
                                |> Seq.toArray

        (switchTracker, sortedItemsList)
    

    let RunWeightedOnSorter 
                 (switchTracker:int[]) (sorterDef:SorterDef)
                 (sortableGen: _ -> seq<int[] * int>) = 

            RunWeightedOnSwitches
                switchTracker sorterDef 
                { 0 .. (sorterDef.switches.Length - 1)}
                sortableGen
    

    let RunWeightedOnStage
                     (stagedSorterDef:StagedSorterDef) (switchTracker:int[]) 
                     (stageNum:int) (sortableGen: _ -> seq<int[] * int>) =
                  
            RunWeightedOnSwitches
                         switchTracker stagedSorterDef.sorterDef
                         (StagedSorterDef.GetSwitchIndexesForStage stagedSorterDef stageNum)
                         sortableGen


    let GetStagePerfAndSwitchUsage (switchTracker:int[])
                                   (stagedSorterDef:StagedSorterDef) 
                                   (stageToTest:int)
                                   (sortableGen: _ -> seq<int[] * int>) =

            let SeqFuncFromWghtedArray (wa:(int[]*int)[]) () =
                wa |> Array.map(fun a -> Array.copy (fst a))
                   |> Array.toSeq

            let (switchTrack, weightedRes) = RunWeightedOnStage stagedSorterDef switchTracker stageToTest sortableGen 

            let sortablesStage1 =  SeqFuncFromWghtedArray weightedRes
            let suffixStart = stagedSorterDef.stageIndexes.[stageToTest + 1]

            let res = GetSwitchUsagesIfSorterAlwaysWorks 
                            switchTrack
                            stagedSorterDef.sorterDef
                            suffixStart
                            sortablesStage1

            match (snd res) with
            | Some swithcUseArray -> (true, Some(weightedRes |> Array.length, swithcUseArray))
            | None -> (false, None)

    //let GetSwitchUsagesIfSorterAlwaysWorks 
    //            (switchTracker:int[]) (sorterDef:SorterDef)
    //            (startPos:int)
    //            (sortableGen: _ -> seq<int[]>) =




           // let (switchTrack1, weightedRes1) = RunWeightedOnStage stagedSorterDef switchTrack 1 sortablesStage1