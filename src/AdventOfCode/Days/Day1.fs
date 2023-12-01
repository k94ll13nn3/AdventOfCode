module Day1

open Helper
open System

let number = 1

let title = "Trebuchet?!"

let extractDigit (input: string) =
    let rec getFirstDigit (index: int) (op) =
        match input[index] with
        | i when Char.IsDigit i -> charToInt i
        | _ -> getFirstDigit (op index 1) op

    getFirstDigit 0 (+) |> (*) 10 |> (+) (getFirstDigit (input.Length - 1) (-))

let replaceNumbers (input: string) =
    // 1e, 2o, ... are for cases like oneight, to replace to 1eight
    let replacements =
        Map
            [ "one", "1e"
              "two", "2o"
              "three", "3e"
              "four", "4"
              "five", "5e"
              "six", "6"
              "seven", "7n"
              "eight", "8t"
              "nine", "9e" ]

    let replaceNumber (input: string) =
        replacements
        |> Map.fold (fun (state: string) key value -> state.Replace(key, value)) input

    let rec replaceNumberInString (acc: string) (input: string) =
        match input.Length with
        | 0 -> replaceNumber acc
        | _ -> replaceNumberInString (String.Concat(replaceNumber acc, input.[0])) (input.[1..])

    replaceNumberInString "" input

let computeFirst () =
    readLinesSeq "Input1.txt" |> Seq.map extractDigit |> Seq.sum

let computeSecond () =
    readLinesSeq "Input1.txt" |> Seq.map (replaceNumbers >> extractDigit) |> Seq.sum
