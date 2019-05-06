namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.SortersFromData


[<TestClass>]
type SortersFromDataFixture () =

    [<TestMethod>]
    member this.TestParseSorterStringToStages() =
      let sd = SorterData.Order16_Green
               |> RefSorter.ParseToStages 
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 10)


    //[<TestMethod>]
    //member this.TestParseSorterStringToNStagesOfSwitches() =
    //  let sd = RefSorter.ParseToNStagesOfSwitches SorterData.Order16_Green 8
    //           |> Seq.toArray

    //  Assert.AreEqual(sd.Length, 53)


    [<TestMethod>]
    member this.TestRefSorter_ParseToStagedSorter() =
      let stagedSorter = RefSorter.ParseToStagedSorter
                                SorterData.Order16_Green 16
                         |> StagedSorter.TruncateStages 3
      Assert.AreEqual(stagedSorter.stageIndexes.Length , 3)
  
   
    [<TestMethod>]
    member this.TestRefSorter_CreateRefStagedSorter() =
      let refSorter = RefSorter.End16
      let (refSorterStr, order) = refSorter |> RefSorter.GetStringAndOrder

      let stagedSorter = RefSorter.CreateRefStagedSorter
                                         RefSorter.End16
      Assert.AreEqual(stagedSorter.sorterDef.order, order)
