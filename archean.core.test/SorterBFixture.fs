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
        let order = 5
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

        let res = RunSorterDef sorterDef (SortableFunc order rnd sortableCount)

        Assert.IsTrue (res.Length > 0)


    [<TestMethod>]
    member this.TestRunSorterDef2() =
        let order = 16
        let totalSwitches = 70
        let prefixStages = 9
        let sortableCount = 400
        let seed = 123
        let rnd = new Random(seed)

        let MakeSortable = Array.init order (fun i -> order - i - 1)

        let SortableFunc (order:int) (rnd : Random) (count:int) () =
            Permutation.CreateRandom rnd order
            |> Seq.map(fun i -> Permutation.value i )
            |> Seq.take count

        let SortableFunc2 (order:int) (rnd : Random) (count:int) () =
            IntBits.AllBinaryTestCases order

        let switchSet = SwitchSet.ForOrder order
        let sorterDef = SortersFromData.CreateRandomSorterDef 16 totalSwitches
                            (RandGenerationMode.End16 (prefixStages, RandSwitchFill.FullStage))
                            rnd

        let res = RunSorterDef2 sorterDef (SortableFunc2 order rnd sortableCount)

        Assert.IsTrue ((fst res).Length > 0)