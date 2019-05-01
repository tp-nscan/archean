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
            |> Seq.iter(fun i -> (RunSwitch 
                                    sorterDef.switches.[i] 
                                    switchTracker 
                                    sortable
                                    i))
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
                (sortableGen: seq<int[]>) =

        let rs (sortable:int[]) = 
            RunSwitchSequence
                startPos (sorterDef.switches.Length - 1)
                sorterDef switchTracker sortable
            
        let allGood = sortableGen |> Seq.map(rs)
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
                    (Sorting.SortableIntArray.SortableSeqAllBinary sorterDef.order)


    let RunSortables 
                (switchTracker:SwitchTracker) 
                (sorterDef:SorterDef) 
                (sortableSeq: seq<int[]>) = 

        let rs (sortable:int[]) = 
            RunSorterOnASortable sorterDef switchTracker sortable

        //let sortedItemsList = sortableSeq
        //                        |> Seq.map(rs)
        //                        |> Seq.toList

        let sortedItemsSet = sortableSeq
                                |> Seq.map(rs)
                                |> Set.ofSeq
                                |> Set.toSeq

        (switchTracker, sortedItemsSet)

    
    let RunSortables2 
                (switchTracker:SwitchTracker) 
                (sorterDef:SorterDef) 
                (sortableSeq: seq<int[]>) = 

        let rs (sortable:int[]) = 
            RunSorterOnASortable sorterDef switchTracker sortable

        let sortedItemsList = sortableSeq
                                |> Seq.map(rs)
                                |> Seq.toList

        (switchTracker, sortedItemsList |> Set.ofList)



    let MapAllZeroOneSwitchables (sorterDef:SorterDef) =
        let switchTracker = SwitchTracker.Make sorterDef.switches.Length
        let (switchTracker, sortedItemsList) =  
            RunSortables 
                    switchTracker 
                    sorterDef 
                    (SortableIntArray.SortableSeqAllBinary sorterDef.order)
        (
            switchTracker,
            sortedItemsList |> Seq.filter(fun stb -> not (Combinatorics.IsSorted stb))
        )

   
    let RunWeightedOnSwitches0
                 (switchTracker:SwitchTracker) 
                 (sorterDef:SorterDef) 
                 (switchIndexes:seq<int>)
                 (sortableSeq: seq<int[] * int>) = 

        let runSorter (sortable:int[]*int) =
            switchIndexes 
            |> Seq.iter(fun switchDex -> 
                                (RunWeightedSortable 
                                    sorterDef.switches.[switchDex] 
                                    switchTracker 
                                    sortable 
                                    switchDex))
            fst sortable
        
        let sortedItemsList = sortableSeq
                                |> Seq.map(runSorter)
                                |> Seq.map(fun r -> (r, 1))
                                |> Seq.toArray

        (switchTracker, sortedItemsList)

    
    let RunWeightedOnSwitches
                 (switchTracker:SwitchTracker) 
                 (sorterDef:SorterDef) 
                 (switchIndexes:seq<int>)
                 (sortableSeq: seq<int[] * int>) = 

        let runSorter (sortable:int[]*int) =
            switchIndexes 
            |> Seq.iter(fun switchDex -> 
                                (RunWeightedSortable 
                                    sorterDef.switches.[switchDex] 
                                    switchTracker 
                                    sortable 
                                    switchDex))
            fst sortable
        
        let sortedItemsList = sortableSeq
                                |> Seq.map(runSorter)
                                |> Seq.countBy id
                                |> Seq.toArray

        (switchTracker, sortedItemsList)
    


    let RunWeightedSortables 
                 (switchTracker:SwitchTracker)
                 (sorterDef:SorterDef)
                 (sortableGen: seq<int[] * int>) = 

            RunWeightedOnSwitches
                switchTracker 
                sorterDef 
                { 0 .. (sorterDef.switches.Length - 1)}
                sortableGen
    

