namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics
open archean.core.Combinatorics_Types
open archean.core.Sorting

[<TestClass>]
type SortingFixture () =


    [<TestMethod>]
    member this.TestSwitchFromPermutation() =
        let order = 650
        let rnd = new Random(123)
        let perm = Permutation.CreateRandom rnd order 1 |> Seq.item 0
        let switchset = Switch.FromPermutation perm |> Seq.toArray
        Assert.IsTrue (switchset.Length < order / 2)

        
    [<TestMethod>]
    member this.TestSwitchFromPolyCycle() =
        let order = 15
        let rnd = new Random(123)
        let polyCycle = TwoCycleIntArray.MakeRandomPolyCycle rnd order 1 |> Seq.item 0
        let switchset = Switch.FromPolyCycle polyCycle |> Seq.toArray
        Assert.IsTrue (switchset.Length = order / 2)

    [<TestMethod>]
    member this.TestMakeSwitchSet() =
        let res = SwitchSet.ForOrder 5
        Assert.IsTrue (res.order = 5)
        Assert.IsTrue (res.switches.Length = 10)

    
    [<TestMethod>]
    member this.TestMakeRandomSorter() =
        let length = 29
        let order = 5
        
        let switchSet = SwitchSet.ForOrder order
        let rnd = new Random(123)
        let res = SorterDef.CreateRand switchSet rnd length

        Assert.IsTrue (res.order = order)
        Assert.IsTrue (res.switches.Length = length)


    [<TestMethod>]
    member this.TestSortable4() =
        let length = 29
        let order = 5
        
        let rnd = new Random(123)
        let s4 = Sortables.Sortable4.CreateRandom rnd
        let sI = Sortables.Sortable4.Identity
        let iO = Sortables.Sortable4.IsSorted s4
        let i1 = Sortables.Sortable4.IsSorted sI
        let a4 = Sortables.Sortable4.ToArray s4
        let sw4 = Sortables.Sortable4.SwitchFuncs.[0] s4
        let b4 = Sortables.Sortable4.ToArray (sw4 |> fst)

        Assert.IsTrue (a4.Length = 4)
        Assert.IsTrue (b4.Length = 4)

        Assert.IsTrue (true)
 
 
    [<TestMethod>]
    member this.TestSortable12AllBinaryTestCases() =
        let cases = Sortables.Sortable12.AllBinaryTestCases |> Seq.toArray

        Assert.IsTrue (cases.Length = 4096)
        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestMakeSorter() =
        let rnd = new Random(123)
        let order = 4
        let len = 160
        let sorterDef = SorterDef.CreateRandom order rnd len
        let sorter = Sorter.MakeSorter Sortables.Sortable4.SwitchFuncForSwitch sorterDef
        let res = Sorter.Sort sorter (Sortables.Sortable4.CreateRandom rnd)
        Assert.IsTrue (Sortables.Sortable4.IsSorted (snd res))
 

    [<TestMethod>]
    member this.TestCountSwitchUse() =
        let rnd = new Random(123)
        let order = 4
        let len = 160
        let sorterDef = SorterDef.CreateRandom order rnd len
        let sorter = Sorter.MakeSorter Sortables.Sortable4.SwitchFuncForSwitch sorterDef
        let res = Sorter.Sort sorter (Sortables.Sortable4.CreateRandom rnd)
        Assert.IsTrue (Sortables.Sortable4.IsSorted (snd res))


    [<TestMethod>]
    member this.TestSortOneAndTrackSwitches() =
        let rnd = new Random(123)
        let order = 4
        let len = 16
        let switchTracker = Array.init len (fun i -> 0)
        let sorterDef = SorterDef.CreateRandom order rnd len
        let sorter = Sorter.MakeSorter Sortables.Sortable4.SwitchFuncForSwitch sorterDef
        let sortable = Sortables.Sortable4.FromArray [|1; 0; 1; 0|]
        let res = Sorter.SortOneAndTrackSwitches sorter switchTracker sortable
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestSortManyAndTrackSwitches() =
        let sortable0 = Sortables.Sortable4.FromArray [|0; 1; 1; 0|]
        let sortable1 = Sortables.Sortable4.FromArray [|1; 0; 1; 0|]
        let sseq = seq {yield sortable0; yield sortable1}
        let checker t = Sortables.Sortable4.IsSorted t

        let rnd = new Random(123)
        let order = 4
        let len = 16
        let switchTracker = Array.init len (fun i -> 0)
        let sorterDef = SorterDef.CreateRandom order rnd len
        let sorter = Sorter.MakeSorter Sortables.Sortable4.SwitchFuncForSwitch sorterDef
        let res = Sorter.SortManyTrackSwitchesAndCheckResults checker sorter switchTracker sseq
        Assert.IsTrue (fst res)

    [<TestMethod>]
    member this.TestGetSwitchCountForSorter() =
        let rnd = new Random(123)
        let order = 4
        let len = 160
        let sorterDef = SorterDef.CreateRandom order rnd len
        let sorter = Sorter.MakeSorter Sortables.Sortable4.SwitchFuncForSwitch sorterDef
        let res = Sortables.Sortable4.GetSwitchCountForSorter sorter
        Assert.IsTrue (true)



    [<TestMethod>]
    member this.TestSortableGen() =
        let order = 12
        Console.Write (SortableGen.GenN order)
        
        Assert.IsTrue (true)

