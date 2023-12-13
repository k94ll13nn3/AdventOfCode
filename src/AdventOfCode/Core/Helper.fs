module Helper

open System
open System.IO

let (+/) path1 path2 = Path.Combine(path1, path2)

let inline readLinesSeq filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadLines

let inline readLines filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadAllLines

let inline readInput filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadAllText

let inline charToInt (c: char) = int c - int '0'

let inline splitC (c: char) (s: string) =
    s.Split(c, StringSplitOptions.RemoveEmptyEntries)

let inline splitA (c: char array) (s: string) =
    s.Split(c, StringSplitOptions.RemoveEmptyEntries)

let inline rotate (a: string[]) =
    [| for x in { 0 .. a.[0].Length - 1 } -> [| for y in { 0 .. a.Length - 1 } -> a.[y].[x] |] |]
    |> Array.map String

let inline rotateA (a: 'a[][]) =
    [| for x in { 0 .. a.[0].Length - 1 } -> [| for y in { 0 .. a.Length - 1 } -> a.[y].[x] |] |]

module Array =
    let copySet i v (arr: 'a[]) =
        let n = Array.copy arr
        n.[i] <- v
        n

    let copySet2 i j v (arr: 'a[][]) =
        let n = Array.copy (arr |> Array.map Array.copy)
        n.[i].[j] <- v
        n
