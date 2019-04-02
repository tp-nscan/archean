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
    member this.TestMergeSwitchResultsIntoStageResults() =
        let order = 5
        let swrSet0 = [|
                        {SwitchResult.switch={Switch.low = 1; hi = 2}; switchIndex=0; useCount=1};
                        {SwitchResult.switch={Switch.low = 1; hi = 3}; switchIndex=1; useCount=1};
                        {SwitchResult.switch={Switch.low = 2; hi = 4}; switchIndex=2; useCount=1};
                        {SwitchResult.switch={Switch.low = 0; hi = 1}; switchIndex=3; useCount=1};
                        {SwitchResult.switch={Switch.low = 0; hi = 1}; switchIndex=4; useCount=1};
                        {SwitchResult.switch={Switch.low = 2; hi = 3}; switchIndex=5; useCount=1};
                        {SwitchResult.switch={Switch.low = 3; hi = 4}; switchIndex=6; useCount=1};
                        {SwitchResult.switch={Switch.low = 1; hi = 2}; switchIndex=7; useCount=1};
                        {SwitchResult.switch={Switch.low = 0; hi = 3}; switchIndex=8; useCount=1};
                     |]
                    

        let res0 = Sorting.Sorter.MergeSwitchResultsIntoStageResults order swrSet0

        let swrSet1 = [|
                        {SwitchResult.switch={Switch.low = 1; hi = 2}; switchIndex=0; useCount=1};
                        {SwitchResult.switch={Switch.low = 1; hi = 3}; switchIndex=1; useCount=1};
                        {SwitchResult.switch={Switch.low = 2; hi = 4}; switchIndex=2; useCount=1};
                        {SwitchResult.switch={Switch.low = 0; hi = 1}; switchIndex=3; useCount=1};
                        {SwitchResult.switch={Switch.low = 0; hi = 1}; switchIndex=4; useCount=1};
                        {SwitchResult.switch={Switch.low = 2; hi = 3}; switchIndex=5; useCount=1};
                        {SwitchResult.switch={Switch.low = 3; hi = 4}; switchIndex=6; useCount=1};
                        {SwitchResult.switch={Switch.low = 0; hi = 2}; switchIndex=7; useCount=1};
                     |]

        let res1 = Sorting.Sorter.MergeSwitchResultsIntoStageResults order swrSet1


        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestGetSwitchCountForSorter() =
        let rnd = new Random(123)
        let order = 12
        let len = 360
        let sorterDef = SorterDef.CreateRandom order rnd len
        let sorter = Sorter.MakeSorter Sortables.Sortable12.SwitchFuncForSwitch sorterDef
        let res = Sortables.Sortable12.GetSwitchCountForSorter sorter
        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestGetSorterResultForSorter10() =
        let reps = 10
        let rnd = new Random(2883)
        let order = 10
        let len = 500
        let GetSorterWithResults (i:int) =
            let sorterDef = SorterDef.CreateRandom order rnd len
            let sorter = Sorter.MakeSorter Sortables.Sortable10.SwitchFuncForSwitch sorterDef
            (Sortables.Sortable10.GetSwitchResultsForSorter sorter Sortables.Sortable10.AllBinaryTestCases, sorterDef)
        
        let counts = {1 .. reps} |> Seq.map(fun i -> GetSorterWithResults i)
                                 |> Seq.filter(fun res -> fst (fst res))
                                 |> Seq.map(fun i -> (Sorting.Sorter.MergeSwitchResultsIntoStageResults order (snd (fst i))))
                                 |> Seq.toArray

        //counts |> Array.iter(fun i -> Console.WriteLine("{0} {1}", (fst i), (snd i) ) )

        Assert.IsTrue (true)
    

    [<TestMethod>]
    member this.BenchGetSwitchCountForSorter10() =
        let reps = 10
        let rnd = new Random(2883)
        let order = 10
        let len = 700
        let GetCount (i:int) =
            let sorterDef = SorterDef.CreateRandom order rnd len
            let sorter = Sorter.MakeSorter Sortables.Sortable10.SwitchFuncForSwitch sorterDef
            Sortables.Sortable10.GetSwitchCountForSorter sorter Sortables.Sortable10.AllBinaryTestCases
        
        let counts = {1 .. reps} |> Seq.map(fun i -> GetCount i) 
                                 |> Seq.filter(fun res -> fst res)
                                 |> Seq.groupBy(fun res -> snd res)
                                 |> Seq.map(fun gps -> (fst gps, (snd gps) |> Seq.length ))
                                 |> Seq.toArray
                                 |> Array.sortBy(fun tup -> fst tup)

        counts |> Array.iter(fun i -> Console.WriteLine("{0} {1}", (fst i), (snd i) ) )

        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestSortableGen() =
        let order = 15
        Console.Write (SortableGen.GenN order)
        
        Assert.IsTrue (true)

