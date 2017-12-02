module AdventOfCode

open Day1

[<EntryPoint>]
let main argv =
    computeFirst |> printfn "%A"
    computeSecond |> printfn "%A"
    0 // return an integer exit code