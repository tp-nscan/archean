namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core.Sorting
open archean.core.SortingReports
open archean.core.SortersFromData

[<TestClass>]
type SortingReportsFixture () =

    [<TestMethod>]
    member this.TestStageLayout_AddSwitch() =
      let order = 16
      let sl0 = StageLayout.InitSwitchPad order
      let sw1 = {Switch.low = 3; Switch.hi = 4}
      let sw2 = {Switch.low = 5; Switch.hi = 7}
      let sl1 = StageLayout.AddSwitch sl0 sw1
      let sl2 = StageLayout.AddSwitch sl1 sw2
      Assert.AreEqual(10, 10)


    [<TestMethod>]
    member this.TestStageLayout_SwitchFits() =
      let order = 16
      let sl0 = StageLayout.InitSwitchPad order
      let sw1 = {Switch.low = 3; Switch.hi = 7}
      let sw2 = {Switch.low = 0; Switch.hi = 2}
      let sw3 = {Switch.low = 7; Switch.hi = 12}
      let sl1 = StageLayout.AddSwitch sl0 sw1
      let fit1 = StageLayout.SwitchFits sl1 sw2
      let fit2 = StageLayout.SwitchFits sl1 sw3
      Assert.AreEqual(10, 10)


    [<TestMethod>]
    member this.TestStageLayout_FitStage() =
         let order = 16
         let switchCount = 120
         let seed = 123;
         let rnd = new Random(seed)
         let switchSeq = Switch.RandomSwitchesOfOrder 16 rnd
                         |> Seq.take switchCount

         let racked = switchSeq |> Seq.toArray


         let res = StageLayout.LayoutSwitches order switchSeq
                   |> Array.maxBy(fun l->l.Length)

         Assert.AreEqual(10, 10)


    [<TestMethod>]
    member this.TestStageLayout_LayoutStagedSorter() =
        let stagedSorter = RefSorter.CreateRefStagedSorter
                                           RefSorter.End16

        let dsf = StagedSorterDef.GetStages stagedSorter
        let res = StageLayout.LayoutStagedSorter stagedSorter


        Assert.AreEqual(10, 10)