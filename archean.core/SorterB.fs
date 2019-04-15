namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections
open Sorting

module SorterB =

    let RunSorterDef (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) = 
        let switchTracker = Array.init sorterDef.switches.Length (fun i -> 0)

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
            
        sortableGen()
        |> Seq.iter(runSorter)

        switchTracker



    let RunSorterDef2 (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) = 
        let switchTracker = Array.init sorterDef.switches.Length (fun i -> 0)

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