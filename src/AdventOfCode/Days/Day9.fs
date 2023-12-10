module Day9

open Helper

let number = 9

let title = "Mirage Maintenance"

let lines = readLines "Input9.txt" |> Array.map (splitC ' ' >> (Array.map int))

let extrapolate (values: int[]) =
    let rec extrapolateRec accStart accBack i current =
        match current |> Array.exists ((<>) 0) with
        | false -> (accStart, accBack)
        | true ->
            let op = if i % 2 = 0 then (+) else (-)

            current
            |> Array.pairwise
            |> Array.map (fun (i, j) -> j - i)
            |> extrapolateRec (op accStart current.[0]) (accBack + current.[^0]) (i + 1)

    extrapolateRec 0 0 0 values

let computeFirst () =
    lines |> Array.map extrapolate |> Array.sumBy snd

let computeSecond () =
    lines |> Array.map extrapolate |> Array.sumBy fst
