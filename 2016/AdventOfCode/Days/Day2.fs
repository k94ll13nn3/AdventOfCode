module Day2

open Reader
open Helper
open System
open Types

type Direction = U | L | R | D

let square = [| [| "1"; "2"; "3" |]; 
                [| "4"; "5"; "6" |]; 
                [| "7"; "8"; "9" |] |]

let diamond = [| [| " "; " "; "1"; "" ; " " |]; 
                 [| " "; "2"; "3"; "4"; " " |]; 
                 [| "5"; "6"; "7"; "8"; "9" |]; 
                 [| " "; "A"; "B"; "C"; " " |]; 
                 [| " "; " "; "D"; " "; " " |] |]

let codeFromSquare p = 
    square.[1 - p.Y].[p.X + 1] // Y axis is inverted

let codeFromDiamond p = 
    diamond.[2 - p.Y].[p.X + 2] // Y axis is inverted

let toDirection c =
    fromString<Direction> (string c)

let moveSquare p d =
    match d with
    | U -> { X = p.X; Y = min (p.Y + 1) 1 }
    | D -> { X = p.X; Y = max (p.Y - 1) -1 }
    | L -> { X = max (p.X - 1) -1; Y = p.Y }
    | R -> { X = min (p.X + 1) 1; Y = p.Y }

let moveDiamond p d =
    match d with
    | U -> match (abs p.X, p.Y) with
            | (0, 2) -> p 
            | (1, 1) -> p 
            | (2, 0) -> p 
            | _ -> { X = p.X; Y = p.Y + 1 }
    | D -> match (abs p.X, p.Y) with
            | (0, -2) -> p 
            | (1, -1) -> p 
            | (2, 0) -> p 
            | _ -> { X = p.X; Y = p.Y - 1 }
    | L -> match (p.X, abs p.Y) with
            | (0, 2) -> p 
            | (-1, 1) -> p 
            | (-2, 0) -> p 
            | _ -> { X = p.X - 1; Y = p.Y }
    | R -> match (p.X, abs p.Y) with
            | (0, 2) -> p 
            | (1, 1) -> p 
            | (2, 0) -> p 
            | _ -> { X = p.X + 1; Y = p.Y }

let resolveLine (str : string) (p : Point) moveFunction =
    str.ToCharArray(0, str.Length)
    |> Array.fold (fun acc elem -> moveFunction acc (toDirection elem)) p
    
let computeFirst =
    let rec advance p (lstr : string list) =
        match lstr with
        | h::t -> let resolvedPoint = (resolveLine h p moveSquare) in (codeFromSquare resolvedPoint) + (advance resolvedPoint t)
        | [] -> ""
    advance {X = 0; Y = 0} (Seq.toList (readLines @"input2.txt"))

let computeSecond =
    let rec advance p (lstr : string list) =
        match lstr with
        | h::t -> let resolvedPoint = (resolveLine h p moveDiamond) in (codeFromDiamond resolvedPoint) + (advance resolvedPoint t)
        | [] -> ""
    advance {X = 0; Y = 0} (Seq.toList (readLines @"input2.txt")) 