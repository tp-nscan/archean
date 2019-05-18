namespace archean.core.test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.Sorter
open archean.core.SortersFromData

[<TestClass>]
type SorterFixture () =

    [<TestMethod>]
    member this.TestRunSorterDef() =
        let order = 6
        let sorterLen = 20
        let sortableCount = 4
        let seed = 123
        let rnd = new Random(seed)

        let MakeSortable = Array.init order (fun i -> order - i - 1)

        let SortableFunc (order:int) (rnd : Random) (count:int) =
            Permutation.CreateRandom rnd order
            |> Seq.map(fun i -> Permutation.value i )
            |> Seq.take count

        let switchSet = SwitchSet.ForOrder order
        let sorterDef = SorterDef.CreateRand switchSet sorterLen rnd
        let startPos = 5
        let switchTracker = SwitchTracker.Make sorterDef.switches.Length
        let (res, switchTrack) = UpdateSwitchUses
                                                        sorterDef
                                                        switchTracker 
                                                        startPos 
                                                        (SortableFunc order rnd sortableCount)

        Assert.IsTrue (switchTrack.Value.Length > 0)


    [<TestMethod>]
    member this.TestRunSwitchesAndGetResults() =
        let order = 16
        let prefixStageCount = 3
        let allStageCount = 10
        let seed = 123
        let rnd = new Random(seed)

        let randSorterStagesNoFill = 
            {RandSorterStages.order = order; 
            stageCount=0; 
            randSwitchFill=RandSwitchFill.NoFill}

        let randSorterStagesFullStage = 
            {RandSorterStages.order = order; 
            stageCount=allStageCount; 
            randSwitchFill=RandSwitchFill.FullStage}


        let prefixSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=prefixStageCount}
        let fullSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=allStageCount}

        let prefixedSorterGenMode = SorterGenerationMode.Prefixed(prefixSorterStages, randSorterStagesNoFill)
        let fullSorterGenMode = SorterGenerationMode.Prefixed(fullSorterStages, randSorterStagesFullStage)

        let prefixSorterDef = SortersFromData.CreateRandomSorterDef 
                                prefixedSorterGenMode rnd

        let fullSorterDef = SortersFromData.CreateRandomSorterDef 
                                fullSorterGenMode rnd

             
        let switchTracker = SwitchTracker.Make fullSorterDef.switches.Length
        let (_, sortableRes) = Sorter.RunSorterOnSortableSeq
                                    prefixSorterDef
                                    switchTracker
                                    (SortableIntArray.SortableSeqAllBinary order)
                
        let SortableFunc = sortableRes |> Array.toSeq
        
        let (switchTrack, sortableRes2) = RunSorterOnSortableSeq 
                                                fullSorterDef 
                                                switchTracker 
                                                (SortableFunc)

        Assert.IsTrue (sortableRes2.Length = 17)


    [<TestMethod>]
    member this.TestRunStagedSorter() =
        let length = 229
        let evalStageDex = 1
        let order = 10
        
        let rnd = new Random(49123)
        let stagedSorter = (SorterDef.CreateRandomPackedStages order length rnd) 
                            |> StagedSorterDef.ToStagedSorterDef
        let switchTracker = SwitchTracker.Make stagedSorter.sorterDef.switches.Length

        let res = StagedSorter.GetStagePerfAndSwitchUsage 
                    switchTracker 
                    stagedSorter
                    evalStageDex
                    (SortableIntArray.SortableSeqAllBinary order)
                            

        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestEvalSorterDef() =
        let length = 929
        let evalStageDex = 1
        let order = 16
        let bbub = "[(7,8),(5,9),(4,11),(1,10),(0,13),(3,14),(6,15),(2,12)]
                    [(2,3),(12,14),(0,7),(10,15),(4,5),(8,13),(1,6),(9,11)]
                    [(0,2),(3,7),(8,12),(13,14),(1,4),(6,9),(11,15),(5,10)]
                    [(0,1),(2,5),(9,13),(14,15),(4,8),(10,12),(3,6),(7,11)]
                    [(1,3),(7,8),(2,4),(5,9),(12,14),(6,10),(11,13)]
                    [(1,2),(3,4),(5,7),(8,9),(11,12),(13,14)]
                    [(2,3),(4,6),(10,11),(12,13)]
                    [(4,5),(6,7),(9,11),(8,10)]
                    [(5,6),(7,8),(9,10),(0,1),(3,13),(4,15),(2,12)]"

        let yzkk = "[(0,1),(2,3),(4,5),(6,7),(8,9),(10,11),(12,13),(14,15)]
                    [(0,2),(1,3),(4,6),(5,7),(8,10),(9,11),(12,14),(13,15)]
                    [(0,4),(1,5),(2,6),(3,7),(8,12),(9,13),(10,14),(11,15)]
                    [(0,8),(1,9),(2,10),(3,11),(4,12),(5,13),(6,14),(7,15)]
                    [(1,2),(5,10),(13,14),(6,9),(3,12),(4,8),(7,11)]
                    [(2,8),(9,10),(11,14),(1,4),(5,6),(7,13)]
                    [(2,4),(11,13),(7,12),(3,8)]
                    [(3,5),(7,9),(10,12),(6,8)]
                    [(3,4),(5,6),(7,8),(9,10),(11,12)]
                    [(6,7),(8,9)]"

        let stagedSorterDef = SortersFromData.RefSorter.ParseToStagedSorter
                                    yzkk 16

        let res = CondenseAllZeroOneSortables stagedSorterDef.sorterDef
        Assert.IsTrue (true)


    //[<TestMethod>]
    //member this.TestRunPrefixedSorterDef() =
    //    let order = 16
    //    let totalSwitchCount = 60
    //    let prefixStageCount = 3
    //    let prefixSwitchCount = 24
    //    let allStageCount = 10
    //    let seed = 123
    //    let rnd = new Random(seed)

    //    let SortableFuncAllBinary (order:int) () =
    //        IntBits.AllBinaryTestCases order

    //    let prefixSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=prefixStageCount}
    //    let fullSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=allStageCount}

    //    let prefixedSorterGenMode = RandGenerationMode.Prefixed(prefixSorterStages, RandSwitchFill.NoFill)
    //    let fullSorterGenMode = RandGenerationMode.Prefixed(fullSorterStages, RandSwitchFill.NoFill)

    //    let prefixSorterDef = SortersFromData.CreateRandomSorterDef 
    //                            prefixSwitchCount prefixedSorterGenMode rnd

    //    let fullSorterDef = SortersFromData.CreateRandomSorterDef 
    //                            totalSwitchCount fullSorterGenMode rnd

             
    //    let switchTracker = Array.init totalSwitchCount (fun i -> 0)
    //    let (_, sortableRes) = GetSwitchAndSwitchableResultsForSorter switchTracker 
    //                                        prefixSorterDef (SortableFuncAllBinary order)

    //    let SortableFunc() = sortableRes |> Set.toSeq
        
    //    let (switchTrack, sortableRes2) = GetSwitchAndSwitchableResultsForSorter2 switchTracker 
    //                                        fullSorterDef (SortableFunc)


    //    Assert.IsTrue (switchTrack.Length = totalSwitchCount)
    //    Assert.IsTrue (sortableRes2.Length = 17)