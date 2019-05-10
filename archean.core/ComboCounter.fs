namespace archean.core


module ComboCounter = 

    type Wheel = {index:int; spokes:array<int[]>; offsets:int[]}

    let GetTotalSequenceCount(wheelset:Wheel[]) = 
        wheelset |> Array.map(fun w->w.spokes.Length)
                 |> Array.fold (fun l s -> s * l) 1


    let GetTotalArrayLength(wheelset:Wheel[]) = 
        wheelset |> Array.map(fun w->w.spokes.[0].Length)
                 |> Array.fold (fun l s -> s + l) 0


    let MakeMapper (prams:int[]) =  
        let mapper (items:int[]) (target:int[]) =
            { 0 .. (prams.Length - 1)}
            |> Seq.iter (fun i -> (target.[prams.[i]] <- items.[i]))
        mapper
    

    let MakeUniformWheelSet (spokes:int[][]) (setSize:int) =
        let partSize = spokes.[0].Length
        let wheelSet = 
            seq { 0 .. (setSize - 1)}
            |> Seq.map 
                (fun si -> 
                    { Wheel.index=0; 
                        spokes=spokes; 
                        offsets=Array.init 
                                    partSize 
                                    (fun i-> i + si*partSize) 
                    }
                )
            |> Seq.toArray
        wheelSet


    let Increment (wheelset:Wheel[]) =
        let mutable carry = 1
        wheelset 
        |> Array.map(
            fun w-> 
                if carry = 1 then
                    if (w.index = w.spokes.Length - 1) then
                        {index=0; spokes=w.spokes; offsets=w.offsets}
                    else
                        carry <- 0
                        {index=w.index+1; spokes=w.spokes; offsets=w.offsets}
                else w
            )


    let _MakeZeroOneArray (arrayLength:int) (wheelset:Wheel[]) =
        let aRet = Array.zeroCreate arrayLength
        let procWheel (wheel:Wheel) =
            let curSpoke = wheel.spokes.[wheel.index]
            curSpoke
            |> Array.iteri(fun dex sp-> 
                aRet.[wheel.offsets.[dex]] <- sp )

        wheelset |> Array.iter(fun wh -> procWheel wh)
        aRet


    let MakeZeroOneArray (wheelset:Wheel[]) =
        _MakeZeroOneArray (GetTotalArrayLength wheelset)


    let MakeAllArrays (wheelsetIn:Wheel[]) =
        let makeArray = 
            (MakeZeroOneArray wheelsetIn)
        let mutable wheelset = wheelsetIn
        seq {   
                while true do
                    yield (makeArray wheelset)
                    wheelset <- Increment wheelset
             }
        |> Seq.take (GetTotalSequenceCount wheelsetIn)


    let Filtered2Stage4BlockArrays (order:int) =
        let blockCount = order / 4
        let spokes = [|[|0;1;0;1|];[|0;0;0;0|];
                       [|0;0;0;1|];[|0;0;1;1|];
                       [|0;1;1;1|];[|1;1;1;1|];|]
        let wheelset = MakeUniformWheelSet spokes blockCount

        MakeAllArrays wheelset


    let Filtered3Stage8BlockArrays (order:int) =
        let blockCount = order / 8
        let spokes = 
            [|
                [|0;0;0;0;0;0;0;0|];[|0;0;0;0;0;0;0;1|];[|0;0;0;0;0;0;1;1|];[|0;0;0;0;0;1;0;1|];
                [|0;0;0;0;0;1;1;1|];[|0;0;0;0;1;1;1;1|];[|0;0;0;1;0;0;0;1|];[|0;0;0;1;0;0;1;1|];
                [|0;0;0;1;0;1;0;1|];[|0;0;0;1;0;1;1;1|];[|0;0;0;1;1;1;1;1|];[|0;0;1;1;0;0;1;1|];
                [|0;0;1;1;0;1;1;1|];[|0;0;1;1;1;1;1;1|];[|0;1;0;1;0;1;0;1|];[|0;1;0;1;0;1;1;1|];
                [|0;1;0;1;1;1;1;1|];[|0;1;1;1;0;1;1;1|];[|0;1;1;1;1;1;1;1|];[|1;1;1;1;1;1;1;1|]
            |]

        let wheelset = MakeUniformWheelSet spokes blockCount

        MakeAllArrays wheelset