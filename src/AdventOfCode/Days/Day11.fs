module Day11

open Helper
open System

let number = 11

let title = "Cosmic Expansion"

let lines = readLines "Input11.txt"

let computeDistance (px: int, py) (hx, hy) (rows, cols) multiplier =
    let expandRows =
        rows
        |> Array.filter (fun e -> (e >= px && e <= hx) || (e >= hx && e <= px))
        |> Array.length
        |> uint64

    let expandCols =
        cols
        |> Array.filter (fun e -> (e >= py && e <= hy) || (e >= hy && e <= py))
        |> Array.length
        |> uint64

    (abs (px - hx) |> uint64)
    + (abs (py - hy) |> uint64)
    + (expandCols * (multiplier - 1UL))
    + (expandRows * (multiplier - 1UL))

let rec computeDistances expand acc multiplier =
    function
    | [] -> acc
    | h :: t ->
        let dist = t |> List.sumBy (fun p -> computeDistance p h expand multiplier)
        computeDistances expand (acc + dist) multiplier t

let expand (input: string[]) =
    let rows (arr: string[]) =
        Seq.init arr.Length id
        |> Seq.filter (fun i -> arr.[i].Contains '#' |> not)
        |> Array.ofSeq

    (rows input, rotate input |> rows)

let points =
    seq {
        for x in 0 .. lines.Length - 1 do
            for y in 0 .. lines.[0].Length - 1 do
                if lines.[x].[y] = '#' then
                    yield (x, y)
    }
    |> List.ofSeq

let computeFirst () =
    computeDistances (expand lines) 0UL 2UL points

let computeSecond () =
    computeDistances (expand lines) 0UL 1000000UL points
