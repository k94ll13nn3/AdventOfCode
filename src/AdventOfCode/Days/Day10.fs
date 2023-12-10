module Day10

open Container
open Helper
open System

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
    | true -> (lastV, acc)
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
    |> fst

let computeSecond () =
    let startPositionX = lines |> Array.findIndex (fun s -> s.Contains "S")
    let startPositionY = lines.[startPositionX].IndexOf 'S'

    let pointsInLoop =
        findFarthest (findStartingPoints startPositionX startPositionY ==> emptyQueue) [] 0
        |> snd

    let mutable count = 0

    for i in 1 .. lines.Length - 1 do // 1 to length - 1 because the outside cannot be in the loop
        for j in 1 .. lines.[0].Length - 1 do
            if pointsInLoop |> List.contains (i, j) |> not then
                let leftCount =
                    seq { for jj in 0 .. j - 1 -> jj }
                    // - has no impact on the point being inside or outside
                    |> Seq.filter (fun e -> lines.[i].[e] <> '-' && List.contains (i, e) pointsInLoop)
                    |> Seq.map (fun e -> lines.[i].[e])
                    |> String.Concat
                    |> (fun s -> s.Replace("F7", "")) // F7 and LF are not creating a new area
                    |> (fun s -> s.Replace("LJ", ""))
                    |> (fun s -> s.Replace("FJ", "|")) // FJ and L7 are like a | with more steps
                    |> (fun s -> s.Replace("L7", "|"))

                if leftCount.Length <> 0 && leftCount.Length % 2 = 1 then
                    count <- count + 1

    count
