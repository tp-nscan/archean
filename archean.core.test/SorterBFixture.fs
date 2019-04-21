namespace archean.core.test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SorterB
open archean.core.SortersFromData

[<TestClass>]
type SorterBFixture () =

    [<TestMethod>]
    member this.TestRunSorterDef() =
        let order = 6
        let sorterLen = 20
        let sortableCount = 4
        let seed = 123
        let rnd = new Random(seed)

        let MakeSortable = Array.init order (fun i -> order - i - 1)

        let SortableFunc (order:int) (rnd : Random) (count:int) () =
            Permutation.CreateRandom rnd order
            |> Seq.map(fun i -> Permutation.value i )
            |> Seq.take count

        let switchSet = SwitchSet.ForOrder order
        let sorterDef = SorterDef.CreateRand switchSet sorterLen rnd
        let startPos = 5
        let switchTracker = Array.init sorterDef.switches.Length (fun i -> 0)
        let (res, switchTrack) = GetSwitchUsagesForGoodSorters switchTracker 
                                        sorterDef startPos (SortableFunc order rnd sortableCount)

        Assert.IsTrue (switchTrack.Value.Length > 0)


    [<TestMethod>]
    member this.TestRunWeightedSwitchesAndGetWeightedResults() =
        let order = 16
        let totalStages = 10
        let prefixStageCount = 3
        let allStageCount = 10
        let seed = 123
        let rnd = new Random(seed)

        let randSorterStages = {RandSorterStages.order = order; stageCount=totalStages; randSwitchFill=RandSwitchFill.NoFill}

        let SortableFuncAllBinary (order:int) () =
            IntBits.AllBinaryTestCases order
            |> Seq.map(fun i -> (i, 1))

        let prefixSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=prefixStageCount}
        let fullSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=allStageCount}

        let prefixedSorterGenMode = RandGenerationMode.Prefixed(prefixSorterStages, randSorterStages)
        let fullSorterGenMode = RandGenerationMode.Prefixed(fullSorterStages, randSorterStages)

        let prefixSorterDef = SortersFromData.CreateRandomSorterDef 
                                prefixedSorterGenMode rnd

        let fullSorterDef = SortersFromData.CreateRandomSorterDef 
                                fullSorterGenMode rnd

             
        let switchTracker = Array.init totalStages (fun i -> 0)
        let (_, sortableRes) = RunWeightedSortablesOnSorterAndReturnResultsSet switchTracker 
                                            prefixSorterDef (SortableFuncAllBinary order)

        let SortableFunc() = sortableRes |> Array.toSeq
        
        let (switchTrack, sortableRes2) = RunWeightedSortablesOnSorterAndReturnResultsSet switchTracker 
                                            fullSorterDef (SortableFunc)


        Assert.IsTrue (switchTrack.Length = totalStages)
        Assert.IsTrue (sortableRes2.Length = 17)



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