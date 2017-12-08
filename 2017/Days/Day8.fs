module Day8

open Helper
open System

let mapOperator s =
    match s with
    | "==" -> (=)
    | ">=" -> (>=)
    | "!=" -> (<>)
    | ">" -> (>)
    | "<" -> (<)
    | "<=" -> (<=)
    | _ -> failwith "mapOperator - error"

let compute (s:string) (acc, m) =
    let getValue map a = (if map |> Map.containsKey a then map.[a] else 0)
    match s with
    | Regex @"(\w+) (\w+) (-?\d+) if (\w+) ([=<>!]+) (-?\d+)" [a; b; c; d; e; f] ->
        if f |> int |> (mapOperator e) (getValue acc d)  then
            let newValue = c |> int |> (if b = "inc" then (~+) else (~-)) |> (+) (getValue acc a)
            (acc |> Map.remove a |> Map.add a newValue, max newValue m)
        else
            (acc, m)
    | _ -> failwith "compute - error"

let computeFirst =
    readLines "Input8.txt"
    |> Seq.fold (fun acc s -> compute s acc) (Map.empty, Int32.MinValue)
    |> fst
    |> Map.fold (fun acc _ i -> max acc i) Int32.MinValue

let computeSecond =
   readLines "Input8.txt"
    |> Seq.fold (fun acc s -> compute s acc) (Map.empty, Int32.MinValue)
|> snd