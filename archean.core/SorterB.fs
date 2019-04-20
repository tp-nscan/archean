namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections
open Sorting

module SorterB =

    let runSwitch (sorterDef:SorterDef) (switchTracker:int[])
                  (sortable:int[]) (switchDex:int)  =
        let sw = sorterDef.switches.[switchDex]
        let lv = sortable.[sw.low]
        let hv = sortable.[sw.hi]
        if(lv > hv) then
            sortable.[sw.hi] <- lv
            sortable.[sw.low] <- hv
            switchTracker.[switchDex] <- switchTracker.[switchDex] + 1

    let runWeightedSwitch (sorterDef:SorterDef) (switchTracker:int[]) 
                          (sortable:int[], weight:int) (switchDex:int)  =
        let sw = sorterDef.switches.[switchDex]
        let lv = sortable.[sw.low]
        let hv = sortable.[sw.hi]
        if(lv > hv) then
            sortable.[sw.hi] <- lv
            sortable.[sw.low] <- hv
            switchTracker.[switchDex] <- switchTracker.[switchDex] + weight

    let GetSwitchResultsForSorter (switchTracker:int[]) (sorterDef:SorterDef) 
                                  (startPos:int) (sortableGen: _ -> seq<int[]>) =

        let runSorter (sortable:int[]) =
            {startPos .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (runSwitch sorterDef switchTracker sortable))
            
        sortableGen() |> Seq.iter(runSorter)
        switchTracker
    
    // returns early if a sort fails on any of the sortables
    let GetSwitchResultsForGoodSorters 
                (switchTracker:int[]) (sorterDef:SorterDef) 
                (startPos:int) (sortableGen: _ -> seq<int[]>) =

        let checker t = Combinatorics.IsSorted t
        
        let runSorter (sortable:int[]) =
            {startPos .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (runSwitch sorterDef switchTracker sortable))
            sortable

        let allGood = sortableGen() |> Seq.map(runSorter)
                                    |> Seq.forall(fun sortable -> checker sortable)
        if allGood then
             (allGood, Some (SwitchResult.CollectTheUsedSwitches sorterDef switchTracker))
        else (allGood, None)


    let RunSwitchesAndGetResults (switchTracker:int[]) 
                (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) = 

        let runSorter (sortable:int[]) =
            {0 .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (runSwitch sorterDef switchTracker sortable))
            sortable
        
        let sortedItemsList = sortableGen()
                                |> Seq.map(runSorter)
                                |> Seq.toList

        let sset = sortedItemsList |> Set.ofList

        (switchTracker, sset)


    let RunWeightedSwitchesAndGetWeightedResults (switchTracker:int[]) 
                 (sorterDef:SorterDef) (sortableGen: _ -> seq<int[] * int>) = 

        let runSorter (sortable:int[]*int) =
            {0 .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (runWeightedSwitch sorterDef switchTracker sortable))
            fst sortable
        
        let sortedItemsList = sortableGen()
                                |> Seq.map(runSorter)
                                |> Seq.countBy id
                                |> Seq.toArray

        (switchTracker, sortedItemsList)
