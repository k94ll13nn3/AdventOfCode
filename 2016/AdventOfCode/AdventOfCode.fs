module AdventOfCode

open Day5

[<EntryPoint>]
let main argv =
    computeFirst |> printfn "%A"
    computeSecond |> printfn "%A"
    0 // return an integer exit code