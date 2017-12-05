module Day1

open Helper

let compute (ls:char[]) (offset:int) =
    ls
    |> Array.mapi (fun i e -> if e = ls.[(i + offset) % ls.Length] then (charToInt e) else 0)
    |> Array.sum

let computeFirst =
    let ls = readInput "Input1.txt"
    compute (ls |> Array.ofSeq) 1

let computeSecond =
    let ls = readInput "Input1.txt"
    compute (ls |> Array.ofSeq) (ls.Length/2)