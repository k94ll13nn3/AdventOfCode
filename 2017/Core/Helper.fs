module Helper

open System.IO
open System.Text.RegularExpressions

let (+/) path1 path2 = Path.Combine(path1, path2)

let readLines filePath = 
    filePath
    |> (+/) @"Inputs\"
    |> File.ReadAllLines

let readInput filePath = 
    filePath
    |> (+/) @"Inputs\"
    |> File.ReadAllText

//http://fssnip.net/29
let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let inline charToInt (c:char) = int c - int '0'

module Array =
    let copySet i v (arr:'a[]) =
        let n = Array.copy arr
        n.[i] <- v
        n