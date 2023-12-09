module Day9

open Helper

let number = 9

let title = "Mirage Maintenance"

let lines = readLines "Input9.txt" |> Array.map (splitC ' ' >> (Array.map int))

let extrapolate (values: int[]) =
    let rec extrapolateRec current acc =
        match current |> Array.exists ((<>) 0) with
        | false -> acc
        | true ->
            let newValues = current.[1..] |> Array.mapi (fun i v -> v - current.[i])
            extrapolateRec newValues (acc + current.[^0])

    extrapolateRec values 0

let extrapolateStart (values: int[]) =
    let rec extrapolateRec current acc i =
        match current |> Array.exists ((<>) 0) with
        | false -> acc
        | true ->
            let newValues = current.[1..] |> Array.mapi (fun i v -> v - current.[i])
            let op = if i % 2 = 0 then (+) else (-)
            extrapolateRec newValues (op acc current.[0]) (i + 1)

    extrapolateRec values 0 0

let computeFirst () =
    lines |> Array.map extrapolate |> Array.sum

let computeSecond () =
    lines |> Array.map extrapolateStart |> Array.sum
