module Day24

open Helper
open Types

type Component = {X : int; Y : int}

let parse s = 
    match s with
    | Regex "(\d+)/(\d+)" [a; b] -> {X = int a; Y = int b}
    | _ -> failwith "parse - error"

let getBridge start distance components size =
    let filteredComponents = components |> List.filter (fun x -> x.X = start || x.Y = start)
    if List.isEmpty filteredComponents then
        None
    else
        filteredComponents 
        |> List.map (fun e -> ((if e.X = start then e.Y else e.X), distance + e.X + e.Y, components |> List.except [e], size + 1) )
        |> Some

let compute testSize input =
    let rec computeRec queue (maxDistance, maxSize) =
        let p = peek queue
        if p.IsNone then
            maxDistance
        else   
            let ((a, b, c, d), q) = take queue     
            let newBridges = getBridge a b c d
            if newBridges.IsNone then     
                computeRec (q) (maxDistance, maxSize)
            else
                let (_, newD, _, newS) = newBridges.Value |> List.maxBy (fun (_, dist, _, _) -> dist)
                if testSize && newS > maxSize then
                    computeRec (newBridges.Value ==> q) (newD, newS)
                elif testSize && newS = maxSize then
                    computeRec (newBridges.Value ==> q) (max newD maxDistance, newS)
                elif not testSize then             
                    computeRec (newBridges.Value ==> q) (max newD maxDistance, maxSize)
                else
                    computeRec (newBridges.Value ==> q) (maxDistance, maxSize)
    computeRec ((0, 0, input, 0) --> emptyQueue) (0, 0)               

let computeFirst =
    readLines "Input24.txt"
    |> Array.map parse
    |> Array.toList
    |> compute false

let computeSecond =
    readLines "Input24.txt"
    |> Array.map parse
    |> Array.toList
    |> compute true