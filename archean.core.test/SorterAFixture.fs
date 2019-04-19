namespace archean.core.test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.SortersFromData
open archean.core.Sorting
open archean.core.SorterA

[<TestClass>]
type SorterAFixture () =

    [<TestMethod>]
    member this.TestMakeSorterForSortableIntArray() =
        let rnd = new Random(123)
        let order = 6
        let len = 160
        let sorterDef = SorterDef.CreateRandom order len rnd
        let sorter = Sorter.MakeSorter SortableIntArray.SwitchFuncForSwitch sorterDef
        let sortable = (SortableIntArray.CreateRandom order rnd) |> Seq.item 0
        let res = Sorter.Sort sorter sortable.values
        Assert.IsTrue (Combinatorics.IsSorted (snd res))

    //[<TestMethod>]
    //member this.TestGetFullSortingResultsForSortableIntArray() =
    //    let rnd = new Random(123)
    //    let order = 10
    //    let len = 160
    //    let sorterDef = SorterDef.CreateRandom order len rnd
    //    let res = SortingReports.GetFullSortingResultsUsingIntArray sorterDef
    //    Assert.IsTrue (true)

    //[<TestMethod>]
    //member this.TestGetSorterSuccessRates() =
    //    let sorterCount = 1
    //    let order = 11
    //    let seed = 100
    //    let mode = RandGenerationMode.None(RandSwitchFill.FullStage, order)

    //    let res = seq {260 .. 10 .. 460} 
    //                    |> Seq.map(fun i -> SortingReports.MakeStageAndSwitchUseHistogram 
    //                                            order i mode sorterCount seed)
    //                    |> Seq.toArray

    //    Assert.IsTrue (true)

