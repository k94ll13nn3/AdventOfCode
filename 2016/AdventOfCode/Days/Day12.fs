module Day12

open Reader
open Helper

let getIndex (str : string) = (int str.[0]) - (int 'a')

type Entry = Value of int64 | Register of string | None
type Instruction = Entry -> Entry -> int64[] -> int -> int64[] * int
type Command = Instruction * Entry * Entry

let cpy (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, Register b -> acc |> Array.mapi (fun i elem -> if i = (getIndex b) then acc.[getIndex a] else elem), i + 1
    | Value a, Register b -> acc |> Array.mapi (fun i elem -> if i = (getIndex b) then a else elem), i + 1
    | _ -> failwith "cpy - err"

let inc (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, None -> (acc |> Array.mapi (fun i elem -> if i = (getIndex a) then elem + 1L else elem)), i + 1
    | _ -> failwith "inc - err"

let dec (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, None -> (acc |> Array.mapi (fun i elem -> if i = (getIndex a) then elem - 1L else elem)), i + 1
    | _ -> failwith "dec - err"

let jnz (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, Register b -> acc, if acc.[getIndex a] <> 0L then i + (int acc.[getIndex b]) else i + 1
    | Value a, Register b -> acc, if a <> 0L then i + (int acc.[getIndex b]) else i + 1
    | Register a, Value b -> acc, if acc.[getIndex a] <> 0L then i + (int b) else i + 1
    | Value a, Value b -> acc, if a <> 0L then i + (int b) else i + 1
    | _ -> failwith "jnz - err"

let parseCommand (str : string) =
    match str with
        | Regex @"cpy ([a-z]+) ([a-z]+)" [x; y] -> cpy, Register x, Register y
        | Regex @"cpy ([0-9]+) ([a-z]+)" [x; y] -> cpy, Value (int64 x), Register y
        | Regex @"inc ([a-z]+)" [x] -> inc, Register x, None
        | Regex @"dec ([a-z]+)" [x] -> dec, Register x, None
        | Regex @"jnz ([a-z]+) (-?[0-9]+)" [x; y] ->  jnz, Register x, Value (int64 y)
        | Regex @"jnz ([0-9]+) (-?[0-9]+)" [x; y] ->  jnz, Value (int64 x), Value (int64 y)
        | Regex @"jnz ([0-9]+) ([a-z]+)" [x; y] ->  jnz, Value (int64 x), Register y
        | Regex @"jnz ([a-z]+) ([a-z]+)" [x; y] ->  jnz, Register x, Register y
        | _ -> failwith "compute - err"

let compute (input : Command[]) =
    let rec doInstruction (index : int) (acc : int64[]) =
        if index >= input.Length
            then acc
            else let a, b, c = input.[index] in let result = a b c acc index in doInstruction (snd result) (fst result)

    doInstruction 0 [|0L; 0L; 0L; 0L|]

let computeFirst =
    readLines "input12.txt" |>  Seq.map parseCommand |> Seq.toArray |> compute |> Array.head

let computeSecond =
    readLines "input12-bis.txt" |>  Seq.map parseCommand |> Seq.toArray |> compute |> Array.head