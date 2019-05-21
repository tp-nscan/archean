namespace archean.core
open System
open archean.core.Sorting
open archean.core.Sorting.SortableIntArray
open archean.core.SortersFromData
open Sorter



module TestData =

    let MakeRandomSorterDef (order:int) (length:int) (seed:int) =
        let rnd = new Random(seed)
        SorterDef.CreateRandom order length rnd

    let SwitchSeq (order:int) (seed:int) = 
        let rnd = new Random(seed)
        Switch.RandomSwitchesOfOrder order rnd

    let StagedSorterDef = 
        RefSorter.CreateRefStagedSorter RefSorter.End16
          