module Day1

open Helper

let number = 1

let title = "Calorie Counting"

[<TailCall>]
let compute (input: string list) =
    let rec computeRec (currentAcc: int) (listAcc: int list) (remainingInput: string list) =
        match remainingInput with
        | [] -> listAcc
        | h :: t ->
            match h with
            | null
            | "" -> computeRec 0 (currentAcc :: listAcc) t
            | _ -> computeRec (currentAcc + (int h)) listAcc t

    computeRec 0 [] input

let elves = readLines "Input1.txt" |> compute

let computeFirst () = elves |> List.max

let computeSecond () =
    elves |> List.sortByDescending id |> List.take 3 |> List.sum
