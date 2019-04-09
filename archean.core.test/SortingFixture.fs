namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SortingReports

[<TestClass>]
type SortingFixture () =


    [<TestMethod>]
    member this.TestSwitchFromPermutation() =
        let order = 12
        let rnd = new Random(123)
        let perm = Permutation.CreateRandom rnd order |> Seq.item 0
        let switchset = Switch.SwitchSeqFromPermutation perm |> Seq.toArray
        Assert.IsTrue (switchset.Length < order / 2)

        
    [<TestMethod>]
    member this.TestMakeStagePackedSwitchSeq() =
        let order = 8
        let rnd = new Random(123)
        //let polyCycle = TwoCycleIntArray.MakeRandomPolyCycle rnd order 1 |> Seq.item 0
        //let switchset = Switch.SwitchSeqFromPolyCycle polyCycle |> Seq.toArray

        let switchset =  (SwitchSet.MakeStagePackedSwitchSeq rnd order)
                            |> Seq.take 16
                            |> Seq.toArray

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
        let res = SorterDef.CreateRand switchSet length rnd

        Assert.IsTrue (res.order = order)
        Assert.IsTrue (res.switches.Length = length)


    [<TestMethod>]
    member this.TestSortable6() =
        let length = 29
        let order = 6
        
        let rnd = new Random(123)
        let s4 = GeneratedSortables.Sortable6.CreateRandom rnd
        let sI = GeneratedSortables.Sortable6.Identity
        let iO = GeneratedSortables.Sortable6.IsSorted s4
        let i1 = GeneratedSortables.Sortable6.IsSorted sI
        let a4 = GeneratedSortables.Sortable6.ToArray s4
        let sw4 = GeneratedSortables.Sortable6.SwitchFuncs.[0] s4
        let b4 = GeneratedSortables.Sortable6.ToArray (sw4 |> fst)

        Assert.IsTrue (a4.Length = order)
        Assert.IsTrue (b4.Length = order)

        Assert.IsTrue (true)
 
 
    [<TestMethod>]
    member this.TestSortable6AllBinaryTestCases() =
        let cases = GeneratedSortables.Sortable6.AllBinaryTestCases |> Seq.toArray

        Assert.IsTrue (cases.Length = 64)
        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestMakeSorterForSortable6() =
        let rnd = new Random(123)
        let order = 6
        let len = 160
        let sorterDef = SorterDef.CreateRandom order len rnd
        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
        let res = Sorter.Sort sorter (GeneratedSortables.Sortable6.CreateRandom rnd)
        Assert.IsTrue (GeneratedSortables.Sortable6.IsSorted (snd res))
 

    [<TestMethod>]
    member this.TestMakeSorterForSortableIntArray() =
        let rnd = new Random(123)
        let order = 6
        let len = 160
        let sorterDef = SorterDef.CreateRandom order len rnd
        let sorter = Sorter.MakeSorter SortableIntArray.SwitchFuncForSwitch sorterDef
        let sortable = (SortableIntArray.CreateRandom order rnd) |> Seq.item 0
        let res = Sorter.Sort sorter sortable.values
        Assert.IsTrue (SortableIntArray.IsSorted (snd res))

    [<TestMethod>]
    member this.TestGetFullSortingResultsForSortableIntArray() =
        let rnd = new Random(123)
        let order = 20
        let len = 1600
        let sorterDef = SorterDef.CreateRandom order len rnd
        let res = SortingReports.GetFullSortingResultsUsingIntArray sorterDef
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestSortableIntArray_IsSorted() =
        Assert.IsFalse (SortableIntArray.IsSorted [|0; 1; 1; 0; 1; 0|])
        Assert.IsTrue (SortableIntArray.IsSorted [|0; 0; 0; 0; 1; 1|])

    [<TestMethod>]
    member this.TestCountSwitchUse() =
        let rnd = new Random(123)
        let order = 6
        let len = 160
        let sorterDef = SorterDef.CreateRandom order len rnd
        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
        let res = Sorter.Sort sorter (GeneratedSortables.Sortable6.CreateRandom rnd)
        Assert.IsTrue (GeneratedSortables.Sortable6.IsSorted (snd res))


    [<TestMethod>]
    member this.TestSortOneAndTrackSwitches() =
        let rnd = new Random(123)
        let order = 6
        let len = 36
        let switchTracker = Array.init len (fun i -> 0)
        let sorterDef = SorterDef.CreateRandom order len rnd
        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
        let sortable = GeneratedSortables.Sortable6.FromArray [|1; 0; 1; 0; 1; 0|]
        let res = Sorter.SortOneAndTrackSwitches sorter switchTracker sortable
        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestSortManyAndTrackSwitches() =
        let sortable0 = GeneratedSortables.Sortable6.FromArray [|0; 1; 1; 0; 1; 0|]
        let sortable1 = GeneratedSortables.Sortable6.FromArray [|1; 0; 1; 0; 1; 1|]
        let sseq = seq {yield sortable0; yield sortable1}
        let checker t = GeneratedSortables.Sortable6.IsSorted t

        let rnd = new Random(123)
        let order = 6
        let len = 66
        let switchTracker = Array.init len (fun i -> 0)
        let sorterDef = SorterDef.CreateRandom order len rnd
        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
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
        let order = 6
        let len = 360
        let sorterDef = SorterDef.CreateRandom order len rnd
        let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
        let res = GeneratedSortables.Sortable6.GetSwitchCountForSorter sorter
        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestGetSorterSuccessRates() =

        // FullStage
        // order 6 : 120
        // order 7 : 180
        // order 8 : 260

        // LooseSwitches
        // order 6 : 120
        // order 7 : 180
        // order 8 : 260

        let sorterCount = 100
        
        let order = 11
        let sorterLen = 100
        let seed = 100
        let mode = Sorting.SorterDef.RandGenerationMode.FullStage

        let res = seq {260 .. 10 .. 460} 
                        |> Seq.map(fun i -> SortingReports.MakeStageAndSwitchUseHistogram 
                                                order i mode SortingReports.SwitchableType.Generated sorterCount seed)
                        |> Seq.toArray

        Assert.IsTrue (true)
    
    
    [<TestMethod>]
    member this.TestGetSorterResultForSorter7() =
        let reps = 10
        let rnd = new Random(12883)
        let order = 7
        let len = 150
        let GetSorterWithResults (i:int) =
            let sorterDef = SorterDef.CreateRandomPackedStages order len rnd
            let sorter = Sorter.MakeSorter GeneratedSortables.Sortable7.SwitchFuncForSwitch sorterDef
            (GeneratedSortables.Sortable7.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable7.AllBinaryTestCases, sorterDef)
        
        let counts = {1 .. reps} |> Seq.map(fun i -> GetSorterWithResults i)
                                 |> Seq.filter(fun res -> fst (fst res))
                                 |> Seq.map(fun i -> Sorting.SorterResult.MakeSorterResult (snd i) (snd (fst i)) )
                                 |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
                                 |> Seq.toArray

        counts |> Array.iter(fun i -> Console.WriteLine("{0} {1} {2}", (fst (fst i)), (snd (fst i)), (snd i)|> Seq.length ))

        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestGetSorterResultForSorter8() =
        let reps = 1000000
        let rnd = new Random(32883)
        let order = 8
        let len = 220
        let GetSorterWithResults (i:int) =
            let sorterDef = SorterDef.CreateRandomPackedStages order len rnd
            let sorter = Sorter.MakeSorter GeneratedSortables.Sortable8.SwitchFuncForSwitch sorterDef
            (GeneratedSortables.Sortable8.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable8.AllBinaryTestCases, sorterDef)
        
        let counts = {1 .. reps} |> Seq.map(fun i -> GetSorterWithResults i)
                                 |> Seq.filter(fun res -> fst (fst res))
                                 |> Seq.map(fun i -> Sorting.SorterResult.MakeSorterResult (snd i) (snd (fst i)) )
                                 |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
                                 |> Seq.toArray

        counts |> Array.iter(fun i -> Console.WriteLine("{0} {1} {2}", (fst (fst i)), (snd (fst i)), (snd i)|> Seq.length ))

        Assert.IsTrue (true)


    [<TestMethod>]
    member this.BenchGetSwitchCountForSorter10() =
        let reps = 10
        let rnd = new Random(2883)
        let order = 6
        let len = 700
        let GetCount (i:int) =
            let sorterDef = SorterDef.CreateRandom order len rnd
            let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
            GeneratedSortables.Sortable6.GetSwitchCountForSorter sorter GeneratedSortables.Sortable6.AllBinaryTestCases
        
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
        let order = { 11 .. 11 } |> Seq.iter(fun i -> Console.Write (SortableGen.GenN i))
        
        
        Assert.IsTrue (true)

