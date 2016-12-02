module Day1

open Reader
open Helper
open System
open Types

let distance p = 
    (abs p.X) + (abs p.Y)

type Direction = N | S | W | E
type Turn = R | L

let toTurn c =
    fromString<Turn> (string c) 

let getDirection t d =
    match t, d with 
    | L, N -> W
    | R, N -> E
    | L, S -> E
    | R, S -> W
    | L, W -> S
    | R, W -> N
    | L, E -> N
    | R, E -> S

type Movement = { turn: Turn; length: int }
let newMovement d l = { turn = toTurn d; length = int l}
let parseMovement (a : string) = newMovement a.[0]  a.[1..]
let parseMovements (a : string) = 
    a.Split([|", "|], StringSplitOptions.None)
    |> Array.toList
    |> List.map parseMovement

let getNextPoint p d t =
    match getDirection t d with 
    | N -> fun l -> {X = p.X; Y = p.Y + l}
    | S -> fun l -> {X = p.X; Y = p.Y - l}
    | W -> fun l -> {X = p.X - l; Y = p.Y}
    | E -> fun l -> {X = p.X + l; Y = p.Y}

let move (p, d) (m) = (getNextPoint p d m.turn m.length, getDirection m.turn d)

let moveUntilHQ movements =
    let rec innerMove (movements : Movement list) p d (acc : Point list) =
        let m = movements.Head
        let nextMove = move (p, d) m
        let visitedPoints = [1..m.length] |> List.map (getNextPoint p d m.turn)
        match intersect visitedPoints acc with
        | h::t -> h
        | [] -> innerMove movements.Tail (fst nextMove) (snd nextMove) (List.append visitedPoints acc)
    innerMove movements {X = 0; Y = 0} N []

let computeFirst =
    readInput @"input1.txt"
    |> parseMovements 
    |> Seq.fold move ({X = 0; Y = 0} , N) 
    |> fst
    |> distance

let computeSecond =
    readInput @"input1.txt"
    |> parseMovements 
    |> moveUntilHQ
    |> distance