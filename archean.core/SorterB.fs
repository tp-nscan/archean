namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections
open Sorting

module SorterB =

    let GetSwitchResultsForSorter (switchTracker:int[]) (sorterDef:SorterDef) 
                                   (sortableGen: _ -> seq<int[]>) =

        let runSwitch (sortable:int[]) (switchDex:int)  =
            let sw = sorterDef.switches.[switchDex]
            let lv = sortable.[sw.low]
            let hv = sortable.[sw.hi]
            if(lv > hv) then
                sortable.[sw.hi] <- lv
                sortable.[sw.low] <- hv
                switchTracker.[switchDex] <- switchTracker.[switchDex] + 1

        let runSorter (sortable:int[]) =
            {0 .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (runSwitch sortable))
            
        sortableGen() |> Seq.iter(runSorter)
        switchTracker
    
    // returns early if a sort fails
    let GetSwitchResultsForSorterAndCheckResults (switchTracker:int[]) (sorterDef:SorterDef) 
                                   (sortableGen: _ -> seq<int[]>) =

        let checker t = Combinatorics.IsSorted t

        let runSwitch (sortable:int[]) (switchDex:int)  =
            let sw = sorterDef.switches.[switchDex]
            let lv = sortable.[sw.low]
            let hv = sortable.[sw.hi]
            if(lv > hv) then
                sortable.[sw.hi] <- lv
                sortable.[sw.low] <- hv
                switchTracker.[switchDex] <- switchTracker.[switchDex] + 1

        let runSorter (sortable:int[]) =
            {0 .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (runSwitch sortable))
            sortable

        let allGood = sortableGen() |> Seq.map(runSorter)
                                    |> Seq.forall(fun sortable -> checker sortable)

        (allGood, SwitchResult.MergeTrackerResultsIntoSwitchResults sorterDef switchTracker)


    let GetSwitchAndSwitchableResultsForSorter (switchTracker:int[]) (sorterDef:SorterDef) 
                                                (sortableGen: _ -> seq<int[]>) = 

        let runSwitch (sortable:int[]) (switchDex:int)  =
            let sw = sorterDef.switches.[switchDex]
            let lv = sortable.[sw.low]
            let hv = sortable.[sw.hi]
            if(lv > hv) then
                sortable.[sw.hi] <- lv
                sortable.[sw.low] <- hv
                switchTracker.[switchDex] <- switchTracker.[switchDex] + 1

        let runSorter (sortable:int[]) =
            {0 .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (runSwitch sortable))
            sortable
        
        let sortedItemsList = sortableGen()
                                |> Seq.map(runSorter)
                                |> Seq.toList

        let sset = sortedItemsList |> Set.ofList

        (switchTracker, sset)


    //let GetSwitchResultsForSorter (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) = 
    //    let switchTracker = Array.init sorterDef.switches.Length (fun i -> 0)
    //    _GetSwitchResultsForSorter switchTracker sorterDef sortableGen


    //let GetSwitchResultsForSorterAndCheckResults (switchTracker:int[]) (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) =
    //    _GetSwitchResultsForSorterAndCheckResults switchTracker sorterDef sortableGen


    //let GetSwitchAndSwitchableResultsForSorter (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) = 
    //    let switchTracker = Array.init sorterDef.switches.Length (fun i -> 0)
    //    _GetSwitchAndSwitchableResultsForSorter switchTracker sorterDef sortableGen