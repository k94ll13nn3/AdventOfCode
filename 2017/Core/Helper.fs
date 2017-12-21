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

    let first (arr: 'a array) = arr.[0]

    let others (arr: 'a array) = arr.[1..]

    let split arr = first arr, others arr  

// https://stackoverflow.com/a/20883296/3836163
// https://stackoverflow.com/a/29878501/3836163
module Array2D = 
    let foldi (folder: 'S -> 'T -> 'S) (state: 'S) (array: 'T[,]) =
        let mutable state = state
        for x in 0 .. Array2D.length1 array - 1 do
            for y in 0 .. Array2D.length2 array - 1 do
                state <- folder state (array.[x, y])
        state

    let joinByRows (a1: 'a[,]) (a2: 'a[,]) =
        let a1l1,a1l2,a2l1,a2l2 = (Array2D.length1 a1),(Array2D.length2 a1),(Array2D.length1 a2),(Array2D.length2 a2)
        if a1l2 <> a2l2 then failwith "arrays have different column sizes"
        let result = Array2D.zeroCreate (a1l1 + a2l1) a1l2
        Array2D.blit a1 0 0 result 0 0 a1l1 a1l2
        Array2D.blit a2 0 0 result a1l1 0 a2l1 a2l2
        result

    let joinByCols (a1: 'a[,]) (a2: 'a[,]) =
        let a1l1,a1l2,a2l1,a2l2 = (Array2D.length1 a1),(Array2D.length2 a1),(Array2D.length1 a2),(Array2D.length2 a2)
        if a1l1 <> a2l1 then failwith "arrays have different row sizes"
        let result = Array2D.zeroCreate a1l1 (a1l2 + a2l2)
        Array2D.blit a1 0 0 result 0 0 a1l1 a1l2
        Array2D.blit a2 0 0 result 0 a1l2 a2l1 a2l2
        result

    // here joiner function must be Array2D.joinByRows or Array2D.joinByCols
    let joinMany joiner (a: seq<'a[,]>)  = 
        let arrays = a |> Array.ofSeq
        if Array.length arrays = 0 then 
            failwith "no arrays"
        elif Array.length arrays = 1 then 
            Array.first arrays
        else
            let rec doJoin acc arrays = 
                if Array.length arrays = 0 then
                    acc
                elif Array.length arrays = 1 then
                    joiner acc (Array.first arrays)
                else
                    let acc = joiner acc (Array.first arrays)
                    doJoin acc (Array.others arrays)
            doJoin <|| Array.split arrays
            // or doJoin arrays.[0] arrays.[1..] 