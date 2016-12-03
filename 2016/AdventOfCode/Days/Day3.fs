module Day3

open Reader
open System

type Triangle = {A : int; B : int; C: int}
let isTriangle t = 
    t.A + t.B > t.C &&
    t.A + t.C > t.B &&
    t.B + t.C > t.A

let parseLine (s : string) =
    let splitted = s.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
    match splitted with
    | [|a; b; c|] -> {A = Int32.Parse(a); B = Int32.Parse(b); C = Int32.Parse(c)}
    | _-> failwith "parseLine - err"

let rec parseList (s : string list) =
    match s with
    | a::b::c::t -> {A = Int32.Parse(a); B = Int32.Parse(b); C = Int32.Parse(c)} :: (parseList t)
    | _ -> []

let takeColumn (i :int) (s : string[][]) =
    s.[0..] 
    |> Array.map (fun x -> x.[i])

let concatColumns (s : string[][]) =
    [| takeColumn 0 s ; takeColumn 1 s; takeColumn 2 s |]
    |> Array.concat 
    |> Array.toList

let invertAndParse (s : seq<string>) =
    s 
    |> Seq.toArray 
    |> Array.map (fun x -> x.Split([|' '|], StringSplitOptions.RemoveEmptyEntries))
    |> concatColumns
    |> parseList

let computeFirst =
    readLines @"input3.txt"
    |> Seq.map parseLine
    |> Seq.filter isTriangle
    |> Seq.length

let computeSecond =
    readLines @"input3.txt"
    |> invertAndParse
    |> Seq.filter isTriangle
    |> Seq.length