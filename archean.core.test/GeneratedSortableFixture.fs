namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SorterA


[<TestClass>]
type GeneratedSortableFixture () =
    let yak = true


    
    //[<TestMethod>]
    //member this.TestSortable6() =
    //    let length = 29
    //    let order = 6
        
    //    let rnd = new Random(123)
    //    let s4 = GeneratedSortables.Sortable6.CreateRandom rnd
    //    let sI = GeneratedSortables.Sortable6.Identity
    //    let iO = GeneratedSortables.Sortable6.IsSorted s4
    //    let i1 = GeneratedSortables.Sortable6.IsSorted sI
    //    let a4 = GeneratedSortables.Sortable6.ToArray s4
    //    let sw4 = GeneratedSortables.Sortable6.SwitchFuncs.[0] s4
    //    let b4 = GeneratedSortables.Sortable6.ToArray (sw4 |> fst)

    //    Assert.IsTrue (a4.Length = order)
    //    Assert.IsTrue (b4.Length = order)

    //    Assert.IsTrue (true)
 
 
    //[<TestMethod>]
    //member this.TestSortable6AllBinaryTestCases() =
    //    let cases = GeneratedSortables.Sortable6.AllBinaryTestCases |> Seq.toArray

    //    Assert.IsTrue (cases.Length = 64)
    //    Assert.IsTrue (true)


    //[<TestMethod>]
    //member this.TestMakeSorterForSortable6() =
    //    let rnd = new Random(123)
    //    let order = 6
    //    let len = 160
    //    let sorterDef = SorterDef.CreateRandom order len rnd
    //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
    //    let res = Sorter.Sort sorter (GeneratedSortables.Sortable6.CreateRandom rnd)
    //    Assert.IsTrue (GeneratedSortables.Sortable6.IsSorted (snd res))

        
    //[<TestMethod>]
    //member this.TestCountSwitchUse() =
    //    let rnd = new Random(123)
    //    let order = 6
    //    let len = 160
    //    let sorterDef = SorterDef.CreateRandom order len rnd
    //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
    //    let res = Sorter.Sort sorter (GeneratedSortables.Sortable6.CreateRandom rnd)
    //    Assert.IsTrue (GeneratedSortables.Sortable6.IsSorted (snd res))

    //[<TestMethod>]
    //member this.TestSortOneAndTrackSwitches() =
    //    let rnd = new Random(123)
    //    let order = 6
    //    let len = 36
    //    let switchTracker = Array.init len (fun i -> 0)
    //    let sorterDef = SorterDef.CreateRandom order len rnd
    //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
    //    let sortable = GeneratedSortables.Sortable6.FromArray [|1; 0; 1; 0; 1; 0|]
    //    let res = Sorter.SortOneAndTrackSwitches sorter switchTracker sortable
    //    Assert.IsTrue (true)


    //[<TestMethod>]
    //member this.TestSortManyAndTrackSwitches() =
    //    let sortable0 = GeneratedSortables.Sortable6.FromArray [|0; 1; 1; 0; 1; 0|]
    //    let sortable1 = GeneratedSortables.Sortable6.FromArray [|1; 0; 1; 0; 1; 1|]
    //    let sseq = seq {yield sortable0; yield sortable1}
    //    let checker t = GeneratedSortables.Sortable6.IsSorted t

    //    let rnd = new Random(123)
    //    let order = 6
    //    let len = 66
    //    let switchTracker = Array.init len (fun i -> 0)
    //    let sorterDef = SorterDef.CreateRandom order len rnd
    //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
    //    let res = Sorter.SortManyTrackSwitchesAndCheckResults checker sorter switchTracker sseq
    //    Assert.IsTrue (fst res)


    //[<TestMethod>]
    //member this.TestGetSwitchCountForSorter() =
    //    let rnd = new Random(123)
    //    let order = 6
    //    let len = 360
    //    let sorterDef = SorterDef.CreateRandom order len rnd
    //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
    //    let res = GeneratedSortables.Sortable6.GetSwitchCountForSorter sorter
    //    Assert.IsTrue (true)



    //[<TestMethod>]
    //member this.TestGetSorterResultForSorter7() =
    //    let reps = 10
    //    let rnd = new Random(12883)
    //    let order = 7
    //    let len = 150
    //    let GetSorterWithResults (i:int) =
    //        let sorterDef = SorterDef.CreateRandomPackedStages order len rnd
    //        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable7.SwitchFuncForSwitch sorterDef
    //        (GeneratedSortables.Sortable7.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable7.AllBinaryTestCases, sorterDef)
        
    //    let counts = {1 .. reps} |> Seq.map(fun i -> GetSorterWithResults i)
    //                             |> Seq.filter(fun res -> fst (fst res))
    //                             |> Seq.map(fun i -> Sorting.SorterResult.MakeSorterResult (snd i) (snd (fst i)) )
    //                             |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
    //                             |> Seq.toArray

    //    counts |> Array.iter(fun i -> Console.WriteLine("{0} {1} {2}", (fst (fst i)), (snd (fst i)), (snd i)|> Seq.length ))

    //    Assert.IsTrue (true)


    //[<TestMethod>]
    //member this.TestGetSorterResultForSorter8() =
    //    let reps = 1
    //    let rnd = new Random(32883)
    //    let order = 8
    //    let len = 220
    //    let GetSorterWithResults (i:int) =
    //        let sorterDef = SorterDef.CreateRandomPackedStages order len rnd
    //        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable8.SwitchFuncForSwitch sorterDef
    //        (GeneratedSortables.Sortable8.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable8.AllBinaryTestCases, sorterDef)
        
    //    let counts = {1 .. reps} |> Seq.map(fun i -> GetSorterWithResults i)
    //                             |> Seq.filter(fun res -> fst (fst res))
    //                             |> Seq.map(fun i -> Sorting.SorterResult.MakeSorterResult (snd i) (snd (fst i)) )
    //                             |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
    //                             |> Seq.toArray

    //    counts |> Array.iter(fun i -> Console.WriteLine("{0} {1} {2}", (fst (fst i)), (snd (fst i)), (snd i)|> Seq.length ))

    //    Assert.IsTrue (true)


    //[<TestMethod>]
    //member this.BenchGetSwitchCountForSorter10() =
    //    let reps = 10
    //    let rnd = new Random(2883)
    //    let order = 6
    //    let len = 700
    //    let GetCount (i:int) =
    //        let sorterDef = SorterDef.CreateRandom order len rnd
    //        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
    //        GeneratedSortables.Sortable6.GetSwitchCountForSorter sorter GeneratedSortables.Sortable6.AllBinaryTestCases
        
    //    let counts = {1 .. reps} |> Seq.map(fun i -> GetCount i) 
    //                             |> Seq.filter(fun res -> fst res)
    //                             |> Seq.groupBy(fun res -> snd res)
    //                             |> Seq.map(fun gps -> (fst gps, (snd gps) |> Seq.length ))
    //                             |> Seq.toArray
    //                             |> Array.sortBy(fun tup -> fst tup)

    //    counts |> Array.iter(fun i -> Console.WriteLine("{0} {1}", (fst i), (snd i) ) )

    //    Assert.IsTrue (true)
