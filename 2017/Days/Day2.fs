module Day2

open Helper
open System

let stringToInts (s:string) =
    s.Split([|'\t'|], StringSplitOptions.RemoveEmptyEntries) 
    |> Array.map (int)

let maxMinusMin arr = (arr |> Array.max) - (arr |> Array.min)

let rec find2 n arr =
    match arr |> List.tryFind (fun e -> n % e = 0) with
    | Some i -> n / i
    | None -> 0

let rec findEvenlyDivision arr =
    match arr with
    | h::t -> if (find2 h t) <> 0 then (find2 h t) else findEvenlyDivision t
    | _ -> 0

let computeFirst =
    readLines "Input2.txt"
    |> Array.sumBy (stringToInts >> maxMinusMin)

let computeSecond =
    readLines "Input2.txt"
    |> Array.sumBy (stringToInts >> Array.sortDescending >> Array.toList >> findEvenlyDivision) 
