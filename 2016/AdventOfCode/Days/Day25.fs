module Day25

open Reader
open Helper

let getIndex (str : string) = (int str.[0]) - (int 'a')

type Entry = Value of int64 | Register of string | None
type Instruction = Entry -> Entry -> int64[] -> int -> int64[] * int * int64
type Command = Instruction * Entry * Entry

let cpy (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, Register b -> acc |> Array.mapi (fun i elem -> if i = (getIndex b) then acc.[getIndex a] else elem), i + 1, 2L
    | Value a, Register b -> acc |> Array.mapi (fun i elem -> if i = (getIndex b) then a else elem), i + 1, 2L
    | _ -> failwith "cpy - err"

let inc (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, None -> (acc |> Array.mapi (fun i elem -> if i = (getIndex a) then elem + 1L else elem)), i + 1, 2L
    | _ -> failwith "inc - err"

let dec (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, None -> (acc |> Array.mapi (fun i elem -> if i = (getIndex a) then elem - 1L else elem)), i + 1, 2L
    | _ -> failwith "dec - err"

let jnz (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, Register b -> acc, (if acc.[getIndex a] <> 0L then i + (int acc.[getIndex b]) else i + 1), 2L
    | Value a, Register b -> acc, (if a <> 0L then i + (int acc.[getIndex b]) else i + 1), 2L
    | Register a, Value b -> acc, (if acc.[getIndex a] <> 0L then i + (int b) else i + 1), 2L
    | Value a, Value b -> acc, (if a <> 0L then i + (int b) else i + 1), 2L
    | _ -> failwith "jnz - err"

let out (x : Entry) (y : Entry) (acc : int64[]) (i : int) =
    match x, y with
    | Register a, None -> acc, i + 1, acc.[getIndex a]
    | Value a, None -> acc, i + 1, a
    | _ -> failwith "  - err"   

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
        | Regex @"out ([a-z]+)" [x] ->  out, Register x, None
        | Regex @"out ([0-9]+)" [x] ->  out, Value (int64 x), None
        | _ -> failwith "compute - err"

let compute (input : Command[]) =
    let rec doInstruction (index : int) (acc : int64[]) (output : int64 list) =
        if index >= input.Length || output.Length >= 20
            then output
            else let a, b, c = input.[index] in let (rAcc, rIndex, rOut) = a b c acc index in doInstruction rIndex rAcc (if rOut = 2L then output else rOut::output)

    doInstruction 0 [|0L; 0L; 0L; 0L|] []

let computeFirstGood (input : Command[]) =
    let rec computeOutput (input : Command[]) (i : int) =
        let newInput = Seq.append [(cpy, (Value (int64 i)), (Register "a"))] input |> Seq.toArray
        let output = compute newInput
        if output = [1L; 0L; 1L; 0L; 1L; 0L; 1L; 0L; 1L; 0L; 1L; 0L; 1L; 0L; 1L; 0L; 1L; 0L; 1L; 0L]
            then i
            else computeOutput input (i+1)
    computeOutput input 0

let computeFirst =
    readLines "input25.txt" 
    |> Seq.map parseCommand 
    |> Seq.toArray 
    |> computeFirstGood 

let computeSecond =
    1