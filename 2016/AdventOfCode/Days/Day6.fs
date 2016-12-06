module Day6

open Reader
open Helper
open System

let rotateStringSeq (s : seq<string>) = s |> Seq.toArray |> rotateString |> Seq.ofArray

let compute sort =
    readLines @"input6.txt"
    |> rotateStringSeq
    |> Seq.map (Seq.groupBy id >> Seq.sortBy sort >>Seq.head >> fst)
    |> seqToString

let computeFirst =
    compute (fun (c, l) -> - (Seq.length l))

let computeSecond =
    compute (fun (c, l) -> Seq.length l)