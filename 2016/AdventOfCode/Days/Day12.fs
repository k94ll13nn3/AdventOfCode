module Day12

open Reader
open Helper

let getIndex (str : string) = (int str.[0]) - (int 'a')

let compute (input : string[]) = 
    let rec doInstruction (index : int) (acc : int64[]) = 
        if index >= input.Length
            then acc 
            else match input.[index] with
                    | Regex @"cpy ([a-z]+) ([a-z]+)" [x; y] -> doInstruction (index + 1) (acc |> Array.mapi (fun i elem -> if i = (getIndex y) then acc.[getIndex x] else elem))
                    | Regex @"cpy ([0-9]+) ([a-z]+)" [x; y] -> doInstruction (index + 1) (acc |> Array.mapi (fun i elem -> if i = (getIndex y) then int64 x else elem))
                    | Regex @"inc ([a-z]+)" [x] -> doInstruction (index + 1) (acc |> Array.mapi (fun i elem -> if i = (getIndex x) then elem + 1L else elem))
                    | Regex @"dec ([a-z]+)" [x] -> doInstruction (index + 1) (acc |> Array.mapi (fun i elem -> if i = (getIndex x) then elem - 1L else elem))
                    | Regex @"jnz ([a-z]+) (-?[0-9]+)" [x; y] ->  if acc.[getIndex x] <> 0L then doInstruction (index + (int y)) acc else doInstruction (index + 1) acc
                    | Regex @"jnz ([0-9]+) (-?[0-9]+)" [x; y] ->  if (int x) <> 0 then doInstruction (index + (int y)) acc else doInstruction (index + 1) acc
                    | _ -> failwith "compute - err"

    doInstruction 0 [|0L; 0L; 0L; 0L|]

let computeFirst =
    readLines "input12.txt" |> Seq.toArray |> compute |> Array.head

let computeSecond =
    readLines "input12-bis.txt" |> Seq.toArray |> compute |> Array.head