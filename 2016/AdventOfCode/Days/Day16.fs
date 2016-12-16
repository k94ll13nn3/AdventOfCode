module Day16

open Reader
open Helper
open System

let input = "11110010111001001"

let rec checksum (str : char[]) =
    if str.Length % 2 = 1
        then str
        else [|0..2..str.Length-2|] |> Array.map (fun i -> if str.[i] = str.[i+1] then '1' else '0') |> checksum

let rec grow (i : int) (s : char[]) =
    if s.Length > i
        then s |> Array.take i
        else [|s; [|'0'|] ; s |> Array.rev |> Array.map (fun c -> if c = '1' then '0' else '1')|] |> Array.concat |> grow i

let resolve (str : string) (i : int) =
    str.ToCharArray() |> grow i |> checksum |> String

let computeFirst =
    resolve input 272

let computeSecond =
    resolve input 35651584