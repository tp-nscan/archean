namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SortersFromData

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

        let res = TestData.MakeRandomSorter order length 123

        Assert.IsTrue (res.order = order)
        Assert.IsTrue (res.switches.Length = length)

    
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
        let res1 = Stage.GetStageIndexesFromSwitches order swrSet0 |> Seq.toArray
  
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestStageResultToString() =
        let swu0 = {SwitchUsage.switch={Switch.low=1; hi=2}; switchIndex=0; useCount=99}
        let swu1 = {SwitchUsage.switch={Switch.low=3; hi=4}; switchIndex=1; useCount=98}
        let sr = {StageResult.switchUsages=[swu0; swu1]}
        let res = StageResult.ToString sr
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestMergeSwitchResultsIntoStageResults() =
        let order = 5
        let swrSet0 = [|
                        {SwitchUsage.switch={Switch.low = 1; hi = 2}; switchIndex=0; useCount=1};
                        {SwitchUsage.switch={Switch.low = 1; hi = 3}; switchIndex=1; useCount=1};
                        {SwitchUsage.switch={Switch.low = 2; hi = 4}; switchIndex=2; useCount=1};
                        {SwitchUsage.switch={Switch.low = 0; hi = 1}; switchIndex=3; useCount=1};
                        {SwitchUsage.switch={Switch.low = 0; hi = 1}; switchIndex=4; useCount=1};
                        {SwitchUsage.switch={Switch.low = 2; hi = 3}; switchIndex=5; useCount=1};
                        {SwitchUsage.switch={Switch.low = 3; hi = 4}; switchIndex=6; useCount=1};
                        {SwitchUsage.switch={Switch.low = 1; hi = 2}; switchIndex=7; useCount=1};
                        {SwitchUsage.switch={Switch.low = 0; hi = 3}; switchIndex=8; useCount=1};
                     |]
                    
        let res0 = Sorting.SorterResult.MergeSwitchUsagesIntoStageResults order swrSet0

        Assert.IsTrue (res0.Length = 6)
    

    [<TestMethod>]
    member this.TestToStagedSorterDef() =
        let sorterDef = SortersFromData.CreateRefSorter RefSorter.End16 9
            
        let stagedSorterDef = StagedSorterDef.ToStagedSorterDef sorterDef 

        Assert.IsTrue (stagedSorterDef.stageIndexes.Length = 10)


    [<TestMethod>]
    member this.TestSortableGen() =
        let order = { 11 .. 11 } |> Seq.iter(fun i -> Console.Write (SortableGen.GenN i))
        Assert.IsTrue (true)


    [<TestMethod>]
    member this.TestStagedSorterDef_AppendSwitches() =
        let order = 10
        let prefixLength = 30
        let appendixLength = 30
        let seed = 1234

        let stagedSorter = 
                TestData.MakeRandomSorter order prefixLength seed
                |> Sorting.StagedSorterDef.ToStagedSorterDef
        
        let switchSequence = 
                TestData.SwitchSeq order (seed + 1)
                |> Seq.take appendixLength

        let appendedStagedSorter = 
                stagedSorter
                |> Sorting.StagedSorterDef.AppendSwitches switchSequence


        Assert.AreEqual(appendedStagedSorter.sorterDef.switches.Length,
                        prefixLength + appendixLength)
