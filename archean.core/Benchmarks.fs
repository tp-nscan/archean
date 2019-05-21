namespace archean.core
open System
open archean.core.Sorting
open archean.core.SortersFromData
open archean.core.Sorter

module Benchmarks = 
    
    let SorterBenchA (seed:int) (order:int) (stageCount:int) (sorterCount:int) =
        let rnd = new Random(seed)
        let Sorters = { 0 .. (sorterCount - 1)} 
                        |> Seq.map(fun i ->
                            SorterDef.CreateRandomPackedStages order (stageCount*order / 2) rnd)
        let res = Sorters |> Seq.map(fun s -> Sorter.Eval s)
                          |> Seq.filter(fun r -> fst r )
                          |> Seq.toArray
        res.Length


    let SorterBenchB (seed:int) (order:int) (stageCount:int) (sorterCount:int) =
        let rnd = new Random(seed)
        let Sorters = { 0 .. (sorterCount - 1)} 
                        |> Seq.map(fun i ->
                            SorterDef.CreateRandomPackedStages order (stageCount*order / 2) rnd)

        let res = Sorters |> Seq.toArray
                          |> Array.Parallel.map(fun s -> Sorter.Eval s)
                          |> Seq.filter(fun r -> fst r )
                          |> Seq.toArray
        res.Length