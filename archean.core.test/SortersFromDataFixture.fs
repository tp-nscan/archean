namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Sorting


[<TestClass>]
type SortersFromDataFixture () =

    [<TestMethod>]
    member this.TestParseSorterStringToStages() =
      let sd = SorterData.Order16_Green
               |> SortersFromData.RefSorter.ParseToStages 
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 10)


    [<TestMethod>]
    member this.TestParseSorterStringToNStagesOfSwitches() =
      let sd = SortersFromData.RefSorter.ParseToNStagesOfSwitches SorterData.Order16_Green 8
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 53)


    [<TestMethod>]
    member this.TestParseSorterStringToStagedSorter() =
      let stagedSorter = SortersFromData.RefSorter.ParseToStagedSorter
                                SorterData.Order16_Green 16
                         |> StagedSorter.TruncateStages 3
      Assert.AreEqual(stagedSorter.stageIndexes.Length , 3)
  
   
    [<TestMethod>]
    member this.TestParseSorterStringToSorter() =
      let sorter = SortersFromData.RefSorter.ParseToSorter
                                         SorterData.Order16_Green 16 3
      Assert.AreEqual(sorter.switches.Length, 24)
