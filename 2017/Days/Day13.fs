module Day13

open Helper

type Layer = {Depth: int; Range: int; Scanner: int; IsActive: bool}
let emptyLayer i = {Depth = i; Range = 0; Scanner = 0; IsActive = false}
let isZero layer i = layer.IsActive && (i % (2 * (layer.Range - 1))) = 0

let parseLine s =
    match s with
    | Regex @"(\d+): (\d+)" [a; b] -> {Depth = int a; Range = int b; Scanner = 0; IsActive = true}
    | _ -> failwith "parseLine - error" 

// if range = 4, scanner will be at 0 when it is at 0, 6, ...:
// 0,1,2,3,4,5,6,7,8
// 0,1,2,3,2,1,0,1,2
// so we must check for (range-1)*2
let compute startingPosition (a:Layer[]) =
    let rec computeRec ind caught =
        match ind with
        | _ when ind >= a.Length -> caught 
        | _ when isZero a.[ind] (startingPosition + ind) -> computeRec (ind + 1) (a.[ind]::caught)
        | _ -> computeRec (ind + 1) caught
    computeRec 0 []

let input =
    let layers = readLines "Input13.txt" |> Seq.map parseLine
    let maxDepthLayer = layers |> Seq.maxBy (fun e -> e.Depth)
    Array.init (maxDepthLayer.Depth + 1) (fun ind -> layers |> Seq.tryFind (fun e -> e.Depth = ind) |> (fun s -> defaultArg s (emptyLayer ind)))

let computeFirst =
    input
    |> compute 0
    |> List.sumBy (fun l -> l.Depth * l.Range)

let computeSecond =
    Seq.initInfinite id |> Seq.find (fun i -> compute i input |> List.isEmpty)