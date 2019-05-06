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
               |> SortersFromData.RefSorter.ParseToStages 
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 10)


    [<TestMethod>]
    member this.TestTruncateStages() =
      let remainingStageCount = 7
      let stgs = TestData.StagedSorterDef
      let trunked = stgs |> StagedSorter.TruncateStages remainingStageCount
      Assert.AreEqual(trunked.stageIndexes.Length, remainingStageCount)
      Assert.AreEqual(trunked.sorterDef.switches.Length, stgs.stageIndexes.[remainingStageCount])


    [<TestMethod>]
    member this.ParseSorterStringToStagedSorter() =
      let sd = SorterData.Order16_Green
               |> SortersFromData.RefSorter.ParseToStages 
               |> Seq.toArray

      Assert.AreEqual(sd.Length, 10)

    [<TestMethod>]
    member this.EvalRefStagedSorter() =
      let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order24
      let res = Sorter.Eval rfsst.sorterDef

      Assert.AreEqual(10, 10)

    [<TestMethod>]
    member this.StageWiseEvalRefStagedSorter() =
      let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order22
      let res = StagedSorter.GetStageWiseWeightedPerf rfsst
      let rep0 = (fst res).weights  |> Seq.map(fun a -> string a) |> String.concat ", "
      let rep1 = (snd res) |> Seq.map(fun a -> string a) |> String.concat ", "
      Console.WriteLine rep0
      Console.WriteLine rep1
      Assert.AreEqual(10, 10)

    [<TestMethod>]
    member this.StageWiseEvalRefStagedSorter0() =
        let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order22
        let res = StagedSorter.GetStageWisePerf rfsst
        let rep0 = (fst res).weights  |> Seq.map(fun a -> string a) |> String.concat ", "
        let rep1 = (snd res) |> Seq.map(fun a -> string a) |> String.concat ", "
        Console.WriteLine rep0
        Console.WriteLine rep1
        Assert.AreEqual(10, 10)



    [<TestMethod>]
    member this.TestRandGenerationMode() =
        let sorterCount = 15
        let seed = 1234
        let rnd = new Random(seed)
        let refSorter = RefSorter.Order10
        let stageToTest = 3

        let (refStr, order) = SortersFromData.RefSorter.GetStringAndOrder refSorter
        let rsst = {RandSorterStages.order=order; stageCount=order*16; randSwitchFill=RandSwitchFill.FullStage }
        let rspst = {RefSorterPrefixStages.refSorter=refSorter; stageCount = stageToTest; }


        let randGenerationMode = RandGenerationMode.Prefixed (rspst, rsst)

        let rgmForFiltering = (randGenerationMode |> RemoveRandomStages)
        let filteringSorter = CreatePrefixedSorter rgmForFiltering
        let filteringSorterLength = filteringSorter.switches.Length
        
        let completeSorterLength = SortersFromData.GetSorterSwitchCount randGenerationMode
        let testSortables = (SortableTestCases rgmForFiltering) 
                                |> Seq.toArray

        let MakeStagedSorterResults (stagedSorterDef:StagedSorterDef) =
            let res = StagedSorter.GetStagePerfAndSwitchUsage 
                        (SwitchTracker.MakePrefixed completeSorterLength filteringSorterLength)
                        stagedSorterDef
                        ((randGenerationMode |> To_PrefixStageCount) - 1)
                        (SortableIntArray.WeightedSortableSeq testSortables)
            (res, stagedSorterDef)

        let testSorters =
            {1 .. sorterCount}
            |> Seq.map(fun i -> (CreateRandomStagedSorterDef randGenerationMode rnd))
            |> Seq.toArray


        let sorterResFancyA =
            testSorters
                |> Array.map(fun stagedSorterDef -> MakeStagedSorterResults stagedSorterDef)
        let sorterResFancy =
            sorterResFancyA
                |> Array.map(fun res -> snd(snd(fst res).Value))


        let sorterResPlain =
            testSorters
                |> Array.map(fun stagedSorterDef -> Sorter.Eval stagedSorterDef.sorterDef)
                |> Array.map(fun res -> (snd res).Value)

        Assert.AreEqual(10, 10)





