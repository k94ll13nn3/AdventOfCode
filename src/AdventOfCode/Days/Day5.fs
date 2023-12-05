module Day5

open Helper

let number = 5

let title = "If You Give A Seed A Fertilizer"

let lines = readLines "Input5.txt"

let getRanges (input: _[]) =
    let rec getRangesRec index (rangesAcc: (uint64 * uint64 * uint64) list list) =
        match index with
        | _ when index >= input.Length -> rangesAcc
        | _ when input.[index] = "" -> getRangesRec (index + 2) ([] :: rangesAcc)
        | _ ->
            let parts = input.[index] |> splitC ' '
            let h :: t = rangesAcc
            let newRange = (uint64 parts.[0], uint64 parts.[1], uint64 parts.[2])
            getRangesRec (index + 1) ((newRange :: h) :: t)

    getRangesRec 0 [ [] ]

let computeFirst () =
    let seed = lines.[0].[6..] |> splitA [| ' ' |] |> Array.map uint64
    let ranges = getRanges lines.[1..] |> List.rev // ranges are stored last to first when reading input

    let rec applyRange range i =
        match range with
        | [] -> i
        | (dest, src, len) :: _ when i >= src && i < (src + len) -> dest + (i - src)
        | _ :: t -> applyRange t i

    ranges
    |> List.fold (fun acc range -> acc |> Array.map (applyRange range)) seed
    |> Array.min

let computeSecond () =
    let ranges = getRanges lines.[1..]
    let seeds = lines.[0].[6..] |> splitC ' ' |> Array.map uint64

    let seedRanges =
        List.init (seeds.Length / 2) (fun i -> (seeds.[i * 2], seeds.[(i * 2) + 1]))

    let rec checkIfInSeedRange sr v =
        match sr with
        | [] -> false
        | (str, len) :: _ when v >= str && v < (str + len) -> true
        | _ :: t -> checkIfInSeedRange t v

    let rec applyRangeReverseRec i range =
        match range with
        | [] -> i
        | (dest, src, len) :: _ when i >= dest && i < (dest + len) -> src + (i - dest)
        | _ :: t -> applyRangeReverseRec i t

    let rec applyReverse i =
        let res = List.fold applyRangeReverseRec i ranges |> checkIfInSeedRange seedRanges
        if res then i else applyReverse (i + 1UL)

    applyReverse 1UL
