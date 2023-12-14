module Day14

open Helper
open System

let number = 14

let title = "Parabolic Reflector Dish"

let lines = readLines "Input14.txt"

let rec compute i index acc inc =
    if index = lines.Length then
        acc
    else
        match lines.[index].[i] with
        | '.' -> compute i (index + 1) acc inc
        | 'O' -> compute i (index + 1) (acc + inc) (inc - 1)
        | _ -> compute i (index + 1) acc (lines.Length - index - 1)

let computeFirst () =
    Seq.init lines.[0].Length id |> Seq.sumBy (fun i -> compute i 0 0 lines.Length)

let computeSecond () = 0
