module Day17

open Helper
open System
open Types
open System.Security.Cryptography
open System.Text

let md5Hasher = CryptoConfig.CreateFromName("MD5") :?> HashAlgorithm
let md5 (str : string) =
    let tmp =
        str
        |> Encoding.UTF8.GetBytes
        |> md5Hasher.ComputeHash
        |> BitConverter.ToString
    tmp.Replace("-","").ToLower()

let input = "dmypynyp"
let invalidPoint = {X = 10; Y = 10}

let transform (i : int) (c : char) =
    match c with
    | _ when c >= 'b' && c <= 'f' -> ["U"; "D"; "L"; "R"].[i]
    | _ -> "Z"

let validate (p : Point) =
    if p.X >= 0 && p.X <= 3 && p.Y >= 0 && p.Y <= 3
        then p
        else invalidPoint

let move (dir : string) (p : Point) =
    match dir with
    | "U" -> {X = p.X - 1; Y = p.Y} |> validate
    | "D" -> {X = p.X + 1; Y = p.Y} |> validate
    | "L" -> {X = p.X; Y = p.Y - 1} |> validate
    | "R" -> {X = p.X; Y = p.Y + 1} |> validate
    | _ -> failwith "move - err"

let getNewElements current p = 
    md5 current 
    |> Seq.take 4
    |> Seq.mapi transform
    |> Seq.filter (fun elem -> elem <> "Z")
    |> Seq.map (fun elem -> current + elem, move elem p)
    |> Seq.filter (fun elem -> snd elem <> invalidPoint)
    |> Seq.toList

let computeFirst =
    let rec findShortestPath (q : Container<string * Point>) =
        let ((current, p), rest) = take q
        match p = {X = 3; Y = 3} with 
        | true -> current.Substring(input.Length)
        | false -> findShortestPath ((getNewElements current p) ++> rest)
    findShortestPath ((input, origin) --> emptyQueue)

let computeSecond =
    let rec findShortestPath (q : Container<string * Point>) (paths : int list) =
        match q = emptyQueue with
        | true -> paths |> List.max
        | false -> 
            let ((current, p), rest) = take q
            match p = {X = 3; Y = 3} with 
            | true -> findShortestPath rest (current.Substring(input.Length).Length :: paths)
            | false -> findShortestPath ((getNewElements current p) ++> rest) paths
    findShortestPath ((input, origin) --> emptyQueue) []