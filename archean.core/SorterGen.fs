namespace archean.core
open System
open Combinatorics_Types
open Microsoft.FSharp.Collections
open Sorting

module SorterGen =
    type SorterGenMode = 
        | FullRand of SorterDef.RandGenerationMode
        | Prefixed of SortersFromData.GenPrefixMode

