module Day24

open System
open Reader
open Helper
open Types

type Cell = 
    | Wall
    | Floor
    | Location of int

let parseCell c =
    match c with 
    | '.' -> Floor
    | '#' -> Wall
    | a -> Location((int)a - (int)'0')

let parseInput lines =
    let a = lines 
            |> Seq.map (fun l -> Seq.map parseCell l |> Seq.toArray) 
            |> Seq.toArray
    let b = a
            |> Array.map (fun l -> Array.mapi (fun ind c -> (ind, c)) l |> Array.filter (fun (_, c) -> match c with Location(x) -> true | _ -> false))
            |> Array.mapi (fun ind c -> Array.map (fun (b, x) -> ({X = ind; Y = b}, x)) c)
            |> Array.fold (fun acc elem -> (Array.fold (fun acc1 elem1 -> elem1::acc1) [] elem)@acc) []
    (Array2D.init (a.Length) (a.[0].Length) (fun i j -> a.[i].[j]), b)

let isValid (map : Cell[,]) (visited : Point list) (p : Point) =
    let c = map.[p.X, p.Y]
    match c with
    | Wall -> false
    | _ -> visited |> Seq.exists (fun elem -> elem = p) |> not

let findNeighboors (p : Point) (map : Cell[,]) (visited : Point list) =
    let (up, down, left, right) = ({X = p.X; Y = p.Y - 1}, {X = p.X; Y = p.Y + 1}, {X = p.X - 1; Y = p.Y}, {X = p.X + 1; Y = p.Y})
    [ up; down; left; right ] |> List.filter (isValid map visited)

let distance (startPoint : Point) (endPoint : Point) (map : Cell[,]) =
    let rec findMinimumDistance (cont : Container<int *  Point>) (visited : Point list) =
        let aaa = visited.Length
        let ((d, p), rest) = take cont in
        if p.X = endPoint.X && p.Y = endPoint.Y
            then d
            else let neighboors = (findNeighboors p map visited) in findMinimumDistance ((neighboors |> List.map (fun x -> d + 1, x)) ++> rest) (visited@neighboors)
    findMinimumDistance ((0, startPoint) --> emptyQueue) []

let computeAllDistance2 start coord map =
    coord |> List.fold (fun acc elem -> (start, elem, (distance start elem map))::acc) []

let rec computeAllDistance (coord : (Point * Cell) list) (map : Cell[,]) =
    match coord with
    | (h, _)::t -> (computeAllDistance2 h (t |> List.map (fst)) map)@(computeAllDistance t map)
    | [] -> []

let distanceBetween (startPoint : Point) (endPoint : Point) (distanceMap : (Point * Point * int) list) =
    let (_, _, d) = distanceMap 
                    |> List.find (fun (a, b, c) -> (a = startPoint && b = endPoint) || (a = endPoint && b = startPoint))
    in d 

let computeDistanceOfPermutation (distanceMap : (Point * Point * int) list) (path : Point list) =
    let h::t = path in
    t 
    |> List.fold (fun acc a -> ((fst acc) + (distanceBetween (snd acc) a distanceMap), a)) (0, h)
    |> fst

let computeFirst =
    let (map, coord) = readLines @"input24.txt" |> parseInput in
    let distanceMap = computeAllDistance coord map
    coord
    |> List.permutations
    |> List.filter (fun ((_, h)::t) -> match h with Location(x) when x = 0 -> true | _ -> false)
    |> List.map (List.map fst)
    |> List.map (fun c -> computeDistanceOfPermutation distanceMap c)
    |> List.min

let computeSecond =
    let (map, coord) = readLines @"input24.txt" |> parseInput in
    let distanceMap = computeAllDistance coord map
    coord
    |> List.permutations
    |> List.filter (fun ((_, h)::t) -> match h with Location(x) when x = 0 -> true | _ -> false)
    |> List.map (List.map fst)
    |> List.map (fun (ch::ct) -> (ch::ct)@[ch])
    |> List.map (fun c -> computeDistanceOfPermutation distanceMap c)
    |> List.min
