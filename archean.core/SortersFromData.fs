namespace archean.core
open System
open archean.core
open archean.core.Sorting

module SortersFromData =

    type RefSorter =
        | Order8Str
        | Order10Str
        | Green16
        | End16
    
    module RefSorter =

        let GetStringAndOrder (refSorter:RefSorter) =
            match refSorter with
            | Order8Str -> (SorterData.Order8Str, 8)
            | Order10Str -> (SorterData.Order10Str, 10)
            | Green16 -> (SorterData.Order16_Green, 16)
            | End16 -> (SorterData.Order16_END, 16)


        let ParseToStages (stagesStr:string) =

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
            

        let ParseToNStagesOfSwitches (stagesStr:string) (stageCount:int) =
            (ParseToStages stagesStr)
            |> Seq.take stageCount
            |> Seq.map(fun s -> s.switches |> List.toSeq)
            |> Seq.concat


        let ParseToSorter (sorterString:string) 
                          (order:int) 
                          (stageCount:int) =
            {SorterDef.order=order; switches= ParseToNStagesOfSwitches sorterString stageCount
                                                            |> Seq.toArray}
            

        let CreateRefSorter (refSorter:RefSorter) (definedStages: int) =
            let (sorterString, order) = (GetStringAndOrder refSorter)
            ParseToSorter sorterString order definedStages


        let ParseToStagedSorter (stagesStr:string) 
                                (order:int) =

            let stagesArray = (ParseToStages stagesStr)
                                |> Seq.toArray

            let mutable curTotal = 0
            let stageIndexes = seq { yield curTotal;
                                        for i = 0 to stagesArray.Length - 1 do
                                        curTotal <- curTotal + stagesArray.[i].switches.Length
                                        yield curTotal }
                                |> Seq.toArray
 
            {
                StagedSorterDef.sorterDef = 
                    { SorterDef.order = order; 
                      switches = stagesArray |> StagedSorterDef.StageArrayToSwitchArray};
                StagedSorterDef.stageIndexes = stageIndexes
            }


        let CreateRefStagedSorter (refSorter:RefSorter) =
                let (sorterString, order) = (GetStringAndOrder refSorter)
                ParseToStagedSorter sorterString order


        //let ParseToStagedSorter (stagesStr:string) 
        //                        (order:int)
        //                        (stageCount:int ) =
        //    let stagesArray = (ParseToStages stagesStr)
        //                        |> Seq.take stageCount
        //                        |> Seq.toArray
        //    let switchesArray = stagesArray
        //                        |> Array.map(fun st -> st.switches |> List.toSeq)
        //                        |> Seq.concat |> Seq.toArray

        //    let mutable curTotal = 0
        //    let stageIndexes = seq { yield curTotal;
        //                                for i = 0 to stagesArray.Length - 1 do
        //                                curTotal <- curTotal + stagesArray.[i].switches.Length
        //                                yield curTotal }
        //                        |> Seq.toArray
                
        //    { 
        //        StagedSorterDef.sorterDef = 
        //            { SorterDef.order = order; switches = switchesArray };
        //        StagedSorterDef.stageIndexes = stageIndexes
        //    }


        //let CreateRefStagedSorter (refSorter:RefSorter) (definedStages: int) =
        //        let (sorterString, order) = (GetStringAndOrder refSorter)
        //        ParseToStagedSorter sorterString order definedStages



    type RandSwitchFill = 
         | LooseSwitches
         | FullStage
         | NoFill

    type RandSorterStages = {order:int; stageCount:int; randSwitchFill:RandSwitchFill;}

    type RandSorterSwitches = {order:int; switchCount:int; randSwitchFill:RandSwitchFill;}

    type RefSorterPrefixStages = {refSorter:RefSorter; stageCount:int}

    type RefSorterSwitches = {refSorter:RefSorter; switchCount:int}

    type RandGenerationMode = 
         | Prefixed of RefSorterPrefixStages * RandSorterStages
         | Pure of RandSorterStages
    
    type StagedSorterType =
         | Ref of RefSorterSwitches
         | PrefixedRand of RefSorterSwitches * RandSorterSwitches
         | Rand of RandSorterSwitches


    let To_PrefixStageCount (randGenerationMode:RandGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=_; stageCount=refStageCount}, 
                     _) ->
                        refStageCount
         | Pure _ -> 0


    let RemoveRandomStages (randGenerationMode:RandGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStageCount}, 
                     {order=order; stageCount=_; randSwitchFill=randSwitchFill;}) ->
                     RandGenerationMode.Prefixed 
                        ({refSorter=refType; stageCount=refStageCount}, 
                        {order=order; stageCount=refStageCount; randSwitchFill=randSwitchFill;})
         | Pure { order=order; stageCount=randStageCount; randSwitchFill=rsf } -> 
                    RandGenerationMode.Pure 
                        { order=order; stageCount=0; randSwitchFill=rsf }


    let To_RefSorter_PrefixStages (randGenerationMode:RandGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStageCount}, 
                      _ ) ->
                     sprintf "%A\t%d" refType refStageCount
         | Pure _ -> "\t"


    let GetSorterStageCount (randGenerationMode:RandGenerationMode) =
        match randGenerationMode with
         | Prefixed ( _, 
                     {order=order; stageCount=totalStageCount; randSwitchFill=_;}) ->
                      totalStageCount * order / 2
         | Pure { order=_; stageCount=totalStageCount; randSwitchFill=_ } ->
                      totalStageCount / 2


    let GetSorterSwitchCount (randGenerationMode:RandGenerationMode) =
        match randGenerationMode with
         | Prefixed (_, 
                     {order=order; stageCount=totalStageCount; randSwitchFill=_;}) ->
                     (totalStageCount * order )/2
         | Pure { order=order; stageCount=totalStageCount; randSwitchFill=_ } ->
                     ( totalStageCount * order ) / 2


    let CreatePrefixedRandomSorter (totalSwitches: int) (prefixStageCount: int)
                                   (prefixStagesString:string, order:int)
                                   (randSwitchFill:RandSwitchFill) (rnd:Random) =
        let switchFiller = 
            match randSwitchFill with
            | LooseSwitches -> (SwitchSet.RandomSwitchesOfOrder order rnd)
            | FullStage -> (Stage.MakeStagePackedSwitchSeq rnd order)
            | NoFill -> Seq.empty

        let switchesFromStages = (RefSorter.ParseToNStagesOfSwitches prefixStagesString prefixStageCount)
        let randPadded = seq { yield! switchesFromStages; yield! switchFiller } 
        {
            SorterDef.order = order;
            switches = randPadded
                        |> Seq.take totalSwitches
                        |> Seq.toArray
        }


    let CreatePrefixedRandomStagedSorter 
                (totalSwitches: int) 
                (prefixStageCount: int)
                (prefixStagesString:string, order:int)
                (randSwitchFill:RandSwitchFill) 
                (rnd:Random) =

        let switchFiller = 
            match randSwitchFill with
            | LooseSwitches -> (SwitchSet.RandomSwitchesOfOrder order rnd)
            | FullStage -> (Stage.MakeStagePackedSwitchSeq rnd order)
            | NoFill -> Seq.empty

        let prefixStagedSorter = RefSorter.ParseToStagedSorter prefixStagesString order

        let retVal = prefixStagedSorter
                    |> StagedSorter.TruncateStages prefixStageCount
                    |> StagedSorterDef.AppendSwitches 
                           (switchFiller
                           |> Seq.take (totalSwitches - prefixStagedSorter.sorterDef.switches.Length))
        retVal


    let CreatePrefixedSorter (randGenerationMode : RandGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, _ ) ->
                RefSorter.CreateRefSorter refType refStages
         | Pure { order=order; stageCount=_; randSwitchFill=_ } ->
                {SorterDef.order = order; switches = Array.empty}


    let CreateRandomSorterDef (randGenerationMode : RandGenerationMode) 
                              (rnd : Random) =
        match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, 
                     {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;}) ->
            CreatePrefixedRandomSorter
                            (stageCount*order / 2) refStages 
                            (RefSorter.GetStringAndOrder refType) 
                            randSwitchFill rnd
         | Pure {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;} ->
            match randSwitchFill with
                | LooseSwitches -> SorterDef.CreateRandom order (stageCount*order / 2) rnd
                | FullStage -> SorterDef.CreateRandomPackedStages order (stageCount*order / 2) rnd
                | NoFill -> {SorterDef.order = order; switches = Array.empty}

 
    let CreateRandomStagedSorterDef (randGenerationMode : RandGenerationMode) 
                                    (rnd : Random) =
        match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, 
                     {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;}) ->
            CreatePrefixedRandomStagedSorter
                            (stageCount*order / 2) refStages 
                            (RefSorter.GetStringAndOrder refType) 
                            randSwitchFill rnd
         | Pure {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;} ->
            match randSwitchFill with
                | LooseSwitches -> (SorterDef.CreateRandom order (stageCount*order / 2) rnd) 
                                    |> StagedSorterDef.ToStagedSorterDef
                | FullStage -> (SorterDef.CreateRandomPackedStages order (stageCount*order / 2) rnd)
                                    |> StagedSorterDef.ToStagedSorterDef
                | NoFill -> ({SorterDef.order = order; switches = Array.empty})
                                    |> StagedSorterDef.ToStagedSorterDef


    let SortableTestCases (randGenerationMode:RandGenerationMode) =
        let (_, sortableRes) = 
            Sorter.CondenseAllZeroOneSortables
                (CreatePrefixedSorter randGenerationMode)
        sortableRes


    let WeightedSortableTestCases (randGenerationMode:RandGenerationMode) =
        let (_, sortableRes) = 
            Sorter.CondenseAllZeroOneWeightedSortables
                (CreatePrefixedSorter randGenerationMode)
        sortableRes
