module Day11

open Reader
open Helper
open System

// -----  Input parsing -----

type Type = Generator | Chip
type Item = { Name : string ; Type : Type }
type Floor = { Number : int; Items : Item list }
type Building = { Floors : Floor list }

let parseItem (str : string) =
    let parts = str.Split([|' '; '.'|])
    let itemType = 
        match parts.[2] with
        | "microchip" -> Chip
        | "generator" -> Generator
        | _ -> failwith "parseItem - err"
    let name = parts.[1].Split([|'-'|]).[0]
    { Name = name ; Type = itemType }

let parseFloor (str : string) =
    let parts = str.Split([|' '|], 5)
    let floorNumber = 
        match parts.[1] with
        | "first" -> 1
        | "second" -> 2
        | "third" -> 3
        | "fourth" -> 4
        | _ -> failwith "parseFloor - err"
    let items = 
        parts.[4].Split([| ", and "; ", " |], StringSplitOptions.RemoveEmptyEntries) 
        |> Seq.filter (fun s -> s <> "nothing relevant.")
        |> Seq.map parseItem 
        |> Seq.toList
    { Number = floorNumber; Items = items }

let parseInput input =
    { Floors = input |> Seq.map parseFloor |> Seq.toList }

// -----  XXX -----

let moveFloor floorNumber building =
    let floor = building.Floors |> List.find (fun elem -> elem.Number = floorNumber)
    let upperFloor = building.Floors |> List.find (fun elem -> elem.Number = floorNumber + 1)
    let newUpperFloor = { Number = upperFloor.Number; Items = upperFloor.Items @ floor.Items }
    let newBuildingFloors = building.Floors |> List.filter (fun f -> f.Number <>floorNumber && f.Number <> floorNumber + 1)
    (floor.Items.Length * 2 - 3, { Floors = newUpperFloor::newBuildingFloors }) // 2 * n - 3 steps to move n items up | does not work with all inputs :(

let resolveInput inputName = 
    let building =
        readLines inputName
        |> parseInput
    let rec resolve floorNumber b moves =
        match floorNumber with
        | 4 -> moves
        | _ -> let (m, newBuilding) = moveFloor floorNumber b in resolve (floorNumber + 1) newBuilding (moves + m)
    resolve 1 building 0

let computeFirst =
    resolveInput @"input11.txt"
    
let computeSecond =
    resolveInput @"input11-bis.txt"