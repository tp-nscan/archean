namespace archean.core
open System
open archean.core.Sorting
open archean.core.Sorting.SortableIntArray
open archean.core.SortersFromData
open Sorter



module TestData =

    let MakeRandomSorterDef (order:int) (length:int) (seed:int) =
        let rnd = new Random(seed)
        let switchSet = SwitchSet.ForOrder order
        SorterDef.CreateRand switchSet length rnd

    let SwitchSeq (order:int) (seed:int) = 
        let rnd = new Random(seed)
        SwitchSet.RandomSwitchesOfOrder order rnd

    let StagedSorterDef = 
        RefSorter.CreateRefStagedSorter RefSorter.End16
          