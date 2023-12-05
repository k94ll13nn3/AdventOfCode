module Day5

open System
open Helper

let number = 5

let title = "If You Give A Seed A Fertilizer"

let lines = readLines "Input5.txt"

let getRanges (input: _[]) =
    let rec getRangesRec index (rangesAcc: (uint64 * uint64 * uint64) list list) =
        match index with
        | _ when index = input.Length -> rangesAcc |> List.rev |> List.tail // tail to remove extra [] from file line end
        | _ ->
            match input.[index] with
            | "" -> getRangesRec (index + 1) ([] :: rangesAcc)
            | _ ->
                let parts = input.[index] |> splitC ' '

                if parts.Length <> 3 then
                    getRangesRec (index + 1) rangesAcc
                else
                    let h :: t = rangesAcc
                    let newRange = (uint64 parts.[0], uint64 parts.[1], uint64 parts.[2])
                    getRangesRec (index + 1) ((newRange :: h) :: t)

    getRangesRec 0 [ [] ]

let applyRanges seed ranges =
    let rec applyRangeRec i range =
        match range with
        | [] -> i
        | (dest, src, len) :: _ when i >= src && i < (src + len) -> dest + (i - src)
        | _ :: t -> applyRangeRec i t

    List.fold applyRangeRec seed ranges

let applyRangesToSeedRange ranges seed =
    Array.init (int (snd seed)) (fun v -> (uint64 v) + (fst seed))
    |> Array.fold (fun acc seed -> ranges |> applyRanges seed |> min acc) UInt64.MaxValue

let computeFirst () =
    let ranges = getRanges lines.[1..]

    lines.[0].[6..]
    |> splitC ' '
    |> Array.map (uint64 >> (fun i -> (i, 1UL)) >> applyRangesToSeedRange ranges)
    |> Array.min

let computeSecond () =
    let seeds = lines.[0].[6..] |> splitC ' ' |> Array.map uint64
    let ranges = getRanges lines.[1..]

    Array.init (seeds.Length / 2) (fun i -> (seeds.[i * 2], seeds.[(i * 2) + 1]))
    |> Array.map (applyRangesToSeedRange ranges)
    |> Array.min
