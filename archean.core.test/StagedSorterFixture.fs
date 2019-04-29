namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Sorting
open archean.core.SortersFromData


[<TestClass>]
type StagedSorterFixture () =


    [<TestMethod>]
    member this.TestParseSorterStringToStages() =
      let sd = SorterData.Order16_Green
               |> SortersFromData.ParseSorterStringToStages 
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 10)


    [<TestMethod>]
    member this.TestRandGenerationMode() =
      let ref = RefSorter.End16
      let rsst = {RandSorterStages.order=16; stageCount=100; randSwitchFill=RandSwitchFill.FullStage }
      let rspst = {RefSorterPrefixStages.refSorter=ref; stageCount = 4; }
      let randGenerationMode = RandGenerationMode.Prefixed (rspst, rsst)
      let sorterCount = 10
      let seed = 1234


      let prefixedSorter = CreatePrefixedSorter randGenerationMode
      let testSortables = (SortableTestCases (randGenerationMode |> RemoveLastRefStage)) |> Seq.toArray


      Assert.AreEqual(10, 10)





