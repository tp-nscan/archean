namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections

module Sorting =

    // the number of bus lanes in a Sorter
    module Order = 
        type T = Order of int
        let create i = 
            if (i >= 0 )
            then Some (Order i)
            else None

    type SwitchCount = {v:int}

    type ISwitch =
        abstract member low: int
        abstract member hi: int

    type Switch(low, hi) =
        interface ISwitch with 
            member this.low = low
            member this.hi = hi

        member this.low = low
        member this.hi = hi


    module Switch =
        let SwitchSeqFromIntArray (pArray:int[]) =
             seq { for i = 0 to pArray.Length - 1 do
                    let j = pArray.[i]
                    if ((j > i ) && (i = pArray.[j]) ) then
                        yield new Switch(i,j) :> ISwitch }

        let SwitchSeqFromPermutation (p:Permutation) =
            SwitchSeqFromIntArray (Permutation.value p)
         
        let SwitchSeqFromPolyCycle (p:TwoCycleIntArray) =
            SwitchSeqFromIntArray (TwoCycleIntArray.value p)
        
        let ToString (sw:ISwitch) =
            sprintf "(%d, %d)" sw.low sw.hi

        let RandomSwitchesOfOrder (order:int) (rnd:Random) =
            seq { while true do 
                     let hi = rnd.Next(order)
                     yield new Switch(rnd.Next(hi), hi) :> ISwitch }


    type Stage = {switches:ISwitch list}
    module Stage =
        let MergeSwitchesIntoStages (order:int) (switches:seq<ISwitch>) =
            let mutable stageTracker = Array.init order (fun i -> false)
            let switchesForStage = new ResizeArray<ISwitch>()
            seq { 
                    for sw in switches do
                        if (stageTracker.[sw.hi] || stageTracker.[sw.low] ) then
                            yield { Stage.switches = switchesForStage |> Seq.toList }
                            stageTracker <- Array.init order (fun i -> false)
                            switchesForStage.Clear()
                        stageTracker.[sw.hi] <- true
                        stageTracker.[sw.low] <- true
                        switchesForStage.Add sw
                    yield { Stage.switches=switchesForStage |> Seq.toList }
                 }

        let GetStageIndexesFromSwitches (order:int) (switches:seq<ISwitch>) =
            let mutable stageTracker = Array.init order (fun i -> false)
            let mutable curDex = 0
            seq { 
                    yield curDex
                    for sw in switches do
                        if (stageTracker.[sw.hi] || stageTracker.[sw.low] ) then
                            yield curDex
                            stageTracker <- Array.init order (fun i -> false)
                        stageTracker.[sw.hi] <- true
                        stageTracker.[sw.low] <- true
                        curDex <- curDex + 1
                    yield curDex
                 }

        let MakeStagePackedSwitchSeq (rnd:Random) (order: int) =
            let aa (rnd:Random)  = 
                (TwoCycleIntArray.MakeRandomPolyCycle rnd order)
                        |> Switch.SwitchSeqFromPolyCycle
            seq { while true do yield! (aa rnd) }

        
    type SortableIntArray = {values:int[]}
    module SortableIntArray =
    
        let Identity (order: int) = { SortableIntArray.values = [|0 .. order-1|] }
        let apply f (p:SortableIntArray) = f p.values
        let value p = apply id p

        let SwitchFuncForSwitch (sw:ISwitch) =
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
             
        let SortableSeq (sortables:int[][]) =
            sortables
                |> Array.map(fun a -> Array.copy a)
                |> Array.toSeq


        let WeightedSortableSeq (sortables:int[][]) =
            sortables
                |> Seq.map(fun a -> (Array.copy a, 1))


        let SortableSeqAllBinary (order:int) =
                IntBits.AllBinaryTestCases order
    


    type SorterDef = {order:int; switches: array<ISwitch>}
    module SorterDef =

        let CreateRandom (order:int) (len:int) (rnd:Random) =
            {
                SorterDef.order=order;
                switches = Switch.RandomSwitchesOfOrder order rnd
                                |> Seq.take len
                                |> Seq.toArray
            }

        let CreateRandomPackedStages (order:int) (len:int) (rnd:Random) =
            {
                SorterDef.order=order;
                switches = (Stage.MakeStagePackedSwitchSeq rnd order)
                                |> Seq.take len
                                |> Seq.toArray
            }

        let AppendSwitches (switches:seq<ISwitch>) (sorterDef:SorterDef) =
            {
                SorterDef.order= sorterDef.order;
                switches = (switches |> Seq.toArray) |> Array.append sorterDef.switches
            }


    type StagedSorterDef = { sorterDef:SorterDef; stageIndexes:array<int>;}
    module StagedSorterDef =
        
        let StageArrayToSwitchArray (sta:seq<Stage>) =
            seq { for stg in sta do yield! (stg.switches |> List.toSeq)}
            |> Seq.toArray

        let ToStagedSorterDef (sd:SorterDef) =
            let sia = sd.switches |> (Stage.GetStageIndexesFromSwitches sd.order) 
                                  |> Seq.toArray;
            {
                 StagedSorterDef.sorterDef = sd; 
                 stageIndexes=sia;
            }


        let GetSwitchIndexesForStage (ssd:StagedSorterDef) (statgeNum:int) =
            { ssd.stageIndexes.[statgeNum] .. 
               (ssd.stageIndexes.[statgeNum + 1] - 1) }
        

        let GetStages (ssd:StagedSorterDef) = 
            { 0 .. ssd.stageIndexes.Length - 2}
            |> Seq.map(fun i -> GetSwitchIndexesForStage ssd i
                                |> Seq.map(fun i-> ssd.sorterDef.switches.[i]))


        // Retains the stage partitions of the prefix
        let AppendSwitches (switches:seq<ISwitch>) (stagedSorterDef:StagedSorterDef) =
            let swCapture = switches |> Seq.toArray
            let appendArray = 
                swCapture 
                |> (Stage.GetStageIndexesFromSwitches 
                        stagedSorterDef.sorterDef.order)
                |> Seq.skip 1
                |> Seq.map (fun i -> i + stagedSorterDef.sorterDef.switches.Length)
                |> Seq.toArray;
            {
                sorterDef = stagedSorterDef.sorterDef 
                            |> (SorterDef.AppendSwitches swCapture);
                stageIndexes =  appendArray
                                |> Array.append stagedSorterDef.stageIndexes
            }


    type SwitchTracker = {weights:int[]}
    module SwitchTracker =
        let Make (length: int) =
            {weights=Array.init length (fun i -> 0)}

        let MakePrefixed (totalSwitches: int) (prefixSwitches: int) =
            {weights=Array.init totalSwitches (fun i -> if (i<prefixSwitches) then 1 else 0)}

        let ToStageArrays (switchTracker:SwitchTracker) 
                          (stagedSorterDef:StagedSorterDef) =

            Combinatorics.BreakIntoSegments 
                switchTracker.weights 
                stagedSorterDef.stageIndexes


        let ToStageReportString 
                        (switchTracker:SwitchTracker) 
                        (stagedSorterDef:StagedSorterDef) =

            let ArrayToString(array:'a[]) =
                sprintf "(%s)" (array |> Seq.map(fun a -> string a) |> String.concat ", ")

            let NestedArrayToString (af: 'a[]->string) (nestedArray:'a[][]) =
                sprintf "(%s)" (nestedArray |> Seq.map(fun a -> ArrayToString a) |> String.concat ", ")

            NestedArrayToString ArrayToString (ToStageArrays switchTracker stagedSorterDef)


    type SwitchUsage = {switch:ISwitch; switchIndex:int; useCount:int}
    module SwitchUsage =

        let CollectTheUsedSwitches 
                    (sorterDef:SorterDef) 
                    (switchTracker:SwitchTracker) = 
            seq { for i = 0 to switchTracker.weights.Length - 1 do
                    if (switchTracker.weights.[i] > 0) then
                        yield {switch=sorterDef.switches.[i]; switchIndex=i; 
                               useCount=switchTracker.weights.[i] } }
            |> Seq.toArray

        let ToString (sw:SwitchUsage) =
            sprintf "{%s, %d, %d}" (sw.switch |> Switch.ToString)
                                   sw.switchIndex sw.useCount


    type StageResult = {switchUsages:SwitchUsage list}
    module StageResult =
        let ToString (stageResult:StageResult) =
            sprintf "[%s]" (stageResult.switchUsages 
                            |> List.map(fun swu -> SwitchUsage.ToString swu)
                            |> String.concat ", ")

        let SwitchReport (stageResult:StageResult) =
            sprintf "[%s]" (stageResult.switchUsages 
                            |> List.map(fun swu -> Switch.ToString swu.switch)
                            |> String.concat ", ")


    type SorterResult = {sorterDef:SorterDef; stageResults:StageResult list}
    module SorterResult =
        let SwitchAndStageCountsIn (stageResults:StageResult list) =
            (stageResults.Length, stageResults |> List.fold(fun s i -> s + i.switchUsages.Length) 0)

        let MergeSwitchUsagesIntoStageResults (order:int) (switchUsages:SwitchUsage[]) =
            let mutable stageTracker = Array.init order (fun i -> false)
            let switchResultsForStage = new ResizeArray<SwitchUsage>()

            let fred = 
                seq { for i = 0 to switchUsages.Length - 1 do
                        let curSwitch = switchUsages.[i].switch
                        if (stageTracker.[curSwitch.hi] || stageTracker.[curSwitch.low] ) then
                            yield { StageResult.switchUsages=switchResultsForStage |> Seq.toList }
                            stageTracker <- Array.init order (fun i -> false)
                            switchResultsForStage.Clear()

                        stageTracker.[curSwitch.hi] <- true
                        stageTracker.[curSwitch.low] <- true
                        switchResultsForStage.Add switchUsages.[i]
                        if (i = switchUsages.Length - 1) then
                            yield { StageResult.switchUsages=switchResultsForStage |> Seq.toList }
                    }
                    |> Seq.toList
            if fred.Length < 10 then
                fred
                else 
                fred

        let MakeSorterResult 
                (sorterDef:SorterDef) 
                (switchUsages:SwitchUsage[]) =

            let cucko = MergeSwitchUsagesIntoStageResults sorterDef.order switchUsages

            {sorterDef=sorterDef; 
            stageResults=MergeSwitchUsagesIntoStageResults sorterDef.order switchUsages}

        let GetSwitchReport (sorterResult:SorterResult) =
            sorterResult.stageResults |> List.map(fun sr -> StageResult.SwitchReport sr)
                                      |> String.concat "\n"  
            



    type SwitchPad = {switches:ResizeArray<ISwitch>; keyUsage:int[] }
    module StageLayout =

        let InitSwitchPad (order:int) =
            {switches= new ResizeArray<ISwitch>(); keyUsage = Array.init order (fun i -> 1) }


        let SwitchFits (switchPad:SwitchPad) (switch:ISwitch) =
            seq {switch.low .. switch.hi}
                |> Seq.forall(fun i -> switchPad.keyUsage.[i] = 1)


        let SwitchPrint (switch:ISwitch) (position:int) (current:int) =
            if  ((position >= switch.low) && (position <= switch.hi)) then
                0 else current
        

        let AddSwitch (switchPad:SwitchPad) (switch:ISwitch) =

            switchPad.switches.Add switch
            { 
                switches = switchPad.switches; 
                keyUsage = Array.init switchPad.keyUsage.Length
                            (fun i -> SwitchPrint switch i switchPad.keyUsage.[i])
            }


        let LayoutSwitchesLoose (order:int) (switches:seq<ISwitch>) = 
            let mutable switchPad = InitSwitchPad order
            seq {
                    for sw in switches do
                        if (SwitchFits switchPad sw) then
                            switchPad <- AddSwitch switchPad sw
                        else
                            yield switchPad.switches |> Seq.toArray
                            switchPad <- InitSwitchPad order
                            switchPad <- AddSwitch switchPad sw

                    yield switchPad.switches |> Seq.toArray
                }
                |> Seq.toArray


        let AddSwitchTight (order:int) (switchPads: SwitchPad list) (switch:ISwitch) =
            let revList = switchPads |> List.rev

            let rec spM (prefix: SwitchPad list) suffix =
                match suffix with
                | head :: tail -> 
                    if (SwitchFits head switch) then
                        tail |> List.append [(AddSwitch head switch)]
                               |> List.append prefix
                               |> List.rev
                    else
                        spM ([head] |> List.append prefix) tail
                | [] -> let switchPad = InitSwitchPad order
                        let mody = (AddSwitch switchPad switch) 
                        [mody] |> (List.append prefix)
                        |> List.rev

            spM [] revList


        let LayoutSwitchesTight (order:int) (switches:seq<ISwitch>) = 
            let mutable switchPads = [InitSwitchPad order]
            seq {
                    for sw in switches do
                        switchPads <- AddSwitchTight order switchPads sw
                }
                |> Seq.toArray |> ignore
            switchPads |> List.map(fun swp -> swp.switches |> Seq.toArray)
                       |> List.rev
                       |> List.toArray
                       

        let LayoutRandomStage (order:int) (rnd:Random) =
            let switches = Stage.MakeStagePackedSwitchSeq rnd order |> Seq.take (order / 2)
            LayoutSwitchesLoose order switches


        let LayoutStagedSorter (ssd:StagedSorterDef) (switchLayout:seq<ISwitch>->ISwitch[][]) =
                StagedSorterDef.GetStages ssd
                     |> Seq.map(fun sq -> switchLayout sq )

                     
        let LayoutStagedSorterSingle (ssd:StagedSorterDef) =
                ssd.sorterDef.switches
                    |> Seq.map(fun sw -> [|[|sw|]|] )
          

        let LayoutStagedSorterLoose (ssd:StagedSorterDef) =
                LayoutStagedSorter ssd (LayoutSwitchesLoose ssd.sorterDef.order)


        let LayoutStagedSorterTight (ssd:StagedSorterDef) =
                LayoutStagedSorter ssd (LayoutSwitchesTight ssd.sorterDef.order)
        

        let LayoutSwitchSeqsTight (order:int) (ssqs:seq<seq<ISwitch>>) =
                ssqs
                  |> Seq.map(fun sq -> (LayoutSwitchesTight order) sq )  


        let LayoutSwitchSeqsLoose (order:int) (ssqs:seq<seq<ISwitch>>) =
                ssqs
                  |> Seq.map(fun sq -> (LayoutSwitchesLoose order) sq )  



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
