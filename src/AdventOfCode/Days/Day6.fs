module Day6

open Helper

let number = 6

let title = "Wait For It"

let lines = readLines "Input6.txt"

let inline countWinningWays (time: int64) distance =
    // -t2 + t*time - distance = 0
    let detla = (time * time - (4L * (-distance) * -1L)) |> float |> sqrt
    let lower = ((float -time) + detla) / (2.0 * -1.0) |> floor |> int64
    let higher = ((float -time) - detla) / (2.0 * -1.0) |> floor |> int64

    higher - lower

let computeFirst () =
    let times = lines.[0] |> splitA [| ':'; ' ' |] |> Array.tail |> Array.map int64
    let distances = lines.[1] |> splitA [| ':'; ' ' |] |> Array.tail |> Array.map int64

    seq { for i in 0 .. times.Length - 1 -> i }
    |> Seq.map (fun i -> countWinningWays times.[i] distances.[i])
    |> Seq.reduce (*)

let computeSecond () =
    let time = lines.[0].Replace(" ", "") |> splitC ':' |> Array.item 1 |> int64
    let distance = lines.[1].Replace(" ", "") |> splitC ':' |> Array.item 1 |> int64

    countWinningWays time distance
