module Day25

open Helper
open Types
           

let computeStep currentState (map:int[]) index =
    if currentState = 'A' then
        if map.[index] = 0 then
            map.[index] <- 1
            ('B', map, index + 1)
        else
            map.[index] <- 0
            ('C', map, index - 1)
    elif currentState = 'B' then
        if map.[index] = 0 then
            map.[index] <- 1
            ('A', map, index - 1)
        else
            map.[index] <- 1
            ('C', map, index + 1)
    elif currentState = 'C' then
        if map.[index] = 0 then
            map.[index] <- 1
            ('A', map , index + 1)
        else
            map.[index] <- 0
            ('D', map, index - 1)
    elif currentState = 'D' then
        if map.[index] = 0 then
            map.[index] <- 1
            ('E', map, index - 1)
        else
            map.[index] <- 1
            ('C', map, index - 1)
    elif currentState = 'E' then
        if map.[index] = 0 then
            map.[index] <- 1
            ('F', map, index + 1)
        else
            map.[index] <- 1
            ('A', map, index + 1)
    else
        if map.[index] = 0 then
            map.[index] <- 1
            ('A', map, index + 1)
        else
            map.[index] <- 1
            ('E', map, index + 1)

let computeFirst =
    Seq.init 12134527 id 
    |> Seq.fold (fun (a, b, c) _ -> computeStep a b c ) ('A', Array.zeroCreate (50000), 50000/2)
    |> (fun (_, e, _) -> e)
    |> Array.filter (fun e -> e = 1)
    |> Array.length

let computeSecond =
    "Noyeux Joel"