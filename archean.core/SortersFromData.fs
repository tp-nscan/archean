namespace archean.core
open System
open archean.core
open archean.core.Sorting


module SortersFromData =

    type GenPrefixMode =
         | Green2 
         | Green3
    
    let ParseSorterDataToArrayOfStages (stagesStr:string) =

        let MakeSwitch (s:string) =
            let pcs = s.Split([|',';|])
                        |> Seq.map(fun i -> i |> int)
                        |> Seq.toArray
            { Switch.low = pcs.[0]; Switch.hi = pcs.[1]}

        stagesStr.Split([|'[';  ']'; '\n'; '\r'; ' ';|], StringSplitOptions.RemoveEmptyEntries)
            |> Seq.map(fun l -> l.Split([|'(';  ')';|], StringSplitOptions.RemoveEmptyEntries)
                                |> Seq.filter(fun pc -> pc <> ",")
                                |> Seq.map(fun pcs -> MakeSwitch pcs)
                                |> Seq.toList
                        )
            |> Seq.map(fun sws -> {Stage.switches = sws} )
            |> Seq.toArray

                              
    let ParseSorterDataToStagedSorter =
        let stages = SorterData.Order4Str |> ParseSorterDataToArrayOfStages
        {StagedSorterDef.order=4; stages=stages}

    let GreenPrefix2 =
        let stages = SorterData.Order16_GreenStr2 |> ParseSorterDataToArrayOfStages
        {StagedSorterDef.order=16; stages=stages}

    let GreenPrefix3 =
        let stages = SorterData.Order16_GreenStr3 |> ParseSorterDataToArrayOfStages
        {StagedSorterDef.order=16; stages=stages}

    let GreenPrefix4 =
        let stages = SorterData.Order16_GreenStr4 |> ParseSorterDataToArrayOfStages
        {StagedSorterDef.order=16; stages=stages}

    let GreenPrefix5 =
        let stages = SorterData.Order16_GreenStr5 |> ParseSorterDataToArrayOfStages
        {StagedSorterDef.order=16; stages=stages}

    let GreenPrefix6 =
        let stages = SorterData.Order16_GreenStr6 |> ParseSorterDataToArrayOfStages
        {StagedSorterDef.order=16; stages=stages}

    let GreenPrefix7 =
        let stages = SorterData.Order16_GreenStr7 |> ParseSorterDataToArrayOfStages
        {StagedSorterDef.order=16; stages=stages}