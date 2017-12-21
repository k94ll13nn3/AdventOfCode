module Day19

open Helper

type Direction = Up | Down | Right | Left

let getNext (p1, p2) = function
    | Up -> (p1 - 1, p2)
    | Down -> (p1 + 1, p2)
    | Right -> (p1, p2 + 1)
    | Left -> (p1, p2 - 1)

let oppositeDirection = function
    | Up -> Down
    | Down -> Up
    | Right -> Left
    | Left -> Right

let isValid (input: char[][]) direction (p1, p2, d) =
    p1 >= 0 && p2 >= 0
    && p1 < input.Length && p2 < input.[0].Length 
    && (oppositeDirection direction) <> d
    && input.[p1].[p2] <> '+' && input.[p1].[p2] <> ' '

// it seems that only checking for ' ' after the end is needed for my input
// but i may have been a '-' or a '|' (the end would be near another route), and I'm not
// sure it would have worked (need to compare direction with value)
let resolve (input: char[][]) =
    let start = input.[0] |> Array.findIndex ((=) '|')
    let rec move ((p1, p2) as p) visited direction steps =
        if p1 < 0 || p2 < 0 || p1 >= input.Length || p2 >= input.[0].Length || input.[p1].[p2] = ' ' then
            (visited, steps - 1) // -1 because we are after the end (in order to check it) -> but only when we are not out of bounds
        elif input.[p1].[p2] <= 'Z' && input.[p1].[p2] >= 'A' then
            move (getNext p direction) (input.[p1].[p2]::visited) direction (steps + 1)
        elif input.[p1].[p2] <> '+' then  
            move (getNext p direction) visited direction (steps + 1)
        else    
            let (p1b, p2b, d) = 
                [(p1, p2 - 1, Left); (p1, p2 + 1, Right); (p1 - 1, p2, Up); (p1 + 1, p2, Down)] 
                |> List.find (isValid input direction) 
            move (p1b, p2b) visited d (steps + 1)
    move (0, start) [] Down 1

let solvedInput = 
    readLines "Input19.txt" 
    |> Array.map (fun s -> s.ToCharArray()) 
    |> resolve

let computeFirst =
    solvedInput
    ||> (fun l _ -> List.foldBack (fun e acc -> acc + e.ToString()) l "")

let computeSecond =
    solvedInput
    |> snd