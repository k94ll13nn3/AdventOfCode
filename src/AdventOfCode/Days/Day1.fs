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

    (getFirstDigit 0 (+)) * 10 + (getFirstDigit (input.Length - 1) (-))


let replaceNumbers (input: string) =
    let replaceNumber (input: string) =
        input
            .Replace("one", "1e")
            .Replace("two", "2o")
            .Replace("three", "3e")
            .Replace("four", "4")
            .Replace("five", "5e")
            .Replace("six", "6")
            .Replace("seven", "7n")
            .Replace("eight", "8t")
            .Replace("nine", "9e")

    let rec replaceNumberInString (acc: string) (input: string) =
        if input.Length = 0 then
            replaceNumber acc
        else
            replaceNumberInString (String.Concat(replaceNumber acc, input.[0])) (input.[1..])

    replaceNumberInString "" input

let computeFirst () =
    readLinesSeq "Input1.txt" |> Seq.map extractDigit |> Seq.sum

let computeSecond () =
    readLinesSeq "Input1.txt" |> Seq.map (replaceNumbers >> extractDigit) |> Seq.sum
