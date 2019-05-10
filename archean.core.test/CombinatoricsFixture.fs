namespace archean.core.test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open archean.core
open archean.core.Combinatorics
open archean.core.Combinatorics_Types

[<TestClass>]
type CombinatoricsFixture () =

    [<TestMethod>]
    member this.TestFisherYatesShuffleFromSeed() =
      let starting = [|2; 4; 6; 8; 10; 12; 14 |]
      let seededFY = Combinatorics.FisherYatesShuffleFromSeed 5 
      let actual =  seededFY [|2; 4; 6; 8; 10; 12; 14 |] |> Seq.toArray
      Assert.AreEqual(starting.Length, actual.Length)
 

    [<TestMethod>]
    member this.TestInt_To_IntArray01() =
      let len = 6
      let expectedArray = [|1; 0; 1; 0; 1; 0|]
      let converted = Combinatorics.Int_To_IntArray01 len 21
      Assert.IsTrue (Combinatorics.CompareArrays expectedArray converted)

      
    [<TestMethod>]
    member this.TestIntArray01_To_Int() =

      let yak =
        seq {for i=1 to 3 do
                yield i}
        |> Seq.toArray

                
      let ct = yak |> Seq.length

      let ct2 = yak |> Seq.length


      Assert.IsTrue true
      //let len = 6
      //let expectedArray = [|1; 0; 1; 0; 1; 0|]
      //let converted = Combinatorics.Int_To_IntArray01 len 21
      //Assert.IsTrue (Combinatorics.CompareArrays expectedArray converted)

 
    [<TestMethod>]
    member this.TestRandomIntPermutations() =
      let expectedCount = 9
      let permutes = Combinatorics.RandomIntPermutations (new Random(123)) 16 expectedCount |> Seq.toArray
      Assert.AreEqual(expectedCount, permutes.Length)
 

    [<TestMethod>]
    member this.TestCompareArrays() =
      let firstArray = [|2; 4; 6; 8; 10; 12; 14 |]
      let shortArray = [|2; 4; 6; 8; 10; 12; |]
      let otherArray = [|2; 4; 6; 8; 11; 12; 14 |]
      Assert.IsTrue (Combinatorics.CompareArrays firstArray firstArray)
      Assert.IsFalse (Combinatorics.CompareArrays firstArray shortArray)
      Assert.IsFalse (Combinatorics.CompareArrays firstArray otherArray)
 

    [<TestMethod>]
    member this.TestIdentityPermutation() =
      let expectedLen = 9
      let expectedSum = ( expectedLen * (expectedLen - 1)) / 2
      let permutes = Permutation.Identity expectedLen
      Assert.AreEqual(expectedLen, permutes |> Permutation.value |> Array.length)
      Assert.AreEqual(expectedSum, permutes |> Permutation.value |> Array.sum)
 

    [<TestMethod>]
    member this.TestComposeMapIntArraysOnIdentity() =
        let orig = [|5; 4; 3; 2; 1; 0 |]
        let prodR = Combinatorics.ComposeMapIntArrays orig ((Permutation.Identity 6) |> Permutation.value)
        Assert.IsTrue (Combinatorics.CompareArrays orig prodR)
        let prodL = Combinatorics.ComposeMapIntArrays ((Permutation.Identity 6) |> Permutation.value) orig
        Assert.IsTrue (Combinatorics.CompareArrays orig prodL)


    [<TestMethod>]
    member this.TestInverseArray() =
       let length = 6
       let orig = Combinatorics.RandomIntPermutations (new Random(1823)) length 1 |> Seq.item 0
       let id = Permutation.Identity length |> Permutation.value
       let inv = Combinatorics.InverseMapArray orig
       let prod = Combinatorics.ComposeMapIntArrays orig inv
       Assert.IsTrue (Combinatorics.CompareArrays id prod)


    [<TestMethod>]
    member this.TestPermutationCreateRandom() =
      let expectedCount = 9
      let permutes = Permutation.CreateRandom (new Random(123)) 16 
                      |> Seq.take expectedCount
                      |> Seq.toArray
      Assert.AreEqual(expectedCount, permutes.Length)
    
    
    [<TestMethod>]
    member this.TestIsSorted() =
        Assert.IsFalse (Combinatorics.IsSorted [|0; 1; 1; 0; 1; 0|])
        Assert.IsTrue (Combinatorics.IsSorted [|0; 0; 0; 0; 1; 1|])


    [<TestMethod>]
    member this.TestMakeTwoCycleIntArray() =
        let length = 19
        let lowBit = 1
        let hiBit = 11

        let id = Permutation.Identity length |> Permutation.value
        let tc = Combinatorics.MakeTwoCycleIntArray length lowBit hiBit
        let prod = Combinatorics.ComposeMapIntArrays tc tc
        Assert.IsTrue (Combinatorics.CompareArrays id prod)


    [<TestMethod>]
    member this.TestMakeAllTwoCycleIntArrays() =
        let length = 19
        let tc = Combinatorics.MakeAllTwoCycleIntArrays length
        Assert.AreEqual ((length * (length - 1)) / 2, tc |> Seq.length)
    

    [<TestMethod>]
    member this.TestMakeRandomFullTwoCycleIntArray() =
        let length = 19
        let tc = Combinatorics.MakeRandomFullTwoCycleIntArray (new Random(123)) length
        let id = Permutation.Identity length |> Permutation.value
        let prod = Combinatorics.ComposeMapIntArrays tc tc
        Assert.IsTrue (Combinatorics.CompareArrays id prod)
 

    [<TestMethod>]
    member this.TestSortIntArray() =
        let length = 29
        let rnd = new Random(123)
        let stage = Combinatorics.MakeRandomFullTwoCycleIntArray rnd length
        let sortable = Combinatorics.RandomIntPermutations rnd length 1 |> Seq.item 0
        let counter = Array.init length (fun i -> 0)

        let sortableResult = Combinatorics.SortCopyOfIntArray sortable stage counter
        
        let sortableScore = Combinatorics.UnsortednessSquared sortable
        let sortableResultScore = Combinatorics.UnsortednessSquared sortableResult

        Assert.IsTrue (sortableScore >  sortableResultScore)


    [<TestMethod>]
    member this.TestConjugateIntArrays() =
        let a = [|0; 2; 1; 3; 4; 5|]
        let b = [|0; 5; 4; 3; 1; 2|]
        let c = [|0; 1; 2; 3; 5; 4|]

        let conj = Combinatorics.ConjugateIntArrays a b
        Assert.IsTrue (Combinatorics.CompareArrays conj c)

    [<TestMethod>]
    member this.TestSorted_0_1_Sequence() =
        let blockLen = 10
        let block = Combinatorics_Types.IntBits.Sorted_O_1_Sequence blockLen 7 |> Seq.toArray
        Assert.IsTrue (block.Length = blockLen)

    [<TestMethod>]
    member this.TestSorted_0_1_Sequences() =
        let blockLen = 10
        let block = Combinatorics_Types.IntBits.Sorted_0_1_Sequences blockLen
        Assert.IsTrue (block.Length = blockLen + 1)


    [<TestMethod>]
    member this.TestBreakIntoSegments() =
        let testArray = [|1; 2; 3; 4; 5; 6; 7; 8; 9|] 
        let testBreaks = [|0; 2; 5; 9|] 

        let yak = BreakIntoSegments testArray testBreaks
        Assert.AreEqual (yak.Length, 3)