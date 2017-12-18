module Day18

open Helper
open Types

let getIndex (str : string) = (int str.[0]) - (int 'a')

let (|Operator|_|) (acc : int64[]) (input : string) =
    match input with
    | Regex @"(\w+) ([a-z]+) (-?[0-9]+)" [op; x; y] -> Some (op, getIndex x, int64 y)
    | Regex @"(\w+) ([a-z]+) ([a-z]+)" [op; x; y] -> Some (op, getIndex x, acc.[getIndex y])
    | _ -> None

let compute1 (input : string[]) = 
    let rec doInstruction (index : int) (acc : int64[]) lastSound = 
        if index >= input.Length then 
            lastSound 
        else 
            match input.[index] with
                | Regex @"snd ([a-z]+)" [x] -> doInstruction (index + 1) acc (acc.[getIndex x])
                | Regex @"rcv (-?[0-9]+)" [x] -> if int x <> 0 then lastSound else doInstruction (index + 1) acc lastSound
                | Regex @"rcv ([a-z]+)" [x] -> if acc.[getIndex x] <> 0L then lastSound else doInstruction (index + 1) acc lastSound
                | Regex @"jgz (-?[0-9]+) (-?[0-9]+)" [x; y] -> doInstruction (if int x > 0 then index + (int y) else index + 1) acc lastSound
                | Operator acc (operator, x, y) -> 
                    match operator with
                    | "set" -> doInstruction (index + 1) (acc |> Array.copySet x y) lastSound
                    | "add" -> doInstruction (index + 1) (acc |> Array.copySet x (acc.[x] + y)) lastSound
                    | "mul" -> doInstruction (index + 1) (acc |> Array.copySet x (acc.[x] * y)) lastSound
                    | "mod" -> doInstruction (index + 1) (acc |> Array.copySet x (acc.[x] % y)) lastSound
                    | "jgz" -> doInstruction (index + (if acc.[x] > 0L then int y else 1)) acc lastSound
                    | _ -> failwith "compute - err"
                | _ -> failwith "compute - err"
    doInstruction 0 (Array.zeroCreate 26) 0L

let compute2 (input : string[]) = 
    let move index (acc : int64[]) dataS dataR b c =
        match input.[index] with
            | Regex @"snd (-?[0-9]+)" [x] -> ((index + 1), (acc), int64 x --> dataS, dataR, (if b then c + 1 else c), false)
            | Regex @"snd ([a-z]+)" [x] -> ((index + 1), (acc), acc.[getIndex x] --> dataS, dataR, (if b then c + 1 else c), false)
            | Regex @"jgz (-?[0-9]+) (-?[0-9]+)" [x; y] -> ((if int x > 0 then index + (int y) else index + 1), (acc), dataS, dataR, c, false)
            | Regex @"rcv ([a-z]+)" [x] -> let t = peek dataR in if t.IsNone then (index, acc, dataS, dataR, c, true) else (index + 1, (acc |> Array.copySet (getIndex x) t.Value), dataS, (dataR |> take |> snd), c, false)
            | Operator acc (operator, x, y) -> 
                match operator with
                | "set" -> (index + 1, acc |> Array.copySet x y, dataS, dataR, c, false)
                | "add" -> (index + 1, acc |> Array.copySet x (acc.[x] + y), dataS, dataR, c, false)
                | "mul" -> (index + 1, acc |> Array.copySet x (acc.[x] * y), dataS, dataR, c, false)
                | "mod" -> (index + 1, acc |> Array.copySet x (acc.[x] % y), dataS, dataR, c, false)
                | "jgz" -> (index + (if acc.[x] > 0L then int y else 1), acc, dataS, dataR, c, false)
                | _ -> failwith "compute - err"
            | _ -> failwith "compute - err"
    let rec doInstruction (index1 : int) (index2 : int) (acc1 : int64[]) (acc2 : int64[]) (data1:Container<int64>) (data2:Container<int64>) numberOfValueSended = 
        // do not need to compare indexes with input.Length because deadlock is more fun !!
        let (a1, b1, c1, d1, _, isProgram0Waiting) = move index1 acc1 data1 data2 false 0
        let (a2, b2, c2, d2, newNumberOfValueSended, isProgram1Waiting) = move index2 acc2 d1 c1 true numberOfValueSended // data send from 0 become data to read for 1
        if isProgram0Waiting && isProgram1Waiting then 
            newNumberOfValueSended
        else
            doInstruction a1 a2 b1 b2 d2 c2 newNumberOfValueSended 
    doInstruction 0 0 (Array.zeroCreate 26) (Array.zeroCreate 26 |> Array.copySet (getIndex "p") 1L) emptyQueue emptyQueue 0

let computeFirst =
    readLines "Input18.txt" |> Seq.toArray |> compute1

let computeSecond =
    readLines "Input18.txt" |> Seq.toArray |> compute2