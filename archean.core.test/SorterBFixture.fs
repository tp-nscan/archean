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
        
        let switchTracker = Array.init sorterDef.switches.Length (fun i -> 0)
        let (res, switchTrack) = GetSwitchResultsForSorterAndCheckResults switchTracker 
                                        sorterDef (SortableFunc order rnd sortableCount)

        Assert.IsTrue (switchTrack.Length > 0)


    [<TestMethod>]
    member this.TestRunPrefixedSorterDef() =
        let order = 16
        let totalSwitchCount = 70
        let prefixStageCount = 3
        let prefixSwitchCount = 24
        let allStageCount = 10
        let seed = 123
        let rnd = new Random(seed)

        let SortableFuncAllBinary (order:int) () =
            IntBits.AllBinaryTestCases order

        let prefixSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=prefixStageCount}
        let fullSorterStages = {RefSorterPrefixStages.refSorter=End16; stageCount=allStageCount}

        let prefixedSorterGenMode = RandGenerationMode.Prefixed(prefixSorterStages, RandSwitchFill.NoFill)
        let fullSorterGenMode = RandGenerationMode.Prefixed(fullSorterStages, RandSwitchFill.NoFill)

        let prefixSorterDef = SortersFromData.CreateRandomSorterDef 
                                prefixSwitchCount prefixedSorterGenMode rnd

        let fullSorterDef = SortersFromData.CreateRandomSorterDef 
                                totalSwitchCount fullSorterGenMode rnd

             
        let switchTracker = Array.init totalSwitchCount (fun i -> 0)
        let (_, sortableRes) = GetSwitchAndSwitchableResultsForSorter switchTracker 
                                            prefixSorterDef (SortableFuncAllBinary order)

        let SortableFunc() = sortableRes |> Set.toSeq
        
        let (switchTrack, sortableRes2) = GetSwitchAndSwitchableResultsForSorter switchTracker 
                                            fullSorterDef (SortableFunc)


        Assert.IsTrue (switchTrack.Length = totalSwitchCount)
        Assert.IsTrue (sortableRes2.Count = 17)