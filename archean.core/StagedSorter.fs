namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections
open Sorting

module StagedSorting =





   let RunSwitchesAndGetResults (switchTracker:int[]) 
                (sorterDef:SorterDef) (sortableGen: _ -> seq<int[]>) = 

        let runSorter (sortable:int[]) =
            {0 .. sorterDef.switches.Length - 1 } 
            |> Seq.iteri(fun i -> (SorterB.runSwitch sorterDef.switches.[i] switchTracker sortable))
            sortable
        
        let sortedItemsList = sortableGen()
                                |> Seq.map(runSorter)
                                |> Seq.toList

        let sset = sortedItemsList |> Set.ofList

        (switchTracker, sset)