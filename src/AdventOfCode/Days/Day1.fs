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

let replaceNumber (input: string) =
    input
        .Replace("one", "1")
        .Replace("two", "2")
        .Replace("three", "3")
        .Replace("four", "4")
        .Replace("five", "5")
        .Replace("six", "6")
        .Replace("seven", "7")
        .Replace("eight", "8")
        .Replace("nine", "9")

let rec replaceNumberInString (acc: string) (input: string) : string =
    if input.Length = 0 then
        replaceNumber acc
    elif
        acc.Contains("one")
        || acc.Contains("two")
        || acc.Contains("three")
        || acc.Contains("four")
        || acc.Contains("five")
        || acc.Contains("six")
        || acc.Contains("seven")
        || acc.Contains("eight")
        || acc.Contains("nine")
    then
        replaceNumberInString (String.Concat(replaceNumber acc, input.[0])) (input.[1..])
    else
        replaceNumberInString (String.Concat(acc, input.[0])) (input.[1..])

let computeFirst () =
    readLinesSeq "Input1.txt" |> Seq.map extractDigit |> Seq.sum

let computeSecond () =
    readLinesSeq "Input1.txt"
    |> Seq.map (replaceNumberInString "" >> extractDigit)
    |> Seq.sum
