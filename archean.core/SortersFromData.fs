namespace archean.core
open System
open archean.core
open archean.core.Sorting


module SortersFromData =

    type RandSwitchFill = 
         | LooseSwitches
         | FullStage
         | NoFill
    
    type RandSorterStages = {randSwitchFill:RandSwitchFill; stageCount:int}

    type RefSorter =
        | Green16
        | End16

    type RefSorterPrefixStages = {refSorter:RefSorter; stageCount:int}

    type RandGenerationMode = 
         | Prefixed of RefSorterPrefixStages * RandSwitchFill
         | None of RandSwitchFill * int

    let ParseSorterStringToStages (stagesStr:string) =

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
    

    let ParseSorterStringToNStagesOfSwitches (stagesStr:string) (stageCount:int) =
        (ParseSorterStringToStages stagesStr)
        |> Seq.take stageCount
        |> Seq.map(fun s -> s.switches|>List.toSeq)
        |> Seq.concat


    let ParseSorterStringToStagedSorter (stagesStr:string) (order:int)
                                        (stageCount:int) =
       let stages = stagesStr |> ParseSorterStringToStages
                              |> Seq.take stageCount
                              |> Seq.toArray
       {StagedSorterDef.order=order; stages=stages}

    let GetReferenceSorterInfo (refSorter:RefSorter) =
        match refSorter with
        | Green16 -> (SorterData.Order16_Green, 16)
        | End16 -> (SorterData.Order16_END, 16)

    let ParseSorterStringToSorter (stagesStr:string) (order:int) 
                                  (stageCount:int) =
       let switches = stagesStr |> ParseSorterStringToStages
                                |> Seq.take stageCount
                                |> Seq.map(fun s -> s.switches|>List.toSeq)
                                |> Seq.concat |> Seq.toArray
       {SorterDef.order=order; switches=switches}

    let CreateRefSorter (refSorter:RefSorter) (definedStages: int) =
        let (sorterString, order) = (GetReferenceSorterInfo refSorter)
        let switchesFromStages = (ParseSorterStringToNStagesOfSwitches sorterString definedStages)
        {
            SorterDef.order = order;
            switches = switchesFromStages |> Seq.toArray
        }


    let CreatePrefixedRandomSorter (totalSwitches: int) (definedStages: int)
                                   (sorterDef:string, order:int)
                                   (randSwitchFill:RandSwitchFill) (rnd:Random) =
        let switchFiller = 
            match randSwitchFill with
            | LooseSwitches -> (SwitchSet.RandomSwitchesOfOrder order rnd)
            | FullStage -> (Stage.MakeStagePackedSwitchSeq rnd order)
            | NoFill -> Seq.empty

        
        let switchesFromStages = (ParseSorterStringToNStagesOfSwitches sorterDef definedStages)
        let randPadded = seq { yield! switchesFromStages; yield! switchFiller } 
        {
            SorterDef.order = order;
            switches = randPadded
                        |> Seq.take totalSwitches
                        |> Seq.toArray
        }


    let CreatePrefixedSorter (randGenerationMode : RandGenerationMode) =
         match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, _ ) ->
                CreateRefSorter refType refStages
         | None ( _ , order) ->
                {SorterDef.order = order; switches = Array.empty}

        
    let CreateRandomSorterDef (totalSwitches: int) 
                              (randGenerationMode : RandGenerationMode) 
                              (rnd : Random) =

        match randGenerationMode with
         | Prefixed ({refSorter=refType; stageCount=refStages}, randSwitchFill) ->
            CreatePrefixedRandomSorter
                            totalSwitches refStages 
                            (GetReferenceSorterInfo refType) 
                            randSwitchFill rnd
         | None (randSwitchFill, order) ->
            match randSwitchFill with
                | LooseSwitches -> SorterDef.CreateRandom order totalSwitches rnd
                | FullStage -> SorterDef.CreateRandomPackedStages order totalSwitches rnd
                | NoFill -> {SorterDef.order = order; switches = Array.empty}
                 