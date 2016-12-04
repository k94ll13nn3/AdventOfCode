module Day4

open Reader
open System
open Helper

type Room = {Name : string; Id : int; Checksum : string}

let removeDashes =
    removeChars "-"
    
let isRealRoom (r : Room) =
    let letters = 
        r.Name 
        |> removeDashes 
        |> Seq.groupBy id 
        |> Seq.sortBy (fun (x, y) -> - (Seq.length y), x) 
        |> Seq.take 5 
        |> Seq.map (fun (x, y) -> x) 
        |> Seq.toArray
    String letters = r.Checksum

let decryptChar (i : int) (l : char) =
    match l with
    | '-' -> ' '
    | c -> int c 
        |> flip (-) 97 
        |> (+) i 
        |> flip (%) 26 
        |> (+) 97 
        |> char

let decryptName  (i : int) (s : string) =
    s 
    |> Seq.map (decryptChar i) 
    |> Seq.toArray 
    |> String

let decryptRoom r =
    decryptName  r.Id r.Name

let parseRoom (s : string) =
    match s with
    | Regex @"([\w\-]+)-(\d+)\[(\w+)\]" [ name; id; checksum ] ->
        {Name = name; Id = int id; Checksum = checksum}
    | _ -> failwith "parseRoom - err"

let computeFirst =
    readLines @"input4.txt"
    |> Seq.map parseRoom 
    |> Seq.filter isRealRoom
    |> Seq.fold (fun acc elem -> acc + elem.Id) 0

let computeSecond =
    let room =
        readLines @"input4.txt"
        |> Seq.map (parseRoom >> (fun r -> decryptRoom r, r.Id))
        |> Seq.tryFind (fun (x, y) -> x.Contains("northpole object storage"))
    snd room.Value // won't be None, I hope