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
    member this.TestMakeRandomSorterDef() =
        let length = 29
        let order = 5

        let res = TestData.MakeRandomSorterDef order length 123

        Assert.IsTrue (res.order = order)
        Assert.IsTrue (res.switches.Length = length)



    [<TestMethod>]
    member this.TestMergeSwitchesIntoStages() =
        let order = 5

        let swrSet0 = [|
                        new Switch(1, 2):>ISwitch;
                        new Switch(1, 3):>ISwitch;
                        new Switch(2, 4):>ISwitch;
                        new Switch(0, 1):>ISwitch;
                        new Switch(0, 1):>ISwitch;
                        new Switch(2, 3):>ISwitch;
                        new Switch(3, 4):>ISwitch;
                        new Switch(1, 2):>ISwitch;
                        new Switch(0, 3):>ISwitch;
                     |] 

        let res0 = Stage.MergeSwitchesIntoStages order swrSet0 |> Seq.toArray
        let res1 = Stage.GetStageIndexesFromSwitches order swrSet0 |> Seq.toArray
  
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestStageResultToString() =
        let swu0 = {SwitchUsage.switch=new Switch(1, 2); switchIndex=0; useCount=99}
        let swu1 = {SwitchUsage.switch=new Switch(3, 4); switchIndex=1; useCount=98}
        let sr = {StageResult.switchUsages=[swu0; swu1]}
        let res = StageResult.ToString sr
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestMergeSwitchResultsIntoStageResults() =
        let order = 5
        let swrSet0 = [|
                        {SwitchUsage.switch=new Switch(1, 2); switchIndex=0; useCount=1};
                        {SwitchUsage.switch=new Switch(1, 3); switchIndex=1; useCount=1};
                        {SwitchUsage.switch=new Switch(2, 4); switchIndex=2; useCount=1};
                        {SwitchUsage.switch=new Switch(0, 1); switchIndex=3; useCount=1};
                        {SwitchUsage.switch=new Switch(0, 1); switchIndex=4; useCount=1};
                        {SwitchUsage.switch=new Switch(2, 3); switchIndex=5; useCount=1};
                        {SwitchUsage.switch=new Switch(3, 4); switchIndex=6; useCount=1};
                        {SwitchUsage.switch=new Switch(1, 2); switchIndex=7; useCount=1};
                        {SwitchUsage.switch=new Switch(0, 3); switchIndex=8; useCount=1};
                     |]
                    
        let res0 = Sorting.SorterResult.MergeSwitchUsagesIntoStageResults order swrSet0

        Assert.IsTrue (res0.Length = 6)
    

    [<TestMethod>]
    member this.TestToStagedSorterDef() =
        let stagedSorterDef = SortersFromData.RefSorter.CreateRefStagedSorter RefSorter.End16
 
        Assert.IsTrue (stagedSorterDef.stageIndexes.Length = 10)


    [<TestMethod>]
    member this.TestSortableGen() =
        let order = { 11 .. 11 } |> Seq.iter(fun i -> Console.Write (SortableGen.GenN i))
        Assert.IsTrue (true)

    [<TestMethod>]
    member this.TestGetStages() =
        let stagedSorter = RefSorter.CreateRefStagedSorter
                                   RefSorter.End16

        let res = StagedSorterDef.GetStages stagedSorter
                  |> Seq.map(fun sq-> sq|>Seq.toArray)
                  |> Seq.toArray

        Assert.IsTrue (true)
                                    

    [<TestMethod>]
    member this.TestStagedSorterDef_AppendSwitches() =
        let order = 10
        let prefixLength = 30
        let appendixLength = 30
        let seed = 1234

        let stagedSorter = 
                TestData.MakeRandomSorterDef order prefixLength seed
                |> Sorting.StagedSorterDef.ToStagedSorterDef
        
        let switchSequence = 
                TestData.SwitchSeq order (seed + 1)
                |> Seq.take appendixLength

        let appendedStagedSorter = 
                stagedSorter
                |> Sorting.StagedSorterDef.AppendSwitches switchSequence


        Assert.AreEqual(appendedStagedSorter.sorterDef.switches.Length,
                        prefixLength + appendixLength)
