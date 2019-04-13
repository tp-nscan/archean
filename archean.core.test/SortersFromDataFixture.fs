namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core


[<TestClass>]
type SortersFromDataFixture () =

    [<TestMethod>]
    member this.TestParseSorterData() =
      let sd = SortersFromData.ParseSorterDataToArrayOfStages
      Assert.AreEqual(1, 1)
