namespace archean.core
open Microsoft.FSharp.Collections
open Sorting

module SorterB =

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
    let GetSwitchUsagesForGoodSorters 
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

    
    let RunWeightedSortablesOnSorterSwitchSequenceAndReturnResultsSet 
                 (switchTracker:int[]) (sorterDef:SorterDef) 
                 (startPos:int) (endPos:int)
                 (sortableGen: _ -> seq<int[] * int>) = 

        let runSorter (sortable:int[]*int) =
            {startPos .. endPos} 
            |> Seq.iteri(fun i -> (runWeightedSortables sorterDef.switches.[i] switchTracker sortable))
            fst sortable
        
        let sortedItemsList = sortableGen()
                                |> Seq.map(runSorter)
                                |> Seq.countBy id
                                |> Seq.toArray

        (switchTracker, sortedItemsList)
    

    let RunWeightedSortablesOnSorterAndReturnResultsSet 
                 (switchTracker:int[]) (sorterDef:SorterDef)
                 (sortableGen: _ -> seq<int[] * int>) = 

        RunWeightedSortablesOnSorterSwitchSequenceAndReturnResultsSet
            switchTracker sorterDef 
            0 (sorterDef.switches.Length - 1)
            sortableGen