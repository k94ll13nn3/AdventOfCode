module Day23

open Reader
open Helper

let getIndex (str : string) = (int str.[0]) - (int 'a')

type Entry = Value of int64 | Register of string | None
type Instruction = Entry -> Entry -> int64[] -> int -> int64[] * int
type Command = string * Instruction * Entry * Entry

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

let tgl (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, None -> acc, (i + (int acc.[getIndex a]))
    | _ -> failwith "tgl - err"

let parseCommand (str : string) =
    match str with
        | Regex @"cpy ([a-z]+) ([a-z]+)" [x; y] -> "cpy", cpy, Register x, Register y
        | Regex @"cpy (-?[0-9]+) ([a-z]+)" [x; y] -> "cpy", cpy, Value (int64 x), Register y
        | Regex @"inc ([a-z]+)" [x] -> "inc", inc, Register x, None
        | Regex @"dec ([a-z]+)" [x] -> "dec", dec, Register x, None
        | Regex @"jnz ([a-z]+) (-?[0-9]+)" [x; y] -> "jnz", jnz, Register x, Value (int64 y)
        | Regex @"jnz ([0-9]+) (-?[0-9]+)" [x; y] -> "jnz", jnz, Value (int64 x), Value (int64 y)
        | Regex @"jnz ([0-9]+) ([a-z]+)" [x; y] -> "jnz", jnz, Value (int64 x), Register y
        | Regex @"jnz ([a-z]+) ([a-z]+)" [x; y] -> "jnz", jnz, Register x, Register y
        | Regex @"tgl ([a-z]+)" [x] ->  "tgl", tgl, Register x, None
        | _ -> failwith "parseCommand - err"

let updateCommand (input : Command) i =
    let s, a, b, c = input in
    match s with
        | "cpy" -> "jnz", jnz, b, c
        | "jnz" -> "cpy", cpy, b, c
        | "inc" -> "dec", dec, b, c
        | "dec" -> "inc", inc, b, c
        | "tgl" -> "inc", inc, b, c
        | _ -> failwith "updateCommand - err"

let updateCommands (i : int) (input : Command[]) =
    input
    |> Array.mapi (fun ind c -> if ind <> i then c else (updateCommand c ind))

let compute (baseAcc : int64[]) (input : Command[]) =
    let rec doInstruction (index : int) (acc : int64[]) (commands : Command[]) =
        if index >= commands.Length
            then acc
            else let s, a, b, c = commands.[index] in 
                 match s with 
                 | "tgl" -> let result = a b c acc index in doInstruction (index + 1) (fst result) (commands |> updateCommands (snd result))
                 | _ -> let result = a b c acc index in doInstruction (snd result) (fst result) commands
    doInstruction 0 baseAcc input

let computeFirst =
    readLines "input23.txt" |>  Seq.map parseCommand |> Seq.toArray |> compute [|7L; 0L; 0L; 0L|] |> Array.head

let computeSecond =
    readLines "input23.txt" |>  Seq.map parseCommand |> Seq.toArray |> compute [|12L; 0L; 0L; 0L|] |> Array.head
