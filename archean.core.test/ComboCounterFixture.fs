namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.ComboCounter



[<TestClass>]
type ComboCounterFixture () =

    [<TestMethod>]
    member this.TestMakeWheelSet() =
      let spokes = [|[|0;0|];[|0;1|];[|1;1|];|]

      let res = ComboCounter.MakeUniformWheelSet spokes 3
      Assert.AreEqual(res.Length, 3)


    [<TestMethod>]
    member this.TestIncrementWheelSet() =
      let spokes = [|[|0;0|];[|0;1|];[|1;1|];|]

      let first = ComboCounter.MakeUniformWheelSet spokes 3
      let next = ComboCounter.Increment first
      let next1 = ComboCounter.Increment next
      let next2 = ComboCounter.Increment next1
      let next3 = ComboCounter.Increment next2
      let next4 = ComboCounter.Increment next3
      let next5 = ComboCounter.Increment next4
      let next6 = ComboCounter.Increment next5
      let next7 = ComboCounter.Increment next6
      Assert.AreEqual(first.Length, 3)


    [<TestMethod>]
    member this.TestMakeMakeZeroOneArray() =
      let spokes = [|[|0;0|];[|0;1|];[|1;1|];|]
      let wheelset = ComboCounter.MakeUniformWheelSet spokes 3

      let genArrays (wheelsetIn:Wheel[]) = 
        let makeArray = 
            (MakeZeroOneArray wheelset)
        let mutable wheelset = wheelsetIn
        seq {   
                while true do
                    yield (makeArray wheelset)
                    wheelset <- ComboCounter.Increment wheelset
             }

      let sortables = ComboCounter.MakeAllArrays wheelset
                        |> Seq.toArray
      Assert.IsTrue(true)


     [<TestMethod>]
     member this.TestFiltered2Stage4BlockArrays() =

       let sortables = ComboCounter.Filtered2Stage4BlockArrays 8
                          |> Seq.toArray

       Assert.IsTrue(true)
       
       
     [<TestMethod>]
     member this.TestInitGroupAssigns() =
        let max = 19
        let start = 2
        let res = GraphOps.InitGroupAssigns {start .. max}
                    |> Seq.toArray

        Assert.AreEqual(res.Length, max-start + 1)


     [<TestMethod>]
     member this.TestGetGroupFromItem() =
         let max = 19
         let start = 2
         let item = 6
         let res = GraphOps.InitGroupAssigns {start .. max}
                     |> Seq.toArray

         let g = GraphOps.GetGroupFromItem res item
         Assert.AreEqual(item, g + start)


     [<TestMethod>]
     member this.TestMergeGroups() =
        let max = 19
        let start = 2
        let item = 6
        let groupAssigns = 
            GraphOps.InitGroupAssigns {start .. max}
                    |> Seq.toArray

        let mg = GraphOps.MergeGroups groupAssigns item 2
        let mg = GraphOps.MergeGroups groupAssigns 3 2
        let mg = GraphOps.MergeGroups groupAssigns 5 7
        let mg = GraphOps.MergeGroups groupAssigns 8 12
        let mg = GraphOps.MergeGroups groupAssigns 8 7

        let partitions = mg
                         |> Seq.groupBy(fun ga-> ga.group)
                         |> Seq.map(fun (key, vals) -> vals |> Seq.toArray)
                         |> Seq.toArray

        Assert.AreEqual(1, 1)