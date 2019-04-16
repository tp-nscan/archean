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
    member this.TestRunSorterDef2() =
        let order = 16
        let totalSwitches = 70
        let prefixStages = 3
        let prefixSwitches = 24
        let allStages = 10
        let sortableCount = 400
        let seed = 123
        let rnd = new Random(seed)

        let MakeSortable = Array.init order (fun i -> order - i - 1)

  

        let SortableFuncAllBinary (order:int) () =
            IntBits.AllBinaryTestCases order

        let switchSet = SwitchSet.ForOrder order
        let prefixSorterDef = SortersFromData.CreateRandomSorterDef 16 prefixSwitches
                               (RandGenerationMode.End16 (prefixStages, RandSwitchFill.FullStage))
                               rnd

        let fullSorterDef = SortersFromData.CreateRandomSorterDef 16 totalSwitches
                               (RandGenerationMode.End16 (allStages, RandSwitchFill.FullStage))
                               rnd
                               
        let prefixTracker = Array.init prefixSorterDef.switches.Length (fun i -> 0)
        let res = GetSwitchAndSwitchableResultsForSorter prefixTracker prefixSorterDef (SortableFuncAllBinary order)

        let SortableFunc() = (snd res) |> Set.toSeq
        
        let fullTracker = Array.init fullSorterDef.switches.Length (fun i -> 0)
        let res2 = GetSwitchAndSwitchableResultsForSorter fullTracker fullSorterDef (SortableFunc)


        Assert.IsTrue ((fst res).Length > 0)
        Assert.IsTrue ((fst res2).Length > 0)