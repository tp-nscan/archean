namespace archean.core
open Microsoft.FSharp.Collections
open Sorting
open Sorting.SortableIntArray

module Sorter =

    let RunSwitch (switch:Switch) 
                  (switchTracker:SwitchTracker)
                  (sortable:int[]) 
                  (switchDex:int) =

        let lv = sortable.[switch.low]
        let hv = sortable.[switch.hi]
        if(lv > hv) then
            sortable.[switch.hi] <- lv
            sortable.[switch.low] <- hv
            switchTracker.weights.[switchDex] <- 
                switchTracker.weights.[switchDex] + 1


    let RunWeightedSortable  (switch:Switch) 
                             (switchTracker:SwitchTracker) 
                             (sortable:int[], weight:int) 
                             (switchDex:int) =

        let lv = sortable.[switch.low]
        let hv = sortable.[switch.hi]
        if(lv > hv) then
            sortable.[switch.hi] <- lv
            sortable.[switch.low] <- hv
            switchTracker.weights.[switchDex] <- 
                switchTracker.weights.[switchDex] + weight


    let RunSwitchSequence 
                    (startPos:int) 
                    (endPos:int) 
                    (sorterDef:SorterDef) 
                    (switchTracker:SwitchTracker)
                    (sortable:int[]) =

            {startPos .. endPos } 
            |> Seq.iteri(fun i -> (RunSwitch 
                                    sorterDef.switches.[i] 
                                    switchTracker 
                                    sortable))
            sortable

            
    let RunSorterOnASortable 
                (sorterDef:SorterDef) 
                (switchTracker:SwitchTracker) 
                (sortable:int[]) =

            RunSwitchSequence 0 (sorterDef.switches.Length - 1) 
                                    sorterDef switchTracker sortable


    let RecordOnSwitchTracker 
                        (switchTracker:SwitchTracker) 
                        (sorterDef:SorterDef) 
                        (sortableGen: _ -> seq<int[]>) =

        let rs (sortable:int[]) = 
            RunSorterOnASortable sorterDef switchTracker sortable
            |> ignore

        sortableGen() |> Seq.iter(rs)
        switchTracker
    

    // returns early if a sort fails on any of the sortables
    let GetSwitchUsagesIfSorterAlwaysWorks 
                (switchTracker:SwitchTracker) 
                (sorterDef:SorterDef)
                (startPos:int)
                (sortableGen: _ -> seq<int[]>) =

        let rs (sortable:int[]) = 
            RunSwitchSequence
                startPos (sorterDef.switches.Length - 1)
                sorterDef switchTracker sortable
            
        let allGood = sortableGen() |> Seq.map(rs)
                                    |> Seq.forall(Combinatorics.IsSorted)
        if allGood then
             (allGood, Some (SwitchUsage.CollectTheUsedSwitches sorterDef switchTracker))
        else (allGood, None)


    let EvalSorterDef (sorterDef:SorterDef) =
        let startPos = 0
        let switchTracker = SwitchTracker.Make sorterDef.switches.Length

        GetSwitchUsagesIfSorterAlwaysWorks
                    switchTracker
                    sorterDef
                    startPos
                    (Sorting.SortableIntArray.SortableFuncAllBinary sorterDef.order)


    let RunSwitchesAndGetResults 
                (switchTracker:SwitchTracker) 
                (sorterDef:SorterDef) 
                (sortableGen: _ -> seq<int[]>) = 

        let rs (sortable:int[]) = 
            RunSorterOnASortable sorterDef switchTracker sortable

        let sortedItemsList = sortableGen()
                                |> Seq.map(rs)
                                |> Seq.toList

        (switchTracker, sortedItemsList |> Set.ofList)


    let MapAllZeroOneSwitchables (sorterDef:SorterDef) =
        let switchTracker = SwitchTracker.Make sorterDef.switches.Length
        let (switchTracker, sortedItemsList) =  
            RunSwitchesAndGetResults 
                    switchTracker 
                    sorterDef 
                    (SortableIntArray.SortableFuncAllBinary sorterDef.order)
        (
            switchTracker,
            sortedItemsList |> Set.filter(fun stb -> not (Combinatorics.IsSorted stb))
        )


    
    let RunWeightedOnSwitches
                 (switchTracker:SwitchTracker) 
                 (sorterDef:SorterDef) 
                 (switchIndexes:seq<int>)
                 (sortableGen: _ -> seq<int[] * int>) = 

        let runSorter (sortable:int[]*int) =
            switchIndexes 
            |> Seq.iter(fun switchDex -> 
                                (RunWeightedSortable 
                                    sorterDef.switches.[switchDex] 
                                    switchTracker 
                                    sortable 
                                    switchDex))
            fst sortable
        
        let sortedItemsList = sortableGen()
                                |> Seq.map(runSorter)
                                |> Seq.countBy id
                                |> Seq.toArray

        (switchTracker, sortedItemsList)
    

    let RunWeightedOnSorter 
                 (switchTracker:SwitchTracker)
                 (sorterDef:SorterDef)
                 (sortableGen: _ -> seq<int[] * int>) = 

            RunWeightedOnSwitches
                switchTracker 
                sorterDef 
                { 0 .. (sorterDef.switches.Length - 1)}
                sortableGen
    

