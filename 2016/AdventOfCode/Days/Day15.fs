module Day15

open Reader
open Helper
open System

type Disc = {Number : int; Positions : int; StartPosition : int}

let parse (str : string) =
    let parts = str.Split([|' '; '.'; '#'|])
    {Number = int parts.[2]; Positions = int parts.[4]; StartPosition = int parts.[12]}

let isValidState (i : int) (discs : Disc list) = 
    discs |> List.exists (fun d -> ((d.StartPosition + i + d.Number) % d.Positions) <> 0) |> not

let resolve (discs : Disc list) = 
    Seq.initInfinite id |> Seq.find (fun i -> isValidState i discs)

let computeFirst =
    readLines "input15.txt" |> Seq.map parse |> Seq.toList |> resolve

let computeSecond =
    readLines "input15-bis.txt" |> Seq.map parse |> Seq.toList |> resolve