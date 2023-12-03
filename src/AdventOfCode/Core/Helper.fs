module Helper

open System.IO

let (+/) path1 path2 = Path.Combine(path1, path2)

let readLinesSeq filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadLines

let readLines filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadAllLines

let readInput filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadAllText

let inline charToInt (c: char) = int c - int '0'
