module Day5

open Helper

let number = 5

let title = "If You Give A Seed A Fertilizer"

let lines = readLines "Input5.txt"

let getRanges (input: _[]) =
    let rec getRangesRec index (rangesAcc: (int64 * int64 * int64) list list) =
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
                    let newRange = (int64 parts.[0], int64 parts.[1], int64 parts.[2])
                    getRangesRec (index + 1) ((h @ [ newRange ]) :: t)

    getRangesRec 0 [ [] ]

let rec applyRange (range: (int64 * int64 * int64) list) (i: int64) =
    match range with
    | [] -> i
    | (dest, src, len) :: t ->
        if i >= src && i <= (src + len) then
            dest + (i - src)
        else
            applyRange t i

let computeFirst () =
    let seed = lines.[0].[6..] |> splitA [| ' ' |] |> Array.map int64
    let ranges = getRanges lines.[1..]

    ranges
    |> List.fold (fun acc range -> acc |> Array.map (applyRange range)) seed
    |> Array.min

let computeSecond () =
    let seed = lines.[0].[6..] |> splitA [| ' ' |] |> Array.map int64
    let seed1 = Array.init (int seed.[1]) (fun i -> (int64 i) + seed.[0])
    let seed2 = Array.init (int seed.[3]) (fun i -> (int64 i) + seed.[2])
    let seeds = Array.concat [ seed1; seed2 ]
    let ranges = getRanges lines.[1..]

    ranges
    |> List.fold (fun acc range -> acc |> Array.map (applyRange range)) seeds
    |> Array.min
