namespace archean.core
open Microsoft.FSharp.Collections
open Sorting
open Sorting.SortableIntArray

module Sorter =

    let RunSwitchOnSortable 
                  (switch:Switch) 
                  (switchTracker:SwitchTracker)
                  (switchDex:int) 
                  (sortable:int[]) =

        let lv = sortable.[switch.low]
        let hv = sortable.[switch.hi]
        if(lv > hv) then
            sortable.[switch.hi] <- lv
            sortable.[switch.low] <- hv
            switchTracker.weights.[switchDex] <- 
                switchTracker.weights.[switchDex] + 1


    let RunSwitchOnWeightedSortable  
                    (switch:Switch) 
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


    let RunSwitchSequenceOnSortable 
                    (sorterDef:SorterDef) 
                    (switchTracker:SwitchTracker)
                    (switchIndexes:seq<int>)
                    (sortable:int[]) =

        switchIndexes
            |> Seq.iter(fun i -> (RunSwitchOnSortable
                                    sorterDef.switches.[i] 
                                    switchTracker
                                    i
                                    sortable))
        sortable


    let RunSwitchSequenceOnWeightedSortable 
                    (sorterDef:SorterDef) 
                    (switchTracker:SwitchTracker)
                    (switchIndexes:seq<int>)
                    (weightedSortable:int[]*int) =

        switchIndexes
            |> Seq.iter(fun i -> (RunSwitchOnWeightedSortable 
                                    sorterDef.switches.[i] 
                                    switchTracker 
                                    weightedSortable
                                    i))
        weightedSortable


    let RunSorterOnSortable
                (sorterDef:SorterDef) 
                (switchTracker:SwitchTracker) 
                (sortable:int[]) =

            RunSwitchSequenceOnSortable
                sorterDef 
                switchTracker
                { 0 .. (sorterDef.switches.Length - 1) }             
                sortable


    let RunSorterOnSortableSeq 
                (sorterDef:SorterDef) 
                (switchTracker:SwitchTracker) 
                (sortableSeq: seq<int[]>) = 

        let rs (sortable:int[]) = 
            RunSorterOnSortable sorterDef switchTracker sortable

        let sortedItemsSet = sortableSeq
                                |> Seq.map(rs)
                                |> Set.ofSeq
                                |> Set.toSeq

        (switchTracker, sortedItemsSet)


    let RunSwitchSequenceOnWeightedSortableSeq
                 (sorterDef:SorterDef) 
                 (switchTracker:SwitchTracker) 
                 (switchIndexes:seq<int>)
                 (weightedSortableSeq: seq<int[] * int>) = 

        let rws (weightedSortable:int[]*int) = 
            fst (RunSwitchSequenceOnWeightedSortable 
                    sorterDef 
                    switchTracker 
                    switchIndexes
                    weightedSortable)
        
        let sortedItemsList = weightedSortableSeq
                                |> Seq.map(rws)
                                |> Seq.countBy id
                                |> Seq.toArray

        (switchTracker, sortedItemsList)
        


    let RunSorterOnWeightedSortableSeq
                 (sorterDef:SorterDef)
                 (switchTracker:SwitchTracker)
                 (weightedSortableSeq: seq<int[] * int>) = 

            RunSwitchSequenceOnWeightedSortableSeq
                sorterDef
                switchTracker
                { 0 .. (sorterDef.switches.Length - 1)}
                weightedSortableSeq

    

    // returns early if a sort fails on any of the sortables
    let GetSwitchUsagesIfSorterAlwaysWorks 
                (sorterDef:SorterDef)
                (switchTracker:SwitchTracker) 
                (startPos:int)
                (sortableGen: seq<int[]>) =

        let rs (sortable:int[]) = 
            RunSwitchSequenceOnSortable
                sorterDef
                switchTracker
                {startPos .. (sorterDef.switches.Length - 1)}
                sortable
            
        let allGood = sortableGen |> Seq.map(rs)
                                  |> Seq.forall(Combinatorics.IsSorted)
        if allGood then
             (allGood, Some (SwitchUsage.CollectTheUsedSwitches sorterDef switchTracker))
        else (allGood, None)


    let EvalSorterDef 
            (sorterDef:SorterDef) =
        let startPos = 0
        let switchTracker = SwitchTracker.Make sorterDef.switches.Length

        GetSwitchUsagesIfSorterAlwaysWorks
                    sorterDef
                    switchTracker
                    startPos
                    (Sorting.SortableIntArray.SortableSeqAllBinary sorterDef.order)


    let CondenseSortables 
        (sorterDef:SorterDef) 
        (sortableSeq:seq<int[]>) =

        let switchTracker = SwitchTracker.Make sorterDef.switches.Length
        let (switchTracker, sortedItemsList) =  
            RunSorterOnSortableSeq
                    sorterDef
                    switchTracker
                    sortableSeq
        (
            switchTracker,
            sortedItemsList |> Seq.filter(fun stb -> not (Combinatorics.IsSorted stb))
        )

    let CondenseWeightedSortables 
        (sorterDef:SorterDef) 
        (sortableSeq:seq<int[]*int>) =

        let switchTracker = SwitchTracker.Make sorterDef.switches.Length
        let (switchTracker, sortedItemsList) =  
            RunSorterOnWeightedSortableSeq
                    sorterDef
                    switchTracker
                    sortableSeq
        (
            switchTracker,
            sortedItemsList |> Seq.filter(fun stb -> 
                        not (Combinatorics.IsSorted (fst stb)))
        )

    let CondenseAllZeroOneSortables 
                (sorterDef:SorterDef) =
        CondenseSortables 
                    sorterDef 
                    (SortableIntArray.SortableSeqAllBinary sorterDef.order)
    

    let CondenseAllZeroOneWeightedSortables (sorterDef:SorterDef) =
        CondenseWeightedSortables 
                    sorterDef 
                    (SortableIntArray.WeightedSortableSeqAllBinary sorterDef.order)

    

