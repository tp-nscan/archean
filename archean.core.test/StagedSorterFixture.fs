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

      let ExtractUsage (data: SwitchUsage[] option) =
          match data with
            | Some d -> d.Length
            | None -> 0

      let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order10
      let (success, data) = Sorter.Eval rfsst.sorterDef

      Assert.AreEqual(rfsst.sorterDef.switches.Length, ExtractUsage data)
      Assert.IsTrue(success)


    [<TestMethod>]
    member this.TestCondenseAllZeroOneSortables() =
    
        let ArrayToString(array:'a[]) =
            sprintf "[|%s|]" (array |> Seq.map(fun a -> string a) |> String.concat ";")

        let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order8Prefix3
        let res = Sorter.CondenseAllZeroOneSortables rfsst.sorterDef
        let rep = (snd res)
                  |> Seq.map(fun s-> ArrayToString s)
                  |> String.concat ";"
        
        Console.WriteLine rep          
        Assert.AreEqual(10, 10)


    [<TestMethod>]
    member this.TestStageWisePerf() =
        let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order24
        let res = StagedSorter.StageWisePerf0 rfsst
        Console.WriteLine (SwitchTracker.ToStageReportString (fst res) rfsst)
        Console.WriteLine ((snd res) |> List.map (string) |> (String.concat ", "))
        Assert.AreEqual(10, 10)

    [<TestMethod>]
    member this.TestStageWisePerf4() =
 
        let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order32
        let res = StagedSorter.StageWisePerf4 rfsst
        Console.WriteLine (SwitchTracker.ToStageReportString (fst res) rfsst)
        Console.WriteLine ((snd res) |> List.map (string) |> (String.concat ", "))

        Assert.AreEqual(10, 10)


    [<TestMethod>]
    member this.TestStageWisePerf8() =
 
        let rfsst = RefSorter.CreateRefStagedSorter RefSorter.Order32
        let res = StagedSorter.StageWisePerf8 rfsst
        Console.WriteLine (SwitchTracker.ToStageReportString (fst res) rfsst)
        Console.WriteLine ((snd res) |> List.map (string) |> (String.concat ", "))

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


        let randGenerationMode = SorterGenerationMode.Prefixed (rspst, rsst)

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
                        (SortableIntArray.SortableSeq testSortables)
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





