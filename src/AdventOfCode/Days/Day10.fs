module Day10

open Container
open Helper

let number = 10

let title = "Pipe Maze"

let lines = readLines "Input10.txt"

type Direction =
    | N
    | S
    | W
    | E

let rec findFarthest (container: Container<((int * int) * int)>) acc lastV =
    match isEmpty container with
    | true -> lastV
    | false ->
        let (((x, y), v), cont) = take container

        let newPoints =
            match lines.[x].[y] with
            | '-' -> [ (x, y - 1); (x, y + 1) ]
            | '|' -> [ (x + 1, y); (x - 1, y) ]
            | 'L' -> [ (x - 1, y); (x, y + 1) ]
            | 'J' -> [ (x - 1, y); (x, y - 1) ]
            | 'F' -> [ (x + 1, y); (x, y + 1) ]
            | '7' -> [ (x + 1, y); (x, y - 1) ]
            | _ -> []
            |> List.filter (fun p -> acc |> List.contains p |> not)

        match newPoints with
        | [ p1 ] -> findFarthest ((p1, v + 1) --> cont) (p1 :: acc) v
        | [ p1; p2 ] -> findFarthest ([ (p1, v + 1); (p2, v + 1) ] ==> cont) (p1 :: p2 :: acc) v
        | _ -> findFarthest cont acc v

// Not counting boundaries, but only neeeds to add . all around to the input if needed
let findStartingPoints startPositionX startPositionY =
    let north = lines.[startPositionX - 1].[startPositionY]
    let south = lines.[startPositionX + 1].[startPositionY]
    let west = lines.[startPositionX].[startPositionY - 1]
    let east = lines.[startPositionX].[startPositionY + 1]

    [ ((startPositionX - 1, startPositionY), List.contains north [ '|'; '7'; 'F' ])
      ((startPositionX + 1, startPositionY), List.contains south [ '|'; 'L'; 'J' ])
      ((startPositionX, startPositionY - 1), List.contains west [ '-'; 'L'; 'F' ])
      ((startPositionX, startPositionY + 1), List.contains east [ '-'; '7'; 'J' ]) ]
    |> List.filter snd
    |> List.map (fun (points, _) -> (points, 1))

let computeFirst () =
    let startPositionX = lines |> Array.findIndex (fun s -> s.Contains "S")
    let startPositionY = lines.[startPositionX].IndexOf 'S'

    findFarthest (findStartingPoints startPositionX startPositionY ==> emptyQueue) [] 0

let computeSecond () = 0
