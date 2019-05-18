
namespace archean.core

open System.Windows.Media

type ColorLeg<'a> = 
    { minC:Color; maxC:Color; spanC:Color[]; mapper:'a->int; minV:'a; maxV:'a;
        tics: 'a[] }

type BrushLeg<'a> = 
    { minB:Brush; maxB:Brush; spanB:SolidColorBrush[]; mapper:'a->int; minV:'a; maxV:'a;
        tics: 'a[] }

module ColorSets =
    
    let WcColors = 
        [|
            Colors.DarkSlateGray;  Colors.SteelBlue; Colors.DodgerBlue; Colors.MediumBlue;
            Colors.DarkBlue; Colors.MediumSeaGreen; Colors.Green;  Colors.DarkGreen;
            Colors.YellowGreen;  Colors.Yellow; Colors.Orange;  Colors.OrangeRed;
            Colors.Red;  Colors.DarkRed;
        |]


    let ColorFade (stepCount:int) (refCol:Color) =
        let bite ob = (byte (ob * 255 / stepCount))
        {0 .. stepCount-1} 
            |> Seq.map(fun st -> Color.FromArgb(bite st, refCol.R, refCol.G, refCol.B))


    let ByteInterp (bL:byte) (bR:byte) (stepMax:int) (step:int) =
        let iL = (int bL)
        let iR = (int bR)
        let incy = (step *  (iR - iL)) / stepMax
        (byte (iL + incy))


    let ColorSpan (stepCount:int) (colStart:Color) (colEnd:Color) =
        {1 .. stepCount } |> Seq.map(fun step -> 
            Color.FromArgb(
                ByteInterp colStart.A colEnd.A (stepCount + 1) step,
                ByteInterp colStart.R colEnd.R (stepCount + 1) step,
                ByteInterp colStart.G colEnd.G (stepCount + 1) step,
                ByteInterp colStart.B colEnd.B (stepCount + 1) step))


    let RedBlueSpan =
        ColorSpan 256 Colors.Red Colors.Blue |> Seq.toArray


    let TriColorStrip (interStep:int) (colorA:Color) (colorB:Color) (colorC:Color) =
        let btwStp = ColorSpan interStep
        seq { yield colorC } 
            |> Seq.append(btwStp colorB colorC)
            |> Seq.append(seq { yield colorB })
            |> Seq.append(btwStp colorA colorB)
            |> Seq.append(seq { yield colorA })

    
    let TriColorRing (interStep:int) (colorA:Color) (colorB:Color) (colorC:Color) =
        let btwStp = ColorSpan interStep
        (btwStp colorC colorA)
            |> Seq.append(seq { yield colorC })
            |> Seq.append(btwStp colorB colorC)
            |> Seq.append(seq { yield colorB })
            |> Seq.append(btwStp colorA colorB)
            |> Seq.append(seq { yield colorA })


    let QuadColorRing (interStep:int) (colorA:Color) (colorB:Color) 
                    (colorC:Color) (colorD:Color) =
        let btwStp = ColorSpan interStep
        (btwStp colorD colorA)
            |> Seq.append(seq { yield colorD })
            |> Seq.append(btwStp colorC colorD)
            |> Seq.append(seq { yield colorC })
            |> Seq.append(btwStp colorB colorC)
            |> Seq.append(seq { yield colorB })
            |> Seq.append(btwStp colorA colorB)
            |> Seq.append(seq { yield colorA })


    let ColStr (col:Color) = 
        sprintf "[%i, %i, %i]" col.R col.G col.B
        

    let RedBlueSFLeg =
        { minC=Colors.Black; maxC=Colors.Green; 
            spanC=RedBlueSpan; mapper=Partition.SF32to256; 
            minV= -1.0f; maxV=0.999f; tics=Partition.SF32Tics256 }


    // 0.25 < beta < 0.75
    let WcHistLeg beta max = 
        let tics = Partition.QuadInterp beta 14 
                    |> Seq.map(fun x -> x * max) 
                    |> Partition.Quantize  
                    |> Seq.toArray
        { minC=Colors.LightYellow; maxC=Colors.HotPink; 
            spanC=WcColors; mapper=(Partition.BinOfA tics); 
            minV=1; maxV=(int max); tics=tics }


    let WcHistLegInts (vals:int[]) =
        let prams = Partition.HalfValueInt vals
        WcHistLeg (fst prams) (float (snd prams))


    let GetLegColor<'a when 'a:comparison> (lm:ColorLeg<'a>) (value:'a) =
        if (value < lm.minV) then lm.minC
        else if (value >= lm.maxV) then lm.maxC
        else
        lm.spanC.[lm.mapper value]


    let GetLegMidVal<'a when 'a:comparison> (lm:ColorLeg<'a>) =
        lm.tics.[(int (lm.tics.GetLength(0) / 2))]

