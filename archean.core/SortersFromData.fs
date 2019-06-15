namespace archean.core
open System
open archean.core
open archean.core.Sorting

module SortersFromData =

    type RefSorter =
        | Order8
        | Order8Prefix3
        | Order10
        | Green16
        | End16
        | Order18
        | Order20
        | Order22
        | Order23
        | Order24
        | Order25
        | Order26
        | Order28
        | Order32
    

    module RefSorter =

        let GetStringAndOrder (refSorter:RefSorter) =
            match refSorter with
            | Order8 -> (SorterData.Order8Str, 8)
            | Order8Prefix3 -> (SorterData.Order8Prefix3Str, 8)
            | Order10 -> (SorterData.Order10Str, 10)
            | Green16 -> (SorterData.Order16_Green, 16)
            | End16 -> (SorterData.Order16_END, 16)
            | Order18 -> (SorterData.Order18Str, 18)
            | Order20 -> (SorterData.Order20Str, 20)
            | Order22 -> (SorterData.Order22Str, 22)
            | Order23 -> (SorterData.Order23Str, 23)
            | Order24 -> (SorterData.Order24aStr, 24)
            | Order25 -> (SorterData.Order25Str, 25)
            | Order26 -> (SorterData.Order26Str, 26)
            | Order28 -> (SorterData.Order28Str, 28)
            | Order32 -> (SorterData.Order32Str, 32)
            


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




    type RandSwitchFill = 
         | LooseSwitches
         | FullStage
         | NoFill

    type RandSorterStages = {order:int; stageCount:int; randSwitchFill:RandSwitchFill;}

    type RandSorterSwitches = {order:int; switchCount:int; randSwitchFill:RandSwitchFill;}

    type RefSorterPrefixStages = {refSorter:RefSorter; stageCount:int}

    type RefSorterSwitches = {refSorter:RefSorter; switchCount:int}

    type SorterGenerationMode = 
         | Prefixed of RefSorterPrefixStages * RandSorterStages
         | Randy of RandSorterStages
    
    type StagedSorterType =
         | Ref of RefSorterSwitches
         | PrefixedRand of RefSorterSwitches * RandSorterSwitches
         | Rand of RandSorterSwitches


    let To_PrefixStageCount (randGenerationMode:SorterGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=_; stageCount=refStageCount}, 
                     _) ->
                        refStageCount
         | Randy _ -> 0


    let RemoveRandomStages (randGenerationMode:SorterGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStageCount}, 
                     {order=order; stageCount=_; randSwitchFill=randSwitchFill;}) ->
                     SorterGenerationMode.Prefixed 
                        ({refSorter=refType; stageCount=refStageCount}, 
                        {order=order; stageCount=refStageCount; randSwitchFill=randSwitchFill;})
         | Randy { order=order; stageCount=randStageCount; randSwitchFill=rsf } -> 
                    SorterGenerationMode.Randy 
                        { order=order; stageCount=0; randSwitchFill=rsf }


    let To_RefSorter_PrefixStages (randGenerationMode:SorterGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStageCount}, 
                      _ ) ->
                     sprintf "%A\t%d" refType refStageCount
         | Randy _ -> "\t"


    let GetSorterStageCount (randGenerationMode:SorterGenerationMode) =
        match randGenerationMode with
         | Prefixed ( _, 
                      {order=order; stageCount=totalStageCount; randSwitchFill=_;}) ->
                      totalStageCount * order / 2
         | Randy { order=_; stageCount=totalStageCount; randSwitchFill=_ } ->
                      totalStageCount / 2


    let GetSorterSwitchCount (randGenerationMode:SorterGenerationMode) =
        match randGenerationMode with
         | Prefixed (_, 
                     {order=order; stageCount=totalStageCount; randSwitchFill=_;}) ->
                     (totalStageCount * order )/2
         | Randy { order=order; stageCount=totalStageCount; randSwitchFill=_ } ->
                     ( totalStageCount * order ) / 2



    let ParseToNStagesOfSwitches (stagesStr:string) (stageCount:int) =
         (RefSorter.ParseToStages stagesStr)
         |> Seq.take stageCount
         |> Seq.map(fun s -> s.switches |> List.toSeq)
         |> Seq.concat


    let CreatePrefixedRandomSorter (totalSwitches: int) (prefixStageCount: int)
                                   (prefixStagesString:string, order:int)
                                   (randSwitchFill:RandSwitchFill) (rnd:Random) =
        let switchFiller = 
            match randSwitchFill with
            | LooseSwitches -> (Switch.RandomSwitchesOfOrder order rnd)
            | FullStage -> (Stage.MakeStagePackedSwitchSeq rnd order)
            | NoFill -> Seq.empty

        let switchesFromStages = (ParseToNStagesOfSwitches prefixStagesString prefixStageCount)
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
            | LooseSwitches -> (Switch.RandomSwitchesOfOrder order rnd)
            | FullStage -> (Stage.MakeStagePackedSwitchSeq rnd order)
            | NoFill -> Seq.empty

        let prefixStagedSorter = RefSorter.ParseToStagedSorter prefixStagesString order

        let retVal = prefixStagedSorter
                    |> StagedSorter.TruncateStages prefixStageCount
                    |> StagedSorterDef.AppendSwitches 
                           (switchFiller
                           |> Seq.take (totalSwitches - prefixStagedSorter.sorterDef.switches.Length))
        retVal


    let ParseToSorter (sorterString:string) 
                      (order:int) 
                      (stageCount:int) =
        {SorterDef.order=order; switches= ParseToNStagesOfSwitches sorterString stageCount
                                                        |> Seq.toArray}
    
    let CreateRefSorter (refSorter:RefSorter) (definedStages: int) =
        let (sorterString, order) = (RefSorter.GetStringAndOrder refSorter)
        ParseToSorter sorterString order definedStages


    let CreatePrefixedSorter (randGenerationMode : SorterGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, _ ) ->
                CreateRefSorter refType refStages
         | Randy { order=order; stageCount=_; randSwitchFill=_ } ->
                {SorterDef.order = order; switches = Array.empty}


    let CreateRandomSorterDef (randGenerationMode:SorterGenerationMode) 
                              (rnd:Random) =
        match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, 
                     {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;}) ->
            CreatePrefixedRandomSorter
                            (stageCount*order / 2) refStages 
                            (RefSorter.GetStringAndOrder refType) 
                            randSwitchFill rnd
         | Randy {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;} ->
            match randSwitchFill with
                | LooseSwitches -> SorterDef.CreateRandom order (stageCount*order / 2) rnd
                | FullStage -> SorterDef.CreateRandomPackedStages order (stageCount*order / 2) rnd
                | NoFill -> {SorterDef.order = order; switches = Array.empty}

 
    let CreateRandomStagedSorterDef (randGenerationMode : SorterGenerationMode) 
                                    (rnd : Random) =
        match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, 
                     {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;}) ->
            CreatePrefixedRandomStagedSorter
                            (stageCount*order / 2) refStages 
                            (RefSorter.GetStringAndOrder refType) 
                            randSwitchFill rnd
         | Randy {order=order; stageCount=stageCount; randSwitchFill=randSwitchFill;} ->
            match randSwitchFill with
                | LooseSwitches -> (SorterDef.CreateRandom order (stageCount*order / 2) rnd) 
                                    |> StagedSorterDef.ToStagedSorterDef
                | FullStage -> (SorterDef.CreateRandomPackedStages order (stageCount*order / 2) rnd)
                                    |> StagedSorterDef.ToStagedSorterDef
                | NoFill -> ({SorterDef.order = order; switches = Array.empty})
                                    |> StagedSorterDef.ToStagedSorterDef


    let SortableTestCases (randGenerationMode:SorterGenerationMode) =
        let (_, sortableRes) = 
            Sorter.CondenseAllZeroOneSortables
                (CreatePrefixedSorter randGenerationMode)
        sortableRes
