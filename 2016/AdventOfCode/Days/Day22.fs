module Day22

open System
open Helper
open Reader

type Node = {X : int; Y : int; Size : int; Used : int; Avail : int; Use : int}

let parse (line : string) =
    let parts = line.Split([|' '; '\t'|], StringSplitOptions.RemoveEmptyEntries)
    match parts.[0] with
    | Regex @"/dev/grid/node-x(\d+)-y(\d+)" [x; y] ->
        {X = int x;
        Y = int y;
        Size = int parts.[1].[0..parts.[1].Length-2];
        Used = int parts.[2].[0..parts.[2].Length-2];
        Avail = int parts.[3].[0..parts.[3].Length-2];
        Use = int parts.[4].[0..parts.[4].Length-2];}
    | _ -> failwith "parse - err"

let rec getNumberOfPairs n nodes =
    match nodes with
    | [] -> 0
    | h::t when n.X = h.X && n.Y = h.Y -> getNumberOfPairs n t
    | h::t when n.Used <= h.Avail -> 1 + getNumberOfPairs n t
    | h::t -> getNumberOfPairs n t

let display node =
    if node.Use > 80 && node.Size > 400
        then '#'
        else if node.Used <> 0
            then '.'
            else '_'

let globalNodes = 
    readLines @"input22.txt" 
    |> Seq.skip 2 
    |> Seq.map parse 
    |> Seq.toList

let computeFirst =
    let rec resolve (nodes : Node list)  =
        match nodes with
        | [] -> 0
        | h::t when h.Used = 0 -> resolve t
        | h::t -> (getNumberOfPairs h globalNodes) + (resolve t)
    resolve globalNodes

let computeSecond =
    let maxX = globalNodes |> List.maxBy (fun n -> n.X)
    let maxY = globalNodes |> List.maxBy (fun n -> n.Y)
    for j in 0..maxY.Y do
        for i in 0..maxX.X do
            printf "%c" (globalNodes |> List.find (fun n -> n.X = i && n.Y = j) |> display)
        printfn ""
    242


// 67 (to go where G is) + 5 * 35 (moving G 1 time requires 5 steps) = 242
//....................................G
//.....................................
//.....................................
//.....................................
//.........############################
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//.....................................
//......................_..............
//.....................................
//.....................................