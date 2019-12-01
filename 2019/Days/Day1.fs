module Day1

open Helper

let rec getFuel acc e =
    if e < 9 then
        acc
    else
        let f = e / 3 - 2
        getFuel (acc + f) f

let computeFirst =
    let ls = readLines "Input1.txt"
    ls |> Array.sumBy (fun e -> ((int e) / 3) - 2)

let computeSecond =
    let ls = readLines "Input1.txt"
    ls |> Array.sumBy (int >> getFuel 0)
