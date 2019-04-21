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
               |> SortersFromData.ParseSorterStringToStages 
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 10)


    [<TestMethod>]
    member this.TestParseSorterStringToNStagesOfSwitches() =
      let sd = SortersFromData.ParseSorterStringToNStagesOfSwitches SorterData.Order16_Green 8
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 53)


    [<TestMethod>]
    member this.TestParseSorterStringToStagedSorter() =
      let stagedSorter = SortersFromData.ParseSorterStringToStagedSorter
                                SorterData.Order16_Green 16 3
      Assert.AreEqual(stagedSorter.stageIndexes.Length , 3)
  
   
    [<TestMethod>]
    member this.TestParseSorterStringToSorter() =
      let sorter = SortersFromData.ParseSorterStringToSorter
                                         SorterData.Order16_Green 16 3
      Assert.AreEqual(sorter.switches.Length, 24)
