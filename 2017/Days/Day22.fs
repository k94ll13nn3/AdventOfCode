module Day22

open Helper

type Direction = U | D | R | L

let turnRight = function | U -> R | R -> D | D -> L | L -> U
let turnLeft = function | U -> L | L -> D | D -> R | R -> U
let opposite = function | U -> D | L -> R | D -> U | R -> L
let getNext (p1, p2) = function | U -> (p1 - 1, p2) | D -> (p1 + 1, p2) | R -> (p1, p2 + 1) | L -> (p1, p2 - 1)

let createInfiniteArray infiniteSize = 
    let a =
        readLines "Input22.txt"
        |> Array.map (fun s -> s.ToCharArray())
        |> array2D 
    let l = a |> Array2D.length1
    let b = Array2D.init infiniteSize infiniteSize (fun _ _ -> '.')
    Array2D.blit a 0 0 b (infiniteSize/2 - l/2) (infiniteSize/2 - l/2) l l
    (b, (infiniteSize/2, infiniteSize/2))

let computeStep (map:char[,]) ((p1,p2) as p) direction infectedBursts =
    if map.[p1,p2] = '#' then 
        let d =  turnRight direction
        map.[p1,p2] <- '.'
        (map, getNext p d, d, infectedBursts)
    else    
        let d =  turnLeft direction
        map.[p1,p2] <- '#'
        (map, getNext p d, d, infectedBursts + 1)

let computeStep2 (map:char[,]) ((p1,p2) as p) direction infectedBursts =
    match map.[p1,p2] with
    | '.' -> 
        let d =  turnLeft direction
        map.[p1,p2] <- 'W'
        (map, getNext p d, d, infectedBursts)
    | 'W' -> 
        map.[p1,p2] <- '#'
        (map, getNext p direction, direction, infectedBursts + 1)
    | '#' -> 
        let d =  turnRight direction
        map.[p1,p2] <- 'F'
        (map, getNext p d, d, infectedBursts)
    | 'F' -> 
        let d =  opposite direction
        map.[p1,p2] <- '.'
        (map, getNext p d, d, infectedBursts)
    | _ -> failwith "computeStep2 - error"

let computeFirst =
    let map, p = createInfiniteArray 10001
    Seq.init 10000 id
    |> Seq.fold (fun acc _ -> let (a, b, c, d) = acc in computeStep a b c d) (map, p, U, 0)
    |> fun (_,_,_,e) -> e

let computeSecond =
    let map, p = createInfiniteArray 10001
    Seq.init 10000000 id
    |> Seq.fold (fun acc _ -> let (a, b, c, d) = acc in computeStep2 a b c d) (map, p, U, 0)
    |> fun (_,_,_,e) -> e