namespace archean.core
open System
open archean.core
open archean.core.Sorting


module SortersFromData =

    type RandSwitchFill = 
         | LooseSwitches
         | FullStage
    
    //(Stages:int)
    type ReferenceStages =
         | Green of int
         | End16 of int
         
    type RandGenerationMode = 
         | Green of int * RandSwitchFill
         | End16 of int * RandSwitchFill
         | None of RandSwitchFill


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


    let ParseSorterStringToSorter (stagesStr:string) (order:int) 
                                  (stageCount:int) =
       let switches = stagesStr |> ParseSorterStringToStages
                                |> Seq.take stageCount
                                |> Seq.map(fun s -> s.switches|>List.toSeq)
                                |> Seq.concat |> Seq.toArray
       {SorterDef.order=order; switches=switches}


    let CreateDefinedPrefixSorter (totalSwitches: int) (definedStages: int)
                                   (sorterDef:string) (order:int)
                                   (randSwitchFill:RandSwitchFill) (rnd:Random) =
        let switchFiller = 
            match randSwitchFill with
            | LooseSwitches -> (SwitchSet.RandomSwitchesOfOrder order rnd)
            | FullStage -> (Stage.MakeStagePackedSwitchSeq rnd order) 
                            
        {
            SorterDef.order = order;
            switches = switchFiller
                        |> Seq.append (ParseSorterStringToNStagesOfSwitches 
                                            sorterDef definedStages)
                        |> Seq.take totalSwitches
                        |> Seq.toArray
            |> Seq.toArray
        }

        
    let CreateRandomSorterDef (order:int) (totalSwitches: int) 
                              (randGenerationMode : RandGenerationMode) 
                              (rnd : Random) =
            match randGenerationMode with
            | Green (definedStages, randSwitchFill) -> CreateDefinedPrefixSorter
                                                          totalSwitches definedStages 
                                                          SorterData.Order16_Green order 
                                                          randSwitchFill rnd
            | End16 (definedStages, randSwitchFill) -> CreateDefinedPrefixSorter
                                                          totalSwitches definedStages 
                                                          SorterData.Order16_END order 
                                                          randSwitchFill rnd
            | None randFill ->
                match randFill with
                  | LooseSwitches -> SorterDef.CreateRandom order totalSwitches rnd
                  | FullStage -> SorterDef.CreateRandomPackedStages order totalSwitches rnd
                 