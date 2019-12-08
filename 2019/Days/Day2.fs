module Day2

open Helper

let rec compute pos (acc: int []) =
    if acc.[pos] = 99 then
        acc.[0]
    else
        let op = acc.[pos]
        let right = acc.[acc.[pos + 1]]
        let left = acc.[acc.[pos + 2]]
        acc.[acc.[pos + 3]] <- if op = 1 then right + left
                               else right * left
        compute (pos + 4) acc

let computeFirst =
    (readInput "Input2.txt").Split ','
    |> Array.map int
    |> compute 0

let computeSecond =
    let input = (readInput "Input2.txt").Split ',' |> Array.map int

    let pairs =
        seq {
            for i in 0 .. 99 do
                for j in 0 .. 99 -> (compute 0 (input |> Array.copySet2 1 i 2 j), i, j)
        }

    let (_, noun, verb) = pairs |> Seq.find (fun (r, _, _) -> r = 19690720)

    100 * noun + verb
 