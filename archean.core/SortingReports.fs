namespace archean.core
open System
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SortersFromData
open SorterB

module SortingReports =
  
    let SortableFuncAllBinary (order:int) () =
            IntBits.AllBinaryTestCases order

    let SortableFuncAllBinary2 (order:int) () =
            IntBits.AllBinaryTestCases order
            |> Seq.map(fun i -> (i, 1))

    let MakeSwitchTracker (totalSwitches: int) (prefixSwitches: int) =
        Array.init totalSwitches (fun i -> if (i<prefixSwitches) then 1 else 0)

    let SortableTestCases (randGenerationMode:RandGenerationMode) =
        let prefixSorter = CreatePrefixedSorter randGenerationMode
        let switchTracker = Array.init prefixSorter.switches.Length (fun i -> 0)
        let (_, sortableRes) = RunSwitchesAndGetResults switchTracker 
                                   prefixSorter (SortableFuncAllBinary prefixSorter.order)
        sortableRes |> Set.toSeq

    let SortableTestCases2 (randGenerationMode:RandGenerationMode) =
        let prefixSorter = CreatePrefixedSorter randGenerationMode
        let switchTracker = Array.init prefixSorter.switches.Length (fun i -> 0)
        let (_, sortableRes) = RunWeightedSwitchesAndGetWeightedResults switchTracker 
                                   prefixSorter (SortableFuncAllBinary2 prefixSorter.order)
        sortableRes |> Array.toSeq

    // returns (NumStagesUsed, NumSwitchesUsed, Count)  
    //let MakeStageAndSwitchUseHistogram (sorterLen: int) 
    //                                   (randGenerationMode : RandGenerationMode)
    //                                   (sorterCount:int) (seed : int) =

    //    let prefixedSorter = SortersFromData.CreatePrefixedSorter randGenerationMode
    //    let prefixedSorterLength = prefixedSorter.switches.Length

    //    let testSortables = (SortableTestCases2 randGenerationMode) |> Seq.toArray
    //    let TestSortablesSeq () =
    //        testSortables
    //            |> Array.map(fun a -> (Array.copy (fst a), snd a))
    //            |> Array.toSeq

    //    let MakeSorterResults (sorterDef:SorterDef) (startPos:int) =
    //        let res = SorterB.GetSwitchResultsForSorterAndCheckResults2 
    //                            (MakeSwitchTracker sorterLen prefixedSorterLength)
    //                            sorterDef
    //                            startPos
    //                            TestSortablesSeq
    //        (res, sorterDef)

    //    let MakeHistoLine (tt:int[]) = 
    //        sprintf "%d\t%A\t%d\t%d\t%d"
    //                prefixedSorter.order randGenerationMode tt.[0] tt.[1] tt.[2]

    //    let rnd = new Random(seed)
    //    let histogram =
    //        {1 .. sorterCount}
    //        |> Seq.map(fun i -> SortersFromData.CreateRandomSorterDef sorterLen randGenerationMode rnd)
    //        |> Seq.toArray
    //        |> Array.Parallel.map(fun sorterDef -> MakeSorterResults sorterDef prefixedSorterLength)
    //        |> Seq.filter(fun ((success, _ ), _ ) -> success)
    //        |> Seq.map(fun (( _ , switchResults), sorterDef) -> Sorting.SorterResult.MakeSorterResult sorterDef switchResults)
    //        |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
    //        |> Seq.map(fun ((success, switchResults), sorterResults) -> [| success; switchResults; sorterResults|> Seq.length |])
    //        |> Seq.toArray

    //    let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%A seed:%d sorterCount:%d resultCount:%d" 
    //                            prefixedSorter.order sorterLen randGenerationMode seed sorterCount (histogram|>Array.sumBy(fun a -> a.[2]))
    //    (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))


     //returns (NumStagesUsed, NumSwitchesUsed, Count)  
    let MakeStageAndSwitchUseHistogram (sorterLen: int) 
                                       (randGenerationMode : RandGenerationMode)
                                       (sorterCount:int) (seed : int) =

        let prefixedSorter = SortersFromData.CreatePrefixedSorter randGenerationMode
        let prefixedSorterLength = prefixedSorter.switches.Length

        let testSortables = (SortableTestCases randGenerationMode) |> Seq.toArray
        let TestSortablesSeq () =
            testSortables
                |> Array.map(fun a -> Array.copy a)
                |> Array.toSeq

        let MakeSorterResults (sorterDef:SorterDef) (startPos:int) =
            let res = SorterB.GetSwitchResultsForGoodSorters 
                                (MakeSwitchTracker sorterLen prefixedSorterLength)
                                sorterDef
                                startPos
                                TestSortablesSeq
            (res, sorterDef)

        let MakeHistoLine (tt:int[]) = 
            sprintf "%d\t%A\t%d\t%d\t%d"
                    prefixedSorter.order randGenerationMode tt.[0] tt.[1] tt.[2]

        let rnd = new Random(seed)
        let histogram =
            {1 .. sorterCount}
            |> Seq.map(fun i -> SortersFromData.CreateRandomSorterDef sorterLen randGenerationMode rnd)
            |> Seq.toArray
            |> Array.Parallel.map(fun sorterDef -> MakeSorterResults sorterDef prefixedSorterLength)
            |> Seq.filter(fun ((success, _ ), _ ) -> success)
            |> Seq.map(fun (( _ , switchResults), sorterDef) -> Sorting.SorterResult.MakeSorterResult sorterDef switchResults.Value)
            |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
            |> Seq.map(fun ((success, switchResults), sorterResults) -> [| success; switchResults; sorterResults|> Seq.length |])
            |> Seq.toArray

        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%A seed:%d sorterCount:%d resultCount:%d" 
                                prefixedSorter.order sorterLen randGenerationMode seed sorterCount (histogram|>Array.sumBy(fun a -> a.[2]))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))