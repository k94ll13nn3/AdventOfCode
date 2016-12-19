module Day19

open System
open Helper

let input = 3014603

let splitList list = List.foldBack (fun x (l,r) -> x::r, l) list ([],[])

// 1   1 : 1
// 2   1 : 2 - 1
// 3   3 : 3
// 4   1 : 4 - 3
// 5   2 : 5 - 3
// 6   3 : 6 - 3
// 7   5 : 2*7 - 3*3
// 8   7 : 2*8 - 3*3
// 9   9 : 9
// 10  1 : 10 - 9
// 11  2 : 11 - 9
// 12  3 : 12 - 9
// 13  4 : 13 - 9
// 14  5 : 14 - 9
// 15  6 : 15 - 9
// 16  7 : 16 - 9
// 17  8 : 17 - 9
// 18  9 : 18 - 9
// 19 11 : 2*19 - 3*9
// 20 13 : 2*20 - 3*9

let computeFirst = 
    let rec step (a : int list) =
        if a.Length = 1
            then a.Head
            else let reduced = fst (splitList a) in 
                    match reduced.Length with
                    | 1 -> reduced.Head
                    | _ when a.Length % 2 = 0 -> step reduced
                    | _ -> step reduced.Tail
    step (List.init input (fun index -> index + 1))

let computeSecond = 
    let n = input |> float
    let t = Math.Pow(3.0,  Math.Log(n, 3.) |> int |> float)
    if n = t then n else max (n - t) (2.0 * n - 3.0 * t)