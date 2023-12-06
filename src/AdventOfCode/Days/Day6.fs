module Day6

open Helper

let number = 6

let title = "Wait For It"

let lines = readLines "Input6.txt"

let inline countWinningWays time distance =
    let lower =
        seq { for i in 1L .. time -> i }
        |> Seq.takeWhile (fun t -> t * time - t * t <= distance)
        |> Seq.length

    let higher =
        seq { for i in time .. -1L .. 1L -> i }
        |> Seq.takeWhile (fun t -> t * time - t * t <= distance)
        |> Seq.length

    time - (higher + lower |> int64)

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
