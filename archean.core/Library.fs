namespace archean.core

open System

module Say =
    let hello name =
        printfn "Hello %s" name


module Combinatorics =

    let FisherYatesShuffle (rnd : Random) (initialList : array<'a>) =
        let availableFlags = Array.init initialList.Length (fun i -> (i, true))
                                                                        // Which items are available and their indices
        let nextItem nLeft =
            let nItem = rnd.Next(0, nLeft)                              // Index out of available items
            let index =                                                 // Index in original deck
                availableFlags                                          // Go through available array
                |> Seq.filter (fun (ndx,f) -> f)                        // and pick out only the available tuples
                |> Seq.nth nItem                                        // Get the one at our chosen index
                |> fst                                                  // and retrieve it's index into the original array
            availableFlags.[index] <- (index, false)                    // Mark that index as unavailable
            initialList.[index]                                         // and return the original item
        seq {(initialList.Length) .. -1 .. 1}                           // Going from the length of the list down to 1
        |> Seq.map (fun i -> nextItem i)                                // yield the next item


    let FisherYatesShuffleFromSeed (seed : int) =                                                  
        FisherYatesShuffle (new Random(seed))


    let RandomIntPermutations (rnd : Random) (len: int) (count:int) =
        let initialList = [|0 .. len-1|]                                
        let permuter = (FisherYatesShuffle rnd)
        Seq.init count (fun n -> (permuter initialList) |> Seq.toArray)
  

    let CompareArrays (a: array<int>) (b: array<int>) =
        if (a.Length <> b.Length) then false
        else seq {for i = 0 to a.Length - 1 do
                    if (a.[i] <> b.[i]) then
                        yield false}
             |> Seq.forall id

  
    let InverseMapArray (a: array<int>) =
        let aInv = Array.init a.Length (fun i -> 0)
        for i = 0 to a.Length - 1 do
            aInv.[a.[i]] <- i
        aInv

      
    let ComposeMapIntArrays (a: array<int>) (b: array<int>) =
        let product = Array.init a.Length (fun i -> 0)
        for i = 0 to a.Length - 1 do
            product.[a.[b.[i]]] <- i
        product
  
    // conj * a * conj ^ -1
    let ConjugateIntArrays (a: array<int>) (conj: array<int>) =
        conj |> ComposeMapIntArrays a |> ComposeMapIntArrays (InverseMapArray conj)


    let DistanceSquared (a: array<int>) (b: array<int>) =
        Array.fold2 (fun acc elem1 elem2 ->
        acc + (elem1 - elem2) * (elem1 - elem2)) 0 a b


    let UnsortednessSquared (a: array<int>) =
        let ref = [|0 .. (a.Length - 1)|]
        DistanceSquared a ref


    let SortIntArray (sortable: array<int>) (stage: array<int>) (counter: array<int>) =
        for i = 0 to stage.Length - 1 do
            let j = stage.[i]
            if (j > i ) then
                let stbA = sortable.[i]
                let stbB = sortable.[j]
                if(stbB < stbA) then
                    sortable.[i] <- stbB
                    sortable.[j] <- stbA
                    counter.[i] <- counter.[i] + 1

                    
    let SortCopyOfIntArray (sortable: array<int>) (stage: array<int>) (counter: array<int>) =
        let stbC = Array.copy sortable
        SortIntArray stbC stage counter
        stbC


    let MakeTwoCycleIntArray (arraysize: int) (lowBit: int) (hiBit: int) =
        Array.init arraysize (fun i -> if   (i = lowBit) then hiBit
                                       elif (i = hiBit) then lowBit
                                       else i)


    let MakeAllTwoCycleIntArrays (arraysize: int) =
        seq {for i = 0 to arraysize - 1 do
                for j = 0 to i - 1 do
                    yield MakeTwoCycleIntArray arraysize i j}


    let MakeRandomFullTwoCycleIntArray (rnd : Random) (arraysize: int) =
        let initialList = [|0 .. arraysize-1|]
        let arrayRet = Array.init arraysize (fun i -> i)
        let rndTupes = (FisherYatesShuffle rnd initialList) |> (Seq.chunkBySize 2) |> Seq.toArray
        for i = 0 to (arraysize / 2) - 1 do
            arrayRet.[rndTupes.[i].[0]] <- rndTupes.[i].[1]
            arrayRet.[rndTupes.[i].[1]] <- rndTupes.[i].[0]
        arrayRet
        
 
    let MakeRandomFullTwoCycleIntArrays (rnd : Random) (arraysize: int) (count: int) =
        seq {1 .. count} |> Seq.map (fun i -> MakeRandomFullTwoCycleIntArray rnd arraysize)



 
module Domain =

    type Permutation = private Permutation of int[]

    module Permutation =

        let Identity (len: int) = Permutation [|0 .. len-1|]
        let apply f (Permutation p) = f p
        let value p = apply id p

        let CreateRandom (rnd : Random) (len: int) (count: int) =
            let initialList = (Identity len) |> value                             
            let permuter = (Combinatorics.FisherYatesShuffle rnd)
            Seq.init count (fun _ -> Permutation ((permuter initialList) |> Seq.toArray))

        let Inverse (p: Permutation) =
            Permutation (Combinatorics.InverseMapArray (p |> value))

        let Product (pA: Permutation) (pB: Permutation) =
            Permutation (Combinatorics.ComposeMapIntArrays (pA |> value) (pB |> value))
    
    
    type TwoCycleIntArray = private TwoCycleIntArray of int[]

    module TwoCycleIntArray =

        let Identity (len: int) = TwoCycleIntArray [|0 .. len-1|]
        let apply f (TwoCycleIntArray p) = f p
        let value p = apply id p

        let MakeMonoCycleFromHiLow (len: int) (hi: int) (low: int) =
            TwoCycleIntArray (Combinatorics.MakeTwoCycleIntArray len low hi)

        let MakeAllMonoCycleOfLength (len: int) =
            (Combinatorics.MakeAllTwoCycleIntArrays len) 
            |> Seq.map (fun s -> TwoCycleIntArray s)

        let MakeRandomPolyCycle (rnd : Random) (len: int) (count: int) =
            Seq.init count (fun _ -> Combinatorics.MakeRandomFullTwoCycleIntArray rnd len)
            |> Seq.map (fun s -> TwoCycleIntArray s)



    type Switch = {low:int; hi:int}
    
    type Stage = {switches:Switch list}

    type Sorter = {order:int; switches: array<Switch>}

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


    module Sorter =
        let CreateRandom (rnd : Random) (switchSet:SwitchSet) (len: int) =
                {
                    Sorter.order=switchSet.order;
                    switches = seq { for i = 0 to len - 1 do
                                       yield switchSet.switches.[rnd.Next(0, switchSet.order)] }
                    |> Seq.toArray
                }
