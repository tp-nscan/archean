namespace archean.core

open System

module Partition =

    let BinOfA<'a when 'a:comparison> (bins:'a[]) (value:'a) = 
        let rec binf (a:'a[]) (dex:int) =
            if dex < 0 then -1
            else if a.[dex] <= value then dex
            else binf a (dex-1)
        binf bins (bins.Length - 1)


    let BinOfL<'a when 'a:comparison> (bins:'a list) (value:'a) = 
        let rec binf (a:'a list) (dex:int) =
            match a with
            | [] -> dex
            | a::b when a <= value -> binf b (dex + 1)
            | _ -> dex - 1
        binf bins 0


    let UF32Tics256 =
        {0.0f .. 255.0f} |> Seq.map(fun x-> x / 256.0f) |> Seq.toArray


    let SF32Tics256 =
        {0.0f .. 255.0f} |> Seq.map(fun x-> x / 128.0f - 0.999f) |> Seq.toArray


    let UF32to256 (value:float32) =
        (int (value * 255.99f))


    let SF32to256 (value:float32) =
        (int (value * 127.49f + 127.48f))
        

    let Quantize (cs:seq<float>) =
        let qv sv rv =
            let iv = (int rv)
            if (sv + 1) > iv then sv + 1
            else iv
        cs |> Seq.mapi(qv)


    let Quantize32 (cs:seq<float32>) =
        let qv sv rv =
            let iv = (int rv)
            if (sv + 1) > iv then sv + 1
            else iv
        cs |> Seq.mapi(qv)


    // produces a sequence within [0.0,1.0] for 0.25 < halfVal < 0.75
    let QuadInterp halfVal count =
        let stepSize = 1.0 / (float count)
        seq {0.0 .. stepSize .. 1.0 }
        |> Seq.map(fun x -> x * x * (2.0 - 4.0 * halfVal) + x * (4.0 * halfVal - 1.0))


    // returns the bin index for the value, for QuadInterp bins distributed over [0.0,1.0]
    // the returned indexes range from 0 .. (count -1)
    let InvQuadInterp halfVal (count:int) value =
        let IntStep v =
            (int (Math.Floor((float count) * v )))

        if(Math.Abs(halfVal - 0.5) < 0.005) then
            IntStep(value / (2.0 * halfVal))
            else
            IntStep((
                        (1.0 - 4.0 * halfVal) 
                        +
                        Math.Sqrt(8.0 * halfVal * (2.0 * halfVal - 1.0 ) + 1.0 +  8.0 * value * (1.0 - 2.0 * halfVal))
                    )
                    / 
                    (4.0 - 8.0 * halfVal)
                  )

    
    let HalfValueInt (vals:int[]) =
        let svs = vals |> Array.sort
        let max = svs.[svs.Length - 1]
        let frac = (float svs.[(int (svs.Length / 2))]) / (float max)
        if frac > 0.75 then (0.75, max)
        else if frac < 0.25 then (0.25, max)
        else (frac, max)

            
    let HalfValueF32 (vals:float32[]) =
        let svs = vals |> Array.sort
        let max = svs.[svs.Length - 1]
        let frac = (float svs.[(int (svs.Length / 2))]) / (float max)
        if frac > 0.75 then (0.75, max)
        else if frac < 0.25 then (0.25, max)
        else (frac, max)


