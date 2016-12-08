module Day8

open Reader
open System

type Rotate = Row | Column
type Instruction = 
    | Rectangle of (int * int)
    | Rotation of (Rotate * int * int)

let toRectangle (str : string) =
    let parts = str.Split([| 'x' |])
    Rectangle (int parts.[0], int parts.[1])

let toRotation (l : string[]) =
    let parts = l.[2].Split([| '=' |])
    let coord = (int parts.[1], int l.[4])
    match l.[1] with
    | "column" -> Rotation (Column, fst coord, snd coord)
    | "row" -> Rotation (Row, fst coord, snd coord)
    | _ -> failwith "toRotation - err" 

let parse (str : string) = 
    let parts = str.Split([| ' ' |])
    match parts.[0] with
    | "rect" -> toRectangle parts.[1]
    | "rotate" -> toRotation parts
    | _ -> failwith "parse - err" 

let executeInstruction (a : int[,]) (i : Instruction) =
    let n = Array2D.length2 a
    let m = Array2D.length1 a
    match i with
    | Rectangle (x, y) -> a |> Array2D.mapi (fun i j e -> if i < y && j < x then 1 else e)
    | Rotation (Row, x, y) -> a |> Array2D.mapi (fun i j e -> if i = x then a.[i, (n + j - y) % n] else e)
    | Rotation (Column, x, y) -> a |> Array2D.mapi (fun i j e -> if j = x then a.[(m + i - y) % m, j] else e)

let computeArray =
    readLines @"input8.txt" 
    |> Seq.map parse 
    |> Seq.fold executeInstruction (Array2D.create 6 50 0)

let computeFirst =
    computeArray
    |> Seq.cast<int> |> Seq.sum

let computeSecond =
    let code = computeArray |> Array2D.map (fun e -> if e = 1 then '█' else ' ')
    seq {0 .. Array2D.length1 code - 1} 
    |> Seq.map (fun i -> String (code.[i, *]))
    |> Seq.iter (printfn "%s")
    0