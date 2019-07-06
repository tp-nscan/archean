namespace archean.core
open Microsoft.FSharp.Collections
open Sorting

module Sorter =

    let RunSwitchOnSortable 
                  (switch:ISwitch) 
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



    let RunSwitchSeqOnSortable 
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


    let RunSorterOnSortable
                (sorterDef:SorterDef) 
                (switchTracker:SwitchTracker) 
                (sortable:int[]) =

            RunSwitchSeqOnSortable
                sorterDef 
                switchTracker
                { 0 .. (sorterDef.switches.Length - 1) }             
                sortable


    let RunSwitchSeqOnSortableSeq
                 (sorterDef:SorterDef) 
                 (switchTracker:SwitchTracker) 
                 (switchIndexes:seq<int>)
                 (sortableSeq: seq<int[]>) = 

        let rws (weightedSortable:int[]) =
            RunSwitchSeqOnSortable 
                    sorterDef 
                    switchTracker 
                    switchIndexes
                    weightedSortable
    
        let sortedItemsList = sortableSeq
                                |> Seq.map(rws)
                                |> Seq.countBy id
                                |> Seq.map fst
                                |> Seq.toArray

        (switchTracker, sortedItemsList)


    let RunSorterOnSortableSeq 
                (sorterDef:SorterDef) 
                (switchTracker:SwitchTracker) 
                (sortableSeq: seq<int[]>) = 

        RunSwitchSeqOnSortableSeq
                    sorterDef
                    switchTracker
                    {0 .. (sorterDef.switches.Length - 1)}
                    sortableSeq


    // returns early if a sort fails on any of the sortables
    let UpdateSwitchUses 
                (sorterDef:SorterDef)
                (switchTracker:SwitchTracker) 
                (startPos:int)
                (sortableSeq: seq<int[]>) =

        let rs (sortable:int[]) = 
            RunSwitchSeqOnSortable
                sorterDef
                switchTracker
                {startPos .. (sorterDef.switches.Length - 1)}
                sortable
            
        let allGood = sortableSeq 
                        |> Seq.map(rs)
                        |> Seq.forall(Combinatorics.IsSorted)

        if allGood then
             (allGood, Some (SwitchUsage.CollectTheUsedSwitches sorterDef switchTracker))
        else (allGood, None)


    let Eval (sorterDef:SorterDef) =
        let startPos = 0
        let switchTracker = SwitchTracker.Make sorterDef.switches.Length

        UpdateSwitchUses
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
            sortedItemsList // |> Seq.filter(fun stb -> not (Combinatorics.IsSorted stb))
        )


    let CondenseAllZeroOneSortables 
                (sorterDef:SorterDef) =
        CondenseSortables 
                    sorterDef 
                    (SortableIntArray.SortableSeqAllBinary sorterDef.order)
   

    

