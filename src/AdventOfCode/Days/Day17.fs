module Day17

open Container
open Helper
open System

let number = 17

let title = "Clumsy Crucible"

let lines =
    readLines "Input17.txt"
    |> Array.map (fun a -> Array.map charToInt (a |> Seq.toArray))

// N = 0, E = 1, S = 2, W = 3
let findPossibles px py dir moves heat =
    match dir with
    | 0 ->
        [ ((px, py - 1), 1, 3, heat)
          ((px, py + 1), 1, 1, heat)
          ((px - 1, py), moves + 1, 0, heat) ]
    | 1 ->
        [ ((px + 1, py), 1, 2, heat)
          ((px, py + 1), moves + 1, 1, heat)
          ((px - 1, py), 1, 0, heat) ]
    | 2 ->
        [ ((px, py - 1), 1, 3, heat)
          ((px, py + 1), 1, 1, heat)
          ((px + 1, py), moves + 1, 2, heat) ]
    | 3 ->
        [ ((px + 1, py), 1, 2, heat)
          ((px, py - 1), moves + 1, 3, heat)
          ((px - 1, py), 1, 0, heat) ]
    | _ -> failwith "error - findPossibles"

let rec findMin (acc: int) queue (mins: int[]) =
    if isEmpty queue then
        acc
    else
        let (((px, py), moves, dir, heat), rem) = take queue

        if (px < 0 || px >= lines.Length || py < 0 || py >= lines.[0].Length) then
            findMin acc rem mins
        else
            let min = mins.[px + 1000 * py]
            let newHeat = heat + lines.[px].[py]

            if moves > 3 || min <= newHeat || newHeat >= acc then
                findMin acc rem mins
            elif (px, py) = (lines.Length - 1, lines.[0].Length - 1) && (newHeat < acc) then
                findMin newHeat rem mins
            elif (px, py) = (lines.Length - 1, lines.[0].Length - 1) then
                findMin acc rem mins
            else
                let possibles = findPossibles px py dir moves newHeat
                mins.[px + 1000 * py] <- newHeat
                findMin acc (possibles ==> queue) mins


let computeFirst () =
    let moves = [ ((0, 0), 1, 2, -lines.[0].[0]); ((0, 0), 1, 1, -lines.[0].[0]) ]

    let mins =
        Array.init (lines.Length + 1000 * lines.[0].Length) (fun _ -> Int32.MaxValue)

    findMin Int32.MaxValue (moves ==> emptyStack) mins

let computeSecond () = 0
