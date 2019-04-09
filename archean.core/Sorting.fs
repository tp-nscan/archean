namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections

module Sorting =

    type Switch = {low:int;hi:int}
    type Stage = {switches:Switch list}
    module Switch =
        let SwitchSeqFromIntArray (pArray:int[]) =
             seq { for i = 0 to pArray.Length - 1 do
                    let j = pArray.[i]
                    if ((j > i ) && (i = pArray.[j]) ) then
                        yield { low=i; hi=j; } }

        let SwitchSeqFromPermutation (p:Permutation) =
            SwitchSeqFromIntArray (Permutation.value p)
         
        let SwitchSeqFromPolyCycle (p:TwoCycleIntArray) =
            SwitchSeqFromIntArray (TwoCycleIntArray.value p)


    type SwitchSet = {order:int; switches: array<Switch>}
    module SwitchSet =
   
        let ForOrder (order:int) =
                {
                    order=order;
                    switches=seq {for i = 0 to order - 1 do
                                    for j = 0 to i - 1 do
                                        yield { low=j; hi=i; } }
                    |> Seq.toArray
                }
         
        let MakeStagePackedSwitchSeq (rnd:Random) (order: int) =
            let aa (rnd:Random)  = 
                (TwoCycleIntArray.MakeRandomPolyCycle rnd order)
                        |> Switch.SwitchSeqFromPolyCycle
            seq { while true do yield! (aa rnd) }


     

    type SorterDef = {order:int; switches: array<Switch>}
    module SorterDef =

        type RandGenerationMode = 
            | LooseSwitches
            | FullStage

        let CreateRand (switchSet:SwitchSet) (len: int) (rnd : Random) =
            {
                SorterDef.order=switchSet.order;
                switches = seq { for i = 0 to len - 1 do
                                    yield switchSet.switches.[rnd.Next(0, switchSet.switches.Length)] }
                |> Seq.toArray
            }

        let CreateRandom (order:int) (len: int) (rnd : Random) =
            CreateRand (SwitchSet.ForOrder order) len rnd

        let CreateRandomPackedStages (order:int) (len: int) (rnd : Random) =
            {
                SorterDef.order=order;
                switches = (SwitchSet.MakeStagePackedSwitchSeq rnd order)
                                |> Seq.take len
                                |> Seq.toArray
            }

        let FromPermutations (switchSet:SwitchSet) (len: int) =
            {
                SorterDef.order=switchSet.order;
                switches = seq { for i = 0 to len - 1 do
                                    yield switchSet.switches.[i] }
                |> Seq.toArray
            }

        let CreateRandomSorterDef (order:int) (sorterLen: int) (randGenerationMode : RandGenerationMode) (rnd : Random) =
            match randGenerationMode with
            | LooseSwitches -> CreateRandom order sorterLen rnd
            | FullStage -> CreateRandomPackedStages order sorterLen rnd


    type SwitchResult = {switch:Switch; switchIndex:int; useCount:int}

    type StageResult = {stageIndex:int; switchResults:SwitchResult list}


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


        let MergeSwitchResultsIntoStageResults (order:int) (switchResults:SwitchResult[]) =
            let mutable stageTracker = Array.init order (fun i -> false)
            let switchResultsForStage = new ResizeArray<SwitchResult>()
            let mutable curStage = 0

            seq { for i = 0 to switchResults.Length - 1 do
                    let curSwitch = switchResults.[i].switch
                    if (stageTracker.[curSwitch.hi] || stageTracker.[curSwitch.low] ) then
                        yield { StageResult.stageIndex=curStage; 
                                switchResults=switchResultsForStage |> Seq.toList }
                        stageTracker <- Array.init order (fun i -> false)
                        switchResultsForStage.Clear()
                        curStage <- curStage + 1

                    stageTracker.[curSwitch.hi] <- true
                    stageTracker.[curSwitch.low] <- true
                    switchResultsForStage.Add switchResults.[i]
                    if (i = switchResults.Length - 1) then
                        yield { StageResult.stageIndex=curStage; 
                                switchResults=switchResultsForStage |> Seq.toList }
                }
            |> Seq.toList


        //Run the sortables through the sorter, return the number of switches used, and return false if 
        //the checker fails to pass one of the outputs from the sorter
        let SortManyAndGetSwitchResults<'T> (checker:'T->bool) (sorter:Sorter<'T>) (sortables: seq<'T>) =
            let switchTracker = Array.init sorter.switches.Length (fun i -> 0)
            let res = SortManyAndTrackSwitchesAndCheckResultsP checker sorter switchTracker sortables
            (fst res, (snd res)|> (MergeTrackerResultsIntoSwitchResults sorter))


    type SortableIntArray = {values:int[]}
    module SortableIntArray =
    
        let Identity (order: int) = { SortableIntArray.values = [|0 .. order-1|] }
        let apply f (p:SortableIntArray) = f p.values
        let value p = apply id p

        let SwitchFuncForSwitch (sw:Switch) =
            fun (x:int[]) -> 
                if (x.[sw.low] > x.[sw.hi]) then
                    let lv = x.[sw.low]
                    x.[sw.low] <- x.[sw.hi]
                    x.[sw.hi] <- lv
                    (x, true)
                else (x, false)

        let CreateRandom (order:int) (rnd : Random) =
            Permutation.CreateRandom rnd order
            |> Seq.map(fun i -> { SortableIntArray.values = Permutation.value i })
        
        let IsSorted (values:int[]) =
            seq{for i=1 to values.Length - 1 do
                    if values.[i-1] > values.[i] then
                        yield false} |> Seq.forall id

        let AllBinaryTestCases (order:int) =
            {0 .. (1 <<< order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray order i)

        let GetSwitchCountForSorter (sorter:Sorter<int[]>) (sortables:seq<int[]>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<int[]> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<int[]>) (sortables:seq<int[]>) =
            let checker t = IsSorted t
            Sorter.SortManyAndGetSwitchResults<int[]> checker sorter sortables

    
    type SorterResult = {sorterDef:SorterDef; stageResults:StageResult list}
    module SorterResult =
        let SorterResultKey (stageResults:StageResult list) =
            (stageResults.Length, stageResults |> List.fold(fun s i -> s + i.switchResults.Length) 0)

        let MakeSorterResult (sorterDef:SorterDef) (switchResults:SwitchResult[]) =
            {sorterDef=sorterDef; stageResults=Sorter.MergeSwitchResultsIntoStageResults sorterDef.order switchResults}


    module SortableGen =
    
        let sb = new System.Text.StringBuilder()
        let myPrint format = Printf.bprintf sb format  

        let tabN (n:int) = 
           {1 .. n} |> Seq.fold (fun (s:string) _ -> s + "    " ) ""

        let tab = "    "

        let SgType (n:int) =
            sprintf "Sortable%d" n

        let SgDef (n:int) =
            myPrint "\n%stype Sortable%d = { " tab n
            { 0 .. (n-1)} |> Seq.iter (fun i -> myPrint "sw%d : int; " i)
            myPrint "}\n"

        let Order (n:int) =
            myPrint "%slet Order = %d\n\n" (tabN 2) n

        let Identity (n:int) =
            myPrint "%slet Identity = {%s.sw0 = 0; " (tabN 2) (SgType n)
            { 1 .. (n-1)} |> Seq.iter (fun i -> myPrint "sw%d = %d; " i i)
            myPrint "}\n\n"
        
        let FromArray (n:int) =
            myPrint "%slet FromArray (ia : int[]) =\n" (tabN 2)
            myPrint "%s{ Sortable%d.sw0 = ia.[0]; " (tabN 3) n
            { 1 .. (n-1)} |> Seq.iter (fun i -> myPrint "sw%d = ia.[%d]; " i i)
            myPrint "}\n\n"

        let ToArray (n : int) =
            myPrint "%slet ToArray (s : %s) =\n" (tabN 2) (SgType n)
            myPrint "%s[| " (tabN 3)
            { 0 .. (n-1)} |> Seq.iter (fun i -> myPrint "s.sw%d; " i)
            myPrint "|]\n\n"

        let AllBinaryTestCases (n : int) =
            myPrint "%slet AllBinaryTestCases =\n" (tabN 2)
            myPrint "%s{0 .. (1 <<< Order) - 1}\n" (tabN 3)
            myPrint "%s|> Seq.map (fun i -> Combinatorics.ToIntArray Order i)\n" (tabN 3)
            myPrint "%s|> Seq.map (fun i -> FromArray i)\n\n" (tabN 3)

        let IsSorted (n : int) =
            let falseCond (swN:int) =
                sprintf "%selif (s.sw%d > s.sw%d) then\n%sfalse\n" (tabN 3) swN (swN+1) (tabN 4)

            myPrint "%slet IsSorted (s : %s) =\n" (tabN 2) (SgType n)
            myPrint "%sif(s.sw0 > s.sw1) then\n%sfalse\n" (tabN 3) (tabN 4)
            { 1 .. (n-2)} |> Seq.iter (fun i -> myPrint "%s" (falseCond i))
            myPrint "%selse\n%strue\n\n" (tabN 3) (tabN 4)

        let CreateRandom (n : int) =
            myPrint "%slet CreateRandom (rnd : Random) =\n" (tabN 2)
            myPrint "%slet fil = Permutation.CreateRandom rnd %d |> Seq.item 0 |> Permutation.value\n" (tabN 3) n
            myPrint "%s{ Sortable%d.sw0 = fil.[0]; " (tabN 3) n
            { 1 .. (n-1)} |> Seq.iter (fun i -> myPrint "sw%d = fil.[%d]; " i i)
            myPrint "}\n\n"
 
        let SwitchFuncs (n : int) =
            let swFunc (swL:int) (swH:int) =
                sprintf "%sfun x ->\n%sif (x.sw%d > x.sw%d) then\n%s({x with sw%d = x.sw%d; sw%d = x.sw%d}, true)\n%selse (x, false)\n" (tabN 3) (tabN 4) swL swH (tabN 5) swL swH swH swL (tabN 5)

            myPrint "%slet SwitchFuncs = [|\n" (tabN 2)
            seq { for i=1 to (n-1) do
                    for j=0 to (i-1) do
                        yield (j, i)} |> Seq.iter (fun i -> myPrint "%s" (swFunc (fst i) (snd i)))
            myPrint "%s|]\n\n" (tabN 2)

        let SwitchFuncForSwitch (n : int) =
            myPrint "%slet SwitchFuncForSwitch (sw:Switch) =\n" (tabN 2)
            myPrint "%sSwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]\n\n" (tabN 3)

        let GetSwitchCountForSorter (n : int) =
            myPrint "%slet GetSwitchCountForSorter (sorter:Sorter<%s>) (sortables:seq<%s>) =\n" (tabN 2) (SgType n) (SgType n)
            myPrint "%slet checker t = IsSorted t\n" (tabN 3)
            myPrint "%sSorter.SortManyGetSwitchCountAndCheckResults<%s> checker sorter sortables\n\n" (tabN 3) (SgType n)

        let GetSwitchResultsForSorter (n : int) =
            myPrint "%slet GetSwitchResultsForSorter (sorter:Sorter<%s>) (sortables:seq<%s>) =\n" (tabN 2) (SgType n) (SgType n)
            myPrint "%slet checker t = IsSorted t\n" (tabN 3)
            myPrint "%sSorter.SortManyAndGetSwitchResults<%s> checker sorter sortables\n\n\n" (tabN 3) (SgType n)


        let GenN (n:int) =
            SgDef n
            myPrint "%smodule %s =\n\n" tab (SgType n)
            Order n
            Identity n
            FromArray n
            ToArray n
            AllBinaryTestCases n
            IsSorted n
            CreateRandom n
            SwitchFuncs n
            SwitchFuncForSwitch n
            GetSwitchCountForSorter n
            GetSwitchResultsForSorter n
            let outStr = sb.ToString()
            sb.Clear() |> ignore
            outStr
