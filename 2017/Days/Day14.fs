module Day14

open System
open Types

let baseInput = "amgozmfv"

// DAY 10 ----------------

let rotate (l:int) (arr: 'a[]) = let l1 = arr.Length in arr |> Array.mapi (fun i _ -> arr.[(l1+i+l)%l1])
let reverse (l:int) (arr: 'a[]) = Array.append (arr.[0..l-1] |> Array.rev) arr.[l..]
let computeRotation (length:int) (index:int) = rotate index >> reverse length >> rotate -index

let compute l =
    let rec computeRec (skip:int) (index:int) (l:int list) (hash:int[]) = 
        match l with
        | [] -> hash
        | h::t -> computeRotation h index hash |> computeRec (skip+1) ((index + skip + h) % hash.Length) t
    computeRec 0 0 (l |> Seq.toList) (Array.init 256 id)

let computeHash (input:string) =
    input.ToCharArray()
    |> Array.map int
    |> (fun e -> Array.append e [|17; 31; 73; 47; 23|])
    |> (fun e -> Seq.init 64 id |> Seq.fold (fun acc _ -> Array.append acc e) [||])
    |> compute
    |> Array.chunkBySize 16
    |> Array.map (Array.reduce (^^^) >> (fun e -> Convert.ToString(e, 2).PadLeft(8, '0')))
    |> Array.reduce (+)
    |> (fun e -> e.ToCharArray())

// DAY 10 ----------------

let tryGet (map:'a[][]) x y =
    match x >= 0 && x < map.Length && y >= 0 && y < map.[0].Length with
    | true -> Some map.[x].[y]
    | false -> None

let isValid (map : char[][]) (visited : (int * int) list) ((p1, p2) as p: int * int ) =
    tryGet map p1 p2 = Some '1' && (visited |> List.exists ((=) p) |> not)

let rec findNeighboors (map:char[][]) (q: Container<int * int>) (visited : (int * int) list) =
    match isEmpty q with 
    | true -> visited
    | false -> 
        let ((i1, i2), newQ) = q |> take
        let neighboors = [(i1, i2-1); (i1, i2+1); (i1-1, i2); (i1+1, i2)] |> List.filter (isValid map visited)   
        findNeighboors map (neighboors ==> newQ) (neighboors@visited)

let removeRegion i (map:char[][]) = 
    let neighboors = findNeighboors map (i --> emptyQueue) [i]
    map
    |> Array.mapi (fun i e -> e |> Array.mapi (fun j f -> if neighboors |> List.exists ((=) (i, j)) then '0' else f))

let indexOf1 map =
    let rec indexOf1Rec i (map:char[][]) =
        if i >= map.Length then 
            None
        else
            let index = map.[i] |> Array.tryFindIndex ((=) '1')
            match index with
            | Some n -> Some (i, n)
            | None -> indexOf1Rec (i + 1) map 
    indexOf1Rec 0 map

let computeRegions map =
    let rec computeRegionsRec acc map =
        match indexOf1 map with
        | None -> acc
        | Some n -> removeRegion n map |> computeRegionsRec (acc + 1)
    computeRegionsRec 0 map

let computeFirst =
    Seq.init 128 (sprintf "%s-%d" baseInput >> computeHash)
    |> Seq.fold (fun acc h -> h |> Seq.filter ((=) '1') |> Seq.length |> (+) acc) 0

let computeSecond =
    (sprintf "%s-%d" baseInput >> computeHash) |> Array.init 128 |> computeRegions