module Day4

open Helper

let number = 4

let title = "Scratchcards"

let lines = readLines "Input4.txt"

let parseLine line =
    let parts = line |> splitA [| ':'; '|' |]

    let gameNumber = parts.[0] |> splitC ' ' |> Array.item 1 |> int
    let winning = parts.[1] |> splitC ' ' |> Array.map int |> Set.ofArray
    let drawn = parts.[2] |> splitC ' ' |> Array.map int |> Set.ofArray

    (gameNumber, Set.intersect winning drawn |> Set.count)

let rec countGames index (acc: _[]) map =
    if index = acc.Length - 1 then
        acc |> Array.sum
    else
        let value = map |> Map.tryFind (index + 1)

        if value.IsSome then
            for i in (index + 1) .. (index + value.Value) do
                acc.[i] <- acc.[i] + acc.[index]

        countGames (index + 1) acc map

let computeFirst () =
    lines |> Array.map (parseLine >> snd >> (+) -1 >> pown 2) |> Array.sum

let computeSecond () =
    lines
    |> Array.map parseLine
    |> Map.ofArray
    |> countGames 0 (Array.create lines.Length 1)
