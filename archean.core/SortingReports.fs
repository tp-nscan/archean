namespace archean.core
open System
open archean.core.Combinatorics_Types
open archean.core.Sorting
open archean.core.SortersFromData
open Sorting.SorterDef
open SorterA

module SortingReports =
  
    let GetFullSortingResultsUsingIntArray (sorterDef:SorterDef) =
        let sorter = Sorter.MakeSorter SortableIntArray.SwitchFuncForSwitch sorterDef
        (Sorter.GetSwitchResultsForSorter sorter (IntBits.AllBinaryTestCases sorterDef.order), sorterDef)


    // returns (NumStagesUsed, NumSwitchesUsed, Count)
    let MakeStageAndSwitchUseHistogram (order:int) 
                                       (sorterLen: int) 
                                       (randGenerationMode : RandGenerationMode)
                                       (sorterCount:int) (seed : int) =

        let MakeHisoLine (tt:int[]) = 
            sprintf "%d\t%A\t%d\t%d\t%d"
                    order randGenerationMode tt.[0] tt.[1] tt.[2]

        let rnd = new Random(seed)
        let histogram =
            {1 .. sorterCount}
            |> Seq.map(fun i -> SortersFromData.CreateRandomSorterDef order sorterLen randGenerationMode rnd)
            |> Seq.toArray
            |> Array.Parallel.map(fun i -> GetFullSortingResultsUsingIntArray i)
            |> Seq.filter(fun res -> fst (fst res))
            |> Seq.map(fun i -> Sorting.SorterResult.MakeSorterResult (snd i) (snd (fst i)) )
            |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
            |> Seq.map(fun i -> [| (fst (fst i)); (snd (fst i)); (snd i)|> Seq.length |])
            |> Seq.toArray

        let summary = sprintf "order:%d sorterLen:%d randGenerationMode:%A seed:%d sorterCount:%d resultCount:%d" 
                                order sorterLen randGenerationMode seed sorterCount (histogram|>Array.sumBy(fun a -> a.[2]))
        (summary, histogram |> Array.map(fun i-> (MakeHisoLine i)))


    let GetFullSortingResultsUsingGenerated (sorterDef:SorterDef) =
        true
        //if (sorterDef.order = 6) then
        //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
        //    (GeneratedSortables.Sortable6.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable6.AllBinaryTestCases, sorterDef)
        //elif (sorterDef.order = 7) then
        //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable7.SwitchFuncForSwitch sorterDef
        //    (GeneratedSortables.Sortable7.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable7.AllBinaryTestCases, sorterDef)
        //elif (sorterDef.order = 8) then
        //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable8.SwitchFuncForSwitch sorterDef
        //    (GeneratedSortables.Sortable8.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable8.AllBinaryTestCases, sorterDef)
        //elif (sorterDef.order = 9) then
        //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable9.SwitchFuncForSwitch sorterDef
        //    (GeneratedSortables.Sortable9.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable9.AllBinaryTestCases, sorterDef)
        //elif (sorterDef.order = 10) then
        //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable10.SwitchFuncForSwitch sorterDef
        //    (GeneratedSortables.Sortable10.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable10.AllBinaryTestCases, sorterDef)
        //else
        //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable11.SwitchFuncForSwitch sorterDef
        //    (GeneratedSortables.Sortable11.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable11.AllBinaryTestCases, sorterDef)

          

    let Bencho (reps:int) =
        true
        //let rnd = new Random(12883)
        //let order = 6
        //let len = 100
        //// order randGenerationMode NumStagesUsed NumSwitchesUsed Count
        //let MakeHisoLine (tt:(int*int)*seq<SorterResult>) = 
        //    sprintf "%d\t%d\t%d\t%d"
        //            order (fst (fst tt)) (snd (fst tt))
        //            ((snd tt)|> Seq.length)

        //let GetSorterWithResults (i:int) =
        //    let sorterDef = SorterDef.CreateRandomPackedStages order len rnd
        //    let sorter = Sorter.MakeSorter GeneratedSortables.Sortable6.SwitchFuncForSwitch sorterDef
        //    (GeneratedSortables.Sortable6.GetSwitchResultsForSorter sorter GeneratedSortables.Sortable6.AllBinaryTestCases, sorterDef)
        
        //let counts = {1 .. reps} |> Seq.map(fun i -> GetSorterWithResults i)
        //                         |> Seq.filter(fun res -> fst (fst res))
        //                         |> Seq.map(fun i -> Sorting.SorterResult.MakeSorterResult (snd i) (snd (fst i)) )
        //                         |> Seq.groupBy(fun res -> SorterResult.SorterResultKey res.stageResults)
        //                         |> Seq.toArray

        //let res = counts |> Array.map(fun i -> (MakeHisoLine i))
        //res
