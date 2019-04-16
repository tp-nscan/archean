namespace archean.core
open System
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SortersFromData
open Sorting.SorterDef
open SorterA

module SortingReports =
  
    let SortableFuncAllBinary (order:int) () =
            IntBits.AllBinaryTestCases order

    let GetFullSortingResultsUsingIntArray (sorterDef:SorterDef) =
        let sorter = Sorter.MakeSorter SortableIntArray.SwitchFuncForSwitch sorterDef
        (Sorter.GetSwitchResultsForSorter sorter (IntBits.AllBinaryTestCases sorterDef.order), sorterDef)


    let GetFullSortingResultsUsingIntArray2 (sorterDef:SorterDef) =
        let switchTracker = Array.init sorterDef.switches.Length (fun i -> 0)
        let res = SorterB.GetSwitchResultsForSorterAndCheckResults switchTracker 
                        sorterDef (SortableFuncAllBinary sorterDef.order)
        (res, sorterDef)


    // returns (NumStagesUsed, NumSwitchesUsed, Count)  
    let MakeStageAndSwitchUseHistogram (order:int) 
                                       (sorterLen: int) 
                                       (randGenerationMode : RandGenerationMode)
                                       (sorterCount:int) (seed : int) =

        let MakeHistoLine (tt:int[]) = 
            sprintf "%d\t%A\t%d\t%d\t%d"
                    order randGenerationMode tt.[0] tt.[1] tt.[2]

        let rnd = new Random(seed)
        let histogram =
            {1 .. sorterCount}
            |> Seq.map(fun i -> SortersFromData.CreateRandomSorterDef order sorterLen randGenerationMode rnd)
            |> Seq.toArray
            |> Array.Parallel.map(fun sorterDef -> GetFullSortingResultsUsingIntArray2 sorterDef)
            |> Seq.filter(fun ((success, switchResults), sorterDef) -> success)
            |> Seq.map(fun ((success, switchResults), sorterDef) -> Sorting.SorterResult.MakeSorterResult sorterDef switchResults)
            |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
            |> Seq.map(fun ((success, switchResults), sorterResults) -> [| success; switchResults; sorterResults|> Seq.length |])
            |> Seq.toArray

        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%A seed:%d sorterCount:%d resultCount:%d" 
                                order sorterLen randGenerationMode seed sorterCount (histogram|>Array.sumBy(fun a -> a.[2]))
        (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))




 
    //// returns (NumStagesUsed, NumSwitchesUsed, Count)  
    //let MakeStageAndSwitchUseHistogramOld (order:int) 
    //                                   (sorterLen: int) 
    //                                   (randGenerationMode : RandGenerationMode)
    //                                   (sorterCount:int) (seed : int) =

    //    let MakeHistoLine (tt:int[]) = 
    //        sprintf "%d\t%A\t%d\t%d\t%d"
    //                order randGenerationMode tt.[0] tt.[1] tt.[2]

    //    let rnd = new Random(seed)
    //    let histogram =
    //        {1 .. sorterCount}
    //        |> Seq.map(fun i -> SortersFromData.CreateRandomSorterDef order sorterLen randGenerationMode rnd)
    //        |> Seq.toArray
    //        |> Array.Parallel.map(fun sorterDef -> GetFullSortingResultsUsingIntArray2 sorterDef)
    //        |> Seq.filter(fun ((success, switchResults), sorterDef) -> success)
    //        |> Seq.map(fun ((success, switchResults), sorterDef) -> Sorting.SorterResult.MakeSorterResult sorterDef switchResults)
    //        |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
    //        |> Seq.map(fun ((success, switchResults), sorterResults) -> [| success; switchResults; sorterResults|> Seq.length |])
    //        |> Seq.toArray

    //    let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%A seed:%d sorterCount:%d resultCount:%d" 
    //                            order sorterLen randGenerationMode seed sorterCount (histogram|>Array.sumBy(fun a -> a.[2]))
    //    (summary, histogram |> Array.map(fun i-> (MakeHistoLine i)))