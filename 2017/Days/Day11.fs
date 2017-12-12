module Day11

open Helper

let getNextPosition position move =
    match move with
    | "n" -> (fst position + 1.0, snd position)
    | "ne" -> (fst position + 0.5, snd position + 0.5)
    | "nw" -> (fst position + 0.5, snd position - 0.5)
    | "s" -> (fst position - 1.0, snd position)
    | "se" -> (fst position - 0.5, snd position + 0.5)
    | "sw" -> (fst position - 0.5, snd position - 0.5)
    | _ -> failwith "getNextPosition - error"

let distance (a, b) = (+) (abs a)  (abs b) |> int

let computeFirst =
    (readInput "Input11.txt").Split ','
    |> Array.fold (fun (position, m) e -> let p = getNextPosition position e in (p, (max m (distance p)))) ((0.0, 0.0), 0)
    |> fst
    |> distance

let computeSecond =
    (readInput "Input11.txt").Split ','
    |> Array.fold (fun (position, m) e -> let p = getNextPosition position e in (p, (max m (distance p)))) ((0.0, 0.0), 0)
    |> snd


let rotate (l:int) (arr: 'a[]) = let l1 = arr.Length in arr |> Array.mapi (fun i _ -> arr.[(l1+i+l)%l1])
let reverse (l:int) (arr: 'a[]) = Array.append (arr.[0..l-1] |> Array.rev) arr.[l..]
let rotrev e f = rotate e >> reverse f >> rotate -e
let t = [|1;2;3;4;5;6|] |> rotrev 1 5