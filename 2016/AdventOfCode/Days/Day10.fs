module Day10

open Reader
open Helper
open System

type Bot = { Number : int; Chips : int list }
type Result<'a> = Success of 'a | Error of 'a

let parseBot (s : string) =
    let splitted = s.Split([|' '|])
    { Number = int splitted.[5]; Chips = [int splitted.[1]] }

let rec concatBot (bots : Bot list) (b : Bot) =
    match bots with
    | [] -> [b]
    | h::t -> if h.Number = b.Number then { Number = h.Number; Chips = b.Chips @ h.Chips }::t else h::(concatBot t b)

let parseBots (s : seq<string>) = 
    s
    |> Seq.map parseBot
    |> Seq.fold concatBot []

let splitInput (input : seq<string>) =
    let limit = input |> Seq.findIndex (fun s -> s.StartsWith("b"))
    let split = input |> Seq.toArray
    (split.[0..limit-1] |> Seq.ofArray, split.[limit..] |> Seq.ofArray)

let getChipsToRemove (lst : int list) =
    let h = lst |> List.sort |> List.head
    let t = lst |> List.sort |> List.tail |> List.sortDescending |> List.head
    (h, t, lst |> List.sort |> List.tail |> List.sortDescending |> List.tail)

let rec takeFromBot botNumber bots =
    match bots with
    | [] -> failwith "takeFromBot - err"
    | [b] -> if b.Number = botNumber && b.Chips.Length >= 2
                then let (n, m, c) = getChipsToRemove b.Chips in (n, m, { Number = b.Number; Chips = c }::[]) 
                else failwith "takeFromBot - err"
    | h::t -> if h.Number = botNumber && h.Chips.Length >= 2
                then let (n, m, c) = getChipsToRemove h.Chips in (n, m, { Number = h.Number; Chips = c }::t) 
                else let (n, m, c) = takeFromBot botNumber t in (n, m, h::c)
    
let executeInstruction (bots : Bot list) (instr : string) =
    let splitted = instr.Split([|' '|])
    let botNumber = int splitted.[1]
    let lowTarget = splitted.[5]
    let lowTargetNumber = int splitted.[6]
    let highTarget = splitted.[10]
    let highTargetNumber = int splitted.[11]

    try
        let (lowTaken, highTaken, rem) = takeFromBot botNumber bots
        if lowTaken = 17 && highTaken = 61 then printfn "%A" botNumber // part 1 
        let firstBot = if lowTarget = "bot" then { Number = lowTargetNumber; Chips = [lowTaken]} else { Number = lowTargetNumber + 1000; Chips = [lowTaken] }
        let secondBot = if highTarget = "bot" then { Number = highTargetNumber; Chips = [highTaken]} else { Number = highTargetNumber + 1000; Chips = [highTaken] }
        Success (concatBot (concatBot rem firstBot) secondBot)
    with
        | Failure msg -> Error bots

let rec executeInstructions (bots : Bot list) (instr : string list) =
    match instr with
    | [] -> []
    | [a] -> let result = executeInstruction bots a in
                match result with
                | Success lst -> lst
                | Error lst -> bots
    | h::t -> let result = executeInstruction bots h in
                match result with
                | Success lst -> executeInstructions lst t
                | Error lst -> executeInstructions bots (t@[h])

let computeFirst =
    let input = 
        readLines @"input10.txt"
        |> Seq.sortBy (fun s -> -(int s.[0]))
        |> Seq.groupBy (fun s -> s.[0])
        |> Seq.map snd

    let bots = input |> Seq.head |> parseBots
    input |> Seq.last |> Seq.toList |> executeInstructions bots |> ignore
    
let computeSecond =
    let input = 
        readLines @"input10.txt"
        |> Seq.sortBy (fun s -> -(int s.[0]))
        |> Seq.groupBy (fun s -> s.[0])
        |> Seq.map snd

    let bots = input |> Seq.head |> parseBots
    input 
    |> Seq.last 
    |> Seq.toList 
    |> executeInstructions bots 
    |> Seq.filter (fun b -> b.Number >= 1000) 
    |> Seq.sortBy (fun b -> b.Number) 
    |> Seq.take 3 
    |> Seq.fold (fun acc b -> b.Chips.Head * acc) 1