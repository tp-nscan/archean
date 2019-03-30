namespace archean.core
open System
open archean.core.Combinatorics_Types
open archean.core.Sorting

module Sortables =

    type ISortable =
        abstract member IsSorted :  unit -> bool
        

    type Sortable4 = {sw0:int; sw1:int; sw2:int; sw3:int;}

    module Sortable4 =
        
        let Order = 4

        let Identity = {Sortable4.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3;}
 
        let FromArray (ia : int[]) =
            {Sortable4.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3];}

        let ToArray (s4 : Sortable4) =
            [| s4.sw0; s4.sw1; s4.sw2; s4.sw3 |]
 
        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1} 
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)


        let IsSorted (s4: Sortable4) =
            if (s4.sw0 > s4.sw1) then
                false
            elif (s4.sw1 > s4.sw2) then
                false
            elif (s4.sw2 > s4.sw3) then
                false
            else
                true
 
 
        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 4 1 |> Seq.item 0 |> Permutation.value
            {Sortable4.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; }
     

        let SwitchFuncs = [|
            fun x ->
                if (x.sw0 > x.sw1) then
                    ({x with sw0 = x.sw1; sw1 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw2) then
                    ({x with sw0 = x.sw2; sw2 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw2) then
                    ({x with sw1 = x.sw2; sw2 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw3) then
                    ({x with sw0 = x.sw3; sw3 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw3) then
                    ({x with sw1 = x.sw3; sw3 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw3) then
                    ({x with sw2 = x.sw3; sw3 = x.sw2}, true)
                    else (x, false)
        |]

        let SwitchFuncForSwitch(sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

        let GetSwitchCountForSorter (sorter:Sorter<Sortable4>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<Sortable4> checker sorter AllBinaryTestCases

 
    type Sortable12 = { sw0 : int; sw1 : int; sw2 : int; sw3 : int; sw4 : int; sw5 : int; sw6 : int; sw7 : int; sw8 : int; sw9 : int; sw10 : int; sw11 : int; }

    module Sortable12 =

        let Order = 12

        let Identity = {Sortable12.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3; sw4 = 4; sw5 = 5; sw6 = 6; sw7 = 7; sw8 = 8; sw9 = 9; sw10 = 10; sw11 = 11; }

        let FromArray (ia : int[]) =
            { Sortable12.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3]; sw4 = ia.[4]; sw5 = ia.[5]; sw6 = ia.[6]; sw7 = ia.[7]; sw8 = ia.[8]; sw9 = ia.[9]; sw10 = ia.[10]; sw11 = ia.[11]; }

        let ToArray (s : Sortable12) =
            [| s.sw0; s.sw1; s.sw2; s.sw3; s.sw4; s.sw5; s.sw6; s.sw7; s.sw8; s.sw9; s.sw10; s.sw11; |]

        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)

        let IsSorted (s : Sortable12) =
            if(s.sw0 <> 0) then
                  false
            elif (s.sw1 <> 1) then
                false
            elif (s.sw2 <> 2) then
                false
            elif (s.sw3 <> 3) then
                false
            elif (s.sw4 <> 4) then
                false
            elif (s.sw5 <> 5) then
                false
            elif (s.sw6 <> 6) then
                false
            elif (s.sw7 <> 7) then
                false
            elif (s.sw8 <> 8) then
                false
            elif (s.sw9 <> 9) then
                false
            elif (s.sw10 <> 10) then
                false
            elif (s.sw11 <> 11) then
                false
            else
                true

        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 12 1 |> Seq.item 0 |> Permutation.value
            { Sortable12.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; sw4 = fil.[4]; sw5 = fil.[5]; sw6 = fil.[6]; sw7 = fil.[7]; sw8 = fil.[8]; sw9 = fil.[9]; sw10 = fil.[10]; sw11 = fil.[11]; }

        let SwitchFuncs = [|
            fun x ->
                if (x.sw0 > x.sw1) then
                    ({x with sw0 = x.sw1; sw1 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw2) then
                    ({x with sw0 = x.sw2; sw2 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw2) then
                    ({x with sw1 = x.sw2; sw2 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw3) then
                    ({x with sw0 = x.sw3; sw3 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw3) then
                    ({x with sw1 = x.sw3; sw3 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw3) then
                    ({x with sw2 = x.sw3; sw3 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw4) then
                    ({x with sw0 = x.sw4; sw4 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw4) then
                    ({x with sw1 = x.sw4; sw4 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw4) then
                    ({x with sw2 = x.sw4; sw4 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw4) then
                    ({x with sw3 = x.sw4; sw4 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw5) then
                    ({x with sw0 = x.sw5; sw5 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw5) then
                    ({x with sw1 = x.sw5; sw5 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw5) then
                    ({x with sw2 = x.sw5; sw5 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw5) then
                    ({x with sw3 = x.sw5; sw5 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw4 > x.sw5) then
                    ({x with sw4 = x.sw5; sw5 = x.sw4}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw6) then
                    ({x with sw0 = x.sw6; sw6 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw6) then
                    ({x with sw1 = x.sw6; sw6 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw6) then
                    ({x with sw2 = x.sw6; sw6 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw6) then
                    ({x with sw3 = x.sw6; sw6 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw4 > x.sw6) then
                    ({x with sw4 = x.sw6; sw6 = x.sw4}, true)
                    else (x, false)
            fun x ->
                if (x.sw5 > x.sw6) then
                    ({x with sw5 = x.sw6; sw6 = x.sw5}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw7) then
                    ({x with sw0 = x.sw7; sw7 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw7) then
                    ({x with sw1 = x.sw7; sw7 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw7) then
                    ({x with sw2 = x.sw7; sw7 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw7) then
                    ({x with sw3 = x.sw7; sw7 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw4 > x.sw7) then
                    ({x with sw4 = x.sw7; sw7 = x.sw4}, true)
                    else (x, false)
            fun x ->
                if (x.sw5 > x.sw7) then
                    ({x with sw5 = x.sw7; sw7 = x.sw5}, true)
                    else (x, false)
            fun x ->
                if (x.sw6 > x.sw7) then
                    ({x with sw6 = x.sw7; sw7 = x.sw6}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw8) then
                    ({x with sw0 = x.sw8; sw8 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw8) then
                    ({x with sw1 = x.sw8; sw8 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw8) then
                    ({x with sw2 = x.sw8; sw8 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw8) then
                    ({x with sw3 = x.sw8; sw8 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw4 > x.sw8) then
                    ({x with sw4 = x.sw8; sw8 = x.sw4}, true)
                    else (x, false)
            fun x ->
                if (x.sw5 > x.sw8) then
                    ({x with sw5 = x.sw8; sw8 = x.sw5}, true)
                    else (x, false)
            fun x ->
                if (x.sw6 > x.sw8) then
                    ({x with sw6 = x.sw8; sw8 = x.sw6}, true)
                    else (x, false)
            fun x ->
                if (x.sw7 > x.sw8) then
                    ({x with sw7 = x.sw8; sw8 = x.sw7}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw9) then
                    ({x with sw0 = x.sw9; sw9 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw9) then
                    ({x with sw1 = x.sw9; sw9 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw9) then
                    ({x with sw2 = x.sw9; sw9 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw9) then
                    ({x with sw3 = x.sw9; sw9 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw4 > x.sw9) then
                    ({x with sw4 = x.sw9; sw9 = x.sw4}, true)
                    else (x, false)
            fun x ->
                if (x.sw5 > x.sw9) then
                    ({x with sw5 = x.sw9; sw9 = x.sw5}, true)
                    else (x, false)
            fun x ->
                if (x.sw6 > x.sw9) then
                    ({x with sw6 = x.sw9; sw9 = x.sw6}, true)
                    else (x, false)
            fun x ->
                if (x.sw7 > x.sw9) then
                    ({x with sw7 = x.sw9; sw9 = x.sw7}, true)
                    else (x, false)
            fun x ->
                if (x.sw8 > x.sw9) then
                    ({x with sw8 = x.sw9; sw9 = x.sw8}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw10) then
                    ({x with sw0 = x.sw10; sw10 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw10) then
                    ({x with sw1 = x.sw10; sw10 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw10) then
                    ({x with sw2 = x.sw10; sw10 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw10) then
                    ({x with sw3 = x.sw10; sw10 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw4 > x.sw10) then
                    ({x with sw4 = x.sw10; sw10 = x.sw4}, true)
                    else (x, false)
            fun x ->
                if (x.sw5 > x.sw10) then
                    ({x with sw5 = x.sw10; sw10 = x.sw5}, true)
                    else (x, false)
            fun x ->
                if (x.sw6 > x.sw10) then
                    ({x with sw6 = x.sw10; sw10 = x.sw6}, true)
                    else (x, false)
            fun x ->
                if (x.sw7 > x.sw10) then
                    ({x with sw7 = x.sw10; sw10 = x.sw7}, true)
                    else (x, false)
            fun x ->
                if (x.sw8 > x.sw10) then
                    ({x with sw8 = x.sw10; sw10 = x.sw8}, true)
                    else (x, false)
            fun x ->
                if (x.sw9 > x.sw10) then
                    ({x with sw9 = x.sw10; sw10 = x.sw9}, true)
                    else (x, false)
            fun x ->
                if (x.sw0 > x.sw11) then
                    ({x with sw0 = x.sw11; sw11 = x.sw0}, true)
                    else (x, false)
            fun x ->
                if (x.sw1 > x.sw11) then
                    ({x with sw1 = x.sw11; sw11 = x.sw1}, true)
                    else (x, false)
            fun x ->
                if (x.sw2 > x.sw11) then
                    ({x with sw2 = x.sw11; sw11 = x.sw2}, true)
                    else (x, false)
            fun x ->
                if (x.sw3 > x.sw11) then
                    ({x with sw3 = x.sw11; sw11 = x.sw3}, true)
                    else (x, false)
            fun x ->
                if (x.sw4 > x.sw11) then
                    ({x with sw4 = x.sw11; sw11 = x.sw4}, true)
                    else (x, false)
            fun x ->
                if (x.sw5 > x.sw11) then
                    ({x with sw5 = x.sw11; sw11 = x.sw5}, true)
                    else (x, false)
            fun x ->
                if (x.sw6 > x.sw11) then
                    ({x with sw6 = x.sw11; sw11 = x.sw6}, true)
                    else (x, false)
            fun x ->
                if (x.sw7 > x.sw11) then
                    ({x with sw7 = x.sw11; sw11 = x.sw7}, true)
                    else (x, false)
            fun x ->
                if (x.sw8 > x.sw11) then
                    ({x with sw8 = x.sw11; sw11 = x.sw8}, true)
                    else (x, false)
            fun x ->
                if (x.sw9 > x.sw11) then
                    ({x with sw9 = x.sw11; sw11 = x.sw9}, true)
                    else (x, false)
            fun x ->
                if (x.sw10 > x.sw11) then
                    ({x with sw10 = x.sw11; sw11 = x.sw10}, true)
                    else (x, false)
        |]

        let SwitchFuncForSwitch (sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

