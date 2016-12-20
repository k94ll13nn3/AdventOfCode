module Day20

open System
open Helper
open Reader

let merge (a : uint32, b : uint32) (c, d) =
    if a = c then (a, max b d)
    else if b = d then (min a c, d)
    else if a <= c && b >= c then (a, d)
    else if a <= d && b >= d then (c, b)
    else if b = c - 1u then (a, d)
    else failwith "merge - err"

let isIn (a : uint32, b : uint32) (c, d) = a <= c && d <= b

let intersect (a : uint32, b : uint32) (c, d) = b + 1u >= c

let rec insert (acc : (uint32*uint32) list) (a : uint32, b : uint32)  =
    match acc with
    | [] -> [(a, b)]
    | [(c, d)] when c <= a -> if (a, b) |> isIn (c, d)  then [(c, d)] else if intersect (c, d) (a, b) then [merge (c, d) (a, b)] else [(a, b);(c, d)]
    | [(c, d)] -> if (c, d) |> isIn (a, b)  then [(a, b)] else if intersect (a, b) (c, d) then [merge (a, b) (c, d)] else [(a, b);(c, d)]
    | (c, d)::t when c <= a -> if (a, b) |> isIn (c, d)  then (c, d)::t else if intersect (c, d) (a, b) then (merge (c, d) (a, b))::t else (c, d)::(insert t (a, b))
    | (c, d)::t -> if (c, d) |> isIn (a, b)  then (a, b)::t else if intersect (a, b) (c, d) then (merge (a, b) (c, d))::t else (c, d)::(insert t (a, b))

let compute =
    readLines @"input20.txt"
    |> Seq.map ((fun s -> s.Split([|'-'|])) >> (fun a -> uint32 a.[0], uint32 a.[1]))
    |> Seq.sort
    |> Seq.fold insert []
    |> List.sort

let computeFirst =
    compute
    |> List.head
    |> snd
    |> (+) 1u

let computeSecond =
    compute
    |> List.sort
    |> List.fold (fun (a, b) elem -> (snd elem, (fst elem) - a - 1u + b)) (0u, 1u) // (y last - x current) - 1 because for example (.., 22) (23, ..) has no IP between and we start at 1 to have the first fold return 0
    |> snd