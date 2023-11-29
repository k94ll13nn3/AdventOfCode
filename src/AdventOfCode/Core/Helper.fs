module Helper

open System.IO

let (+/) path1 path2 = Path.Combine(path1, path2)

let readLines filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadAllLines |> Array.toList

let readInput filePath =
    filePath |> (+/) @"Inputs\" |> File.ReadAllText
