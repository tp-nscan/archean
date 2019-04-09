namespace archean.core
open System
open archean.core.Combinatorics_Types
open archean.core.Sorting

module GeneratedSortables =

    type ISortable =
        abstract member IsSorted :  unit -> bool
       

    type Sortable6 = { sw0 : int; sw1 : int; sw2 : int; sw3 : int; sw4 : int; sw5 : int; }
    module Sortable6 =

        let Order = 6

        let Identity = {Sortable6.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3; sw4 = 4; sw5 = 5; }

        let FromArray (ia : int[]) =
            { Sortable6.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3]; sw4 = ia.[4]; sw5 = ia.[5]; }

        let ToArray (s : Sortable6) =
            [| s.sw0; s.sw1; s.sw2; s.sw3; s.sw4; s.sw5; |]

        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)

        let IsSorted (s : Sortable6) =
            if(s.sw0 > s.sw1) then
                false
            elif (s.sw1 > s.sw2) then
                false
            elif (s.sw2 > s.sw3) then
                false
            elif (s.sw3 > s.sw4) then
                false
            elif (s.sw4 > s.sw5) then
                false
            else
                true

        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 6 |> Seq.item 0 |> Permutation.value
            { Sortable6.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; sw4 = fil.[4]; sw5 = fil.[5]; }

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
        |]

        let SwitchFuncForSwitch (sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

        let GetSwitchCountForSorter (sorter:Sorter<Sortable6>) (sortables:seq<Sortable6>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<Sortable6> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<Sortable6>) (sortables:seq<Sortable6>) =
            let checker t = IsSorted t
            Sorter.SortManyAndGetSwitchResults<Sortable6> checker sorter sortables


    type Sortable7 = { sw0 : int; sw1 : int; sw2 : int; sw3 : int; sw4 : int; sw5 : int; sw6 : int; }
    module Sortable7 =

        let Order = 7

        let Identity = {Sortable7.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3; sw4 = 4; sw5 = 5; sw6 = 6; }

        let FromArray (ia : int[]) =
            { Sortable7.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3]; sw4 = ia.[4]; sw5 = ia.[5]; sw6 = ia.[6]; }

        let ToArray (s : Sortable7) =
            [| s.sw0; s.sw1; s.sw2; s.sw3; s.sw4; s.sw5; s.sw6; |]

        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)

        let IsSorted (s : Sortable7) =
            if(s.sw0 > s.sw1) then
                false
            elif (s.sw1 > s.sw2) then
                false
            elif (s.sw2 > s.sw3) then
                false
            elif (s.sw3 > s.sw4) then
                false
            elif (s.sw4 > s.sw5) then
                false
            elif (s.sw5 > s.sw6) then
                false
            else
                true

        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 7 |> Seq.item 0 |> Permutation.value
            { Sortable7.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; sw4 = fil.[4]; sw5 = fil.[5]; sw6 = fil.[6]; }

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
        |]

        let SwitchFuncForSwitch (sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

        let GetSwitchCountForSorter (sorter:Sorter<Sortable7>) (sortables:seq<Sortable7>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<Sortable7> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<Sortable7>) (sortables:seq<Sortable7>) =
            let checker t = IsSorted t
            Sorter.SortManyAndGetSwitchResults<Sortable7> checker sorter sortables



    type Sortable8 = { sw0 : int; sw1 : int; sw2 : int; sw3 : int; sw4 : int; sw5 : int; sw6 : int; sw7 : int; }
    module Sortable8 =

        let Order = 8

        let Identity = {Sortable8.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3; sw4 = 4; sw5 = 5; sw6 = 6; sw7 = 7; }

        let FromArray (ia : int[]) =
            { Sortable8.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3]; sw4 = ia.[4]; sw5 = ia.[5]; sw6 = ia.[6]; sw7 = ia.[7]; }

        let ToArray (s : Sortable8) =
            [| s.sw0; s.sw1; s.sw2; s.sw3; s.sw4; s.sw5; s.sw6; s.sw7; |]

        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)

        let IsSorted (s : Sortable8) =
            if(s.sw0 > s.sw1) then
                false
            elif (s.sw1 > s.sw2) then
                false
            elif (s.sw2 > s.sw3) then
                false
            elif (s.sw3 > s.sw4) then
                false
            elif (s.sw4 > s.sw5) then
                false
            elif (s.sw5 > s.sw6) then
                false
            elif (s.sw6 > s.sw7) then
                false
            else
                true

        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 8 |> Seq.item 0 |> Permutation.value
            { Sortable8.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; sw4 = fil.[4]; sw5 = fil.[5]; sw6 = fil.[6]; sw7 = fil.[7]; }

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
        |]

        let SwitchFuncForSwitch (sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

        let GetSwitchCountForSorter (sorter:Sorter<Sortable8>) (sortables:seq<Sortable8>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<Sortable8> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<Sortable8>) (sortables:seq<Sortable8>) =
            let checker t = IsSorted t
            Sorter.SortManyAndGetSwitchResults<Sortable8> checker sorter sortables


    type Sortable9 = { sw0 : int; sw1 : int; sw2 : int; sw3 : int; sw4 : int; sw5 : int; sw6 : int; sw7 : int; sw8 : int; }
    module Sortable9 =

        let Order = 9

        let Identity = {Sortable9.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3; sw4 = 4; sw5 = 5; sw6 = 6; sw7 = 7; sw8 = 8; }

        let FromArray (ia : int[]) =
            { Sortable9.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3]; sw4 = ia.[4]; sw5 = ia.[5]; sw6 = ia.[6]; sw7 = ia.[7]; sw8 = ia.[8]; }

        let ToArray (s : Sortable9) =
            [| s.sw0; s.sw1; s.sw2; s.sw3; s.sw4; s.sw5; s.sw6; s.sw7; s.sw8; |]

        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)

        let IsSorted (s : Sortable9) =
            if(s.sw0 > s.sw1) then
                false
            elif (s.sw1 > s.sw2) then
                false
            elif (s.sw2 > s.sw3) then
                false
            elif (s.sw3 > s.sw4) then
                false
            elif (s.sw4 > s.sw5) then
                false
            elif (s.sw5 > s.sw6) then
                false
            elif (s.sw6 > s.sw7) then
                false
            elif (s.sw7 > s.sw8) then
                false
            else
                true

        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 9 |> Seq.item 0 |> Permutation.value
            { Sortable9.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; sw4 = fil.[4]; sw5 = fil.[5]; sw6 = fil.[6]; sw7 = fil.[7]; sw8 = fil.[8]; }

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
        |]

        let SwitchFuncForSwitch (sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

        let GetSwitchCountForSorter (sorter:Sorter<Sortable9>) (sortables:seq<Sortable9>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<Sortable9> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<Sortable9>) (sortables:seq<Sortable9>) =
            let checker t = IsSorted t
            Sorter.SortManyAndGetSwitchResults<Sortable9> checker sorter sortables



    type Sortable10 = { sw0 : int; sw1 : int; sw2 : int; sw3 : int; sw4 : int; sw5 : int; sw6 : int; sw7 : int; sw8 : int; sw9 : int; }
    module Sortable10 =

        let Order = 10

        let Identity = {Sortable10.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3; sw4 = 4; sw5 = 5; sw6 = 6; sw7 = 7; sw8 = 8; sw9 = 9; }

        let FromArray (ia : int[]) =
            { Sortable10.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3]; sw4 = ia.[4]; sw5 = ia.[5]; sw6 = ia.[6]; sw7 = ia.[7]; sw8 = ia.[8]; sw9 = ia.[9]; }

        let ToArray (s : Sortable10) =
            [| s.sw0; s.sw1; s.sw2; s.sw3; s.sw4; s.sw5; s.sw6; s.sw7; s.sw8; s.sw9; |]

        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)

        let IsSorted (s : Sortable10) =
            if(s.sw0 > s.sw1) then
                false
            elif (s.sw1 > s.sw2) then
                false
            elif (s.sw2 > s.sw3) then
                false
            elif (s.sw3 > s.sw4) then
                false
            elif (s.sw4 > s.sw5) then
                false
            elif (s.sw5 > s.sw6) then
                false
            elif (s.sw6 > s.sw7) then
                false
            elif (s.sw7 > s.sw8) then
                false
            elif (s.sw8 > s.sw9) then
                false
            else
                true

        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 10 |> Seq.item 0 |> Permutation.value
            { Sortable10.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; sw4 = fil.[4]; sw5 = fil.[5]; sw6 = fil.[6]; sw7 = fil.[7]; sw8 = fil.[8]; sw9 = fil.[9]; }

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
        |]

        let SwitchFuncForSwitch (sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

        let GetSwitchCountForSorter (sorter:Sorter<Sortable10>) (sortables:seq<Sortable10>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<Sortable10> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<Sortable10>) (sortables:seq<Sortable10>) =
            let checker t = IsSorted t
            Sorter.SortManyAndGetSwitchResults<Sortable10> checker sorter sortables



    type Sortable11 = { sw0 : int; sw1 : int; sw2 : int; sw3 : int; sw4 : int; sw5 : int; sw6 : int; sw7 : int; sw8 : int; sw9 : int; sw10 : int; }
    module Sortable11 =

        let Order = 11

        let Identity = {Sortable11.sw0 = 0; sw1 = 1; sw2 = 2; sw3 = 3; sw4 = 4; sw5 = 5; sw6 = 6; sw7 = 7; sw8 = 8; sw9 = 9; sw10 = 10; }

        let FromArray (ia : int[]) =
            { Sortable11.sw0 = ia.[0]; sw1 = ia.[1]; sw2 = ia.[2]; sw3 = ia.[3]; sw4 = ia.[4]; sw5 = ia.[5]; sw6 = ia.[6]; sw7 = ia.[7]; sw8 = ia.[8]; sw9 = ia.[9]; sw10 = ia.[10]; }

        let ToArray (s : Sortable11) =
            [| s.sw0; s.sw1; s.sw2; s.sw3; s.sw4; s.sw5; s.sw6; s.sw7; s.sw8; s.sw9; s.sw10; |]

        let AllBinaryTestCases =
            {0 .. (1 <<< Order) - 1}
            |> Seq.map (fun i -> Combinatorics.ToIntArray Order i)
            |> Seq.map (fun i -> FromArray i)

        let IsSorted (s : Sortable11) =
            if(s.sw0 > s.sw1) then
                false
            elif (s.sw1 > s.sw2) then
                false
            elif (s.sw2 > s.sw3) then
                false
            elif (s.sw3 > s.sw4) then
                false
            elif (s.sw4 > s.sw5) then
                false
            elif (s.sw5 > s.sw6) then
                false
            elif (s.sw6 > s.sw7) then
                false
            elif (s.sw7 > s.sw8) then
                false
            elif (s.sw8 > s.sw9) then
                false
            elif (s.sw9 > s.sw10) then
                false
            else
                true

        let CreateRandom (rnd : Random) =
            let fil = Permutation.CreateRandom rnd 11|> Seq.item 0 |> Permutation.value
            { Sortable11.sw0 = fil.[0]; sw1 = fil.[1]; sw2 = fil.[2]; sw3 = fil.[3]; sw4 = fil.[4]; sw5 = fil.[5]; sw6 = fil.[6]; sw7 = fil.[7]; sw8 = fil.[8]; sw9 = fil.[9]; sw10 = fil.[10]; }

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
        |]

        let SwitchFuncForSwitch (sw:Switch) =
            SwitchFuncs.[sw.low + (sw.hi * (sw.hi - 1)) / 2]

        let GetSwitchCountForSorter (sorter:Sorter<Sortable11>) (sortables:seq<Sortable11>) =
            let checker t = IsSorted t
            Sorter.SortManyGetSwitchCountAndCheckResults<Sortable11> checker sorter sortables

        let GetSwitchResultsForSorter (sorter:Sorter<Sortable11>) (sortables:seq<Sortable11>) =
            let checker t = IsSorted t
            Sorter.SortManyAndGetSwitchResults<Sortable11> checker sorter sortables

