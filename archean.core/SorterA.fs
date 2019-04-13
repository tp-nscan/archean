﻿namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections
open Sorting

module SorterA =

    type Sorter<'T> = { sorterDef:SorterDef; switches: array<'T->'T*bool> }
    module Sorter =

        let MakeSorter<'T> (switchmap:Switch->('T->'T*bool)) (sorterDef:SorterDef) =
            { sorterDef=sorterDef; switches=sorterDef.switches |> Array.map(fun i-> (switchmap i)) }
        

        let Sort<'T> (sorter:Sorter<'T>) (sortable: 'T) =
            let wank = Array.init sorter.switches.Length (fun i -> 0)
            let mutable stt = sortable;
            let yab2 dex sw =
                let pr = sw stt
                stt <- fst pr
                if (snd pr) then
                    wank.[dex] <- wank.[dex] + 1
                    
            sorter.switches |> Array.iteri(yab2)
            (wank, stt)
    
        //Run the sortable through the sorter, and return the result and if any of the switches were used
        let SortOneWithResults<'T> (sorter:Sorter<'T>) (sortable: 'T) =
            let mutable stt = sortable;
            let mutable wasUsed = false
            let runSwitch sw =
                let pr = sw stt
                stt <- fst pr
                wasUsed <- wasUsed && (snd pr)
                    
            sorter.switches |> Array.iter(runSwitch)
            (wasUsed, stt)

        //Run the sortable through the sorter, and return the result with the record usage on each switch
        let SortOneAndTrackSwitches<'T> (sorter:Sorter<'T>) (switchTracker:array<int>) (sortable: 'T) =
            let mutable stt = sortable;
            let runSwitch dex sw =
                let pr = sw stt
                stt <- fst pr
                if (snd pr) then
                    switchTracker.[dex] <- switchTracker.[dex] + 1
                    
            sorter.switches |> Array.iteri(runSwitch)
            (switchTracker, stt)

        //Run the sortables through the sorter, and return the result with the record usage on each switch
        let SortManyAndTrackSwitches<'T> (sorter:Sorter<'T>) (switchTracker:int[]) (sortables: seq<'T>) =
            let sorterWithTracker = SortOneAndTrackSwitches sorter switchTracker
            sortables |> Seq.map (fun i -> (sorterWithTracker i) ) |> ignore
            switchTracker
        
        //Run the sortables through the sorter, return the record usage on each switch, and return false if 
        //the checker fails to pass one of the outputs from the sorter
        let SortManyTrackSwitchesAndCheckResults<'T> (checker:'T->bool) (sorter:Sorter<'T>) (switchTracker:int[]) (sortables: seq<'T>) =
            let sorterWithTracker = SortOneAndTrackSwitches sorter switchTracker
            let allGood = sortables |> Seq.map (fun i -> (sorterWithTracker i) ) 
                                    |> Seq.forall(fun i -> checker (snd i))
            (allGood, switchTracker)

        //Run the sortables through the sorter, return the record usage on each switch, and return false if 
        //the checker fails to pass one of the outputs from the sorter
        let SortManyAndTrackSwitchesAndCheckResultsP<'T> (checker:'T->bool) (sorter:Sorter<'T>) 
                                                            (switchTracker:int[]) (sortables: seq<'T>) =
            let sorterWithTracker = SortOneAndTrackSwitches sorter switchTracker
            let allGood = sortables |> Seq.toArray
                                    |> Array.Parallel.map (fun i -> (sorterWithTracker i) ) 
                                    |> Seq.forall (fun i -> checker (snd i))
            (allGood, switchTracker)
            
        //Run the sortables through the sorter, return the number of switches used, and eturn false if 
        //the checker fails to pass one of the outputs from the sorter
        let SortManyGetSwitchCountAndCheckResults<'T> (checker:'T->bool) (sorter:Sorter<'T>) (sortables: seq<'T>) =
            let switchTracker = Array.init sorter.switches.Length (fun i -> 0)
            let res = SortManyAndTrackSwitchesAndCheckResultsP checker sorter switchTracker sortables
            (fst res, (snd res)|> Array.sumBy(fun i -> if (i>0) then 1 else 0))
            
        let MergeTrackerResultsIntoSwitchResults (sorter:Sorter<'T>) (switchTracker:int[]) = 
            seq { for i = 0 to switchTracker.Length - 1 do
                    if (switchTracker.[i] > 0) then
                        yield {switch=sorter.sorterDef.switches.[i]; switchIndex=i; useCount=switchTracker.[i] } }
            |> Seq.toArray


        //Run the sortables through the sorter, return the number of switches used, and return false if 
        //the checker fails to pass one of the outputs from the sorter
        let SortManyAndGetSwitchResults<'T> (checker:'T->bool) (sorter:Sorter<'T>) (sortables: seq<'T>) =
            let switchTracker = Array.init sorter.switches.Length (fun i -> 0)
            let res = SortManyAndTrackSwitchesAndCheckResultsP checker sorter switchTracker sortables
            (fst res, (snd res)|> (MergeTrackerResultsIntoSwitchResults sorter))

        let GetSwitchCountForSorter (sorter:Sorter<int[]>) (sortables:seq<int[]>) =
            let checker t = Combinatorics.IsSorted t
            SortManyGetSwitchCountAndCheckResults<int[]> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<int[]>) (sortables:seq<int[]>) =
            let checker t = Combinatorics.IsSorted t
            SortManyAndGetSwitchResults<int[]> checker sorter sortables



