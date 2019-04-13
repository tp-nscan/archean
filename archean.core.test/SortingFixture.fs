namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SorterA

[<TestClass>]
type SortingFixture () =
    let yak = true

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
        let switchCount = 16
        let rnd = new Random(123)

        let switchset =  (Stage.MakeStagePackedSwitchSeq rnd order)
                            |> Seq.take switchCount
                            |> Seq.toArray

        Assert.IsTrue (switchset.Length = switchCount)

    [<TestMethod>]
    member this.TestMakeSwitchSet() =
        let res = SwitchSet.ForOrder 5
        Assert.IsTrue (res.order = 5)
        Assert.IsTrue (res.switches.Length = 10)

    
    [<TestMethod>]
    member this.TestMakeRandomSorterDef() =
        let length = 29
        let order = 5
        
        let switchSet = SwitchSet.ForOrder order
        let rnd = new Random(123)
        let res = SorterDef.CreateRand switchSet length rnd

        Assert.IsTrue (res.order = order)
        Assert.IsTrue (res.switches.Length = length)


 

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

    [<TestMethod>]
    member this.TestGetFullSortingResultsForSortableIntArray() =
        let rnd = new Random(123)
        let order = 10
        let len = 160
        let sorterDef = SorterDef.CreateRandom order len rnd
        let res = SortingReports.GetFullSortingResultsUsingIntArray sorterDef
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestSortableIntArray_IsSorted() =
        Assert.IsFalse (Combinatorics.IsSorted [|0; 1; 1; 0; 1; 0|])
        Assert.IsTrue (Combinatorics.IsSorted [|0; 0; 0; 0; 1; 1|])


       [<TestMethod>]
       member this.TestMergeSwitchesIntoStages() =
        let order = 5
        let swrSet0 = [|
                        {Switch.low = 1; hi = 2};
                        {Switch.low = 1; hi = 3};
                        {Switch.low = 2; hi = 4};
                        {Switch.low = 0; hi = 1};
                        {Switch.low = 0; hi = 1};
                        {Switch.low = 2; hi = 3};
                        {Switch.low = 3; hi = 4};
                        {Switch.low = 1; hi = 2};
                        {Switch.low = 0; hi = 3};
                     |]

        let res0 = Stage.MergeSwitchesIntoStages order swrSet0 |> Seq.toArray
  
        Assert.IsTrue (true)


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
                    
        let res0 = Sorting.SorterResult.MergeSwitchResultsIntoStageResults order swrSet0

        Assert.IsTrue (res0.Length = 6)



    [<TestMethod>]
    member this.TestGetSorterSuccessRates() =
        
        let boko = seq { 0 .. 2 .. 8}
        let koko = seq { 1 .. 5}

        let dingo = boko |> Seq.append koko
                         |> Seq.toArray

        let rnd = new Random(123)

        let guko = SorterDef.Green6Switches |> Seq.toArray
        let guko2 = SorterDef.Green7Switches |> Seq.toArray

        let yuko = SorterDef.CreateGreen2 30 rnd
        let yuko2 = SorterDef.CreateGreen3 30 rnd

        // FullStage
        // order 6 : 120
        // order 7 : 180
        // order 8 : 260

        // LooseSwitches
        // order 6 : 120
        // order 7 : 180
        // order 8 : 260

        let sorterCount = 1
        
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
    member this.TestSortableGen() =
        let order = { 11 .. 11 } |> Seq.iter(fun i -> Console.Write (SortableGen.GenN i))
        
        
        Assert.IsTrue (true)

