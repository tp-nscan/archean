
namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.ComboCounter
open System.Windows.Media



[<TestClass>]
type ColorSetsFixture () =

    [<TestMethod>]
    member this.TestRedBlueSpan() =
      let len = 16

      let res = ColorSets.TwoColorSpan Colors.Red Colors.Blue len
      Assert.AreEqual(res.Length, len)