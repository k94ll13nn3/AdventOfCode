module Day16

open Helper
open System.Collections.Generic
open System.Linq

let number = 16

let title = "The Floor Will Be Lava"

let lines = readLines "Input16.txt"

// N = 0, E = 1, S = 2, W = 3
let getNextPoint (x, y) dir =
    match dir with
    | 0 -> (x - 1, y)
    | 2 -> (x + 1, y)
    | 1 -> (x, y + 1)
    | 3 -> (x, y - 1)
    | _ -> failwith "error getNextPoint"

let compute (ix, iy) idir =
    let acc = new HashSet<((int * int) * int)>()

    let rec energize (x, y) dir =
        if acc.Contains(((x, y), dir)) then
            Some 0
        elif x < 0 || x >= lines.Length || y < 0 || y >= lines.[0].Length then
            None
        else
            acc.Add((x, y), dir) |> ignore

            match (lines.[x].[y], dir) with
            | ('|', 1)
            | ('|', 3) ->
                energize (getNextPoint (x, y) 0) 0 |> ignore
                energize (getNextPoint (x, y) 2) 2 |> ignore
                None
            | ('-', 0)
            | ('-', 2) ->
                energize (getNextPoint (x, y) 1) 1 |> ignore
                energize (getNextPoint (x, y) 3) 3 |> ignore
                None
            | ('/', _) ->
                let newDir =
                    match dir with
                    | 0 -> 1
                    | 1 -> 0
                    | 2 -> 3
                    | 3 -> 2
                    | _ -> failwith "error /"

                energize (getNextPoint (x, y) newDir) newDir
            | ('\\', _) ->
                let newDir =
                    match dir with
                    | 0 -> 3
                    | 1 -> 2
                    | 2 -> 1
                    | 3 -> 0
                    | _ -> failwith "error /"

                energize (getNextPoint (x, y) newDir) newDir
            | ('|', 0)
            | ('|', 2)
            | ('-', 3)
            | ('-', 1)
            | ('.', _) -> energize (getNextPoint (x, y) dir) dir
            | _ -> failwith "error let rec energize (x, y) dir acc ="

    energize (ix, iy) idir |> ignore
    acc.DistinctBy(fst).Count()


let computeFirst () = compute (0, 0) 1

let computeSecond () =
    seq {
        for i in 0 .. lines.Length - 1 do
            yield!
                seq {
                    ((i, 0), 1)
                    ((i, lines.[0].Length - 1), 3)
                }

        for j in 0 .. lines.[0].Length - 1 do
            yield!
                seq {
                    ((0, j), 2)
                    ((lines.Length - 1, j), 0)
                }
    }
    |> Seq.map (fun (x, d) -> compute x d)
    |> Seq.max
