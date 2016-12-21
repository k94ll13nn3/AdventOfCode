module Day21

open System
open Helper
open Reader

let rotate i (a : 'T[]) =
    let n = a.Length
    a |> Array.mapi (fun ind c -> let k = (ind + i) % n in a.[if k < 0 then n + k else k])

let positions n = seq {0..n-1} |> Seq.map (fun e -> e, (if e >= 4 then e + 2 else e + 1) % n)

let apply (pass : char[]) (instr : string) =
    match instr with
    | Regex @"swap position (\d+) with position (\d+)" [x; y] -> pass |> Array.mapi (fun i c -> if i = int x then pass.[int y] else if i = int y then pass.[int x] else c)
    | Regex @"swap letter (\w) with letter (\w)" [x; y] -> pass |> Array.map (fun c -> if c = x.[0] then y.[0] else if c = y.[0] then x.[0] else c)
    | Regex @"rotate (left|right) (\d+) step" [x; y] -> 
        match x with
        | "left" -> rotate (int y) pass
        | "right" -> rotate (- (int y)) pass
        | _ -> failwith "apply - err"
    | Regex @"rotate based on position of letter (\w)" [x] -> 
        let ind = positions pass.Length |> Seq.find (fun e -> (fst e) = (pass |> Array.findIndex (fun c -> c = x.[0]))) |> snd in
        pass |> rotate -ind
    | Regex @"reverse positions (\d+) through (\d+)" [x; y] ->
        match int x, int y with
        | 0, a when a = pass.Length - 1 -> pass |> Array.rev
        | 0, a -> Array.concat [pass.[0..a] |> Array.rev; pass.[a+1..]]
        | a, b when b = pass.Length - 1 -> Array.concat [pass.[0..a-1]; pass.[a..] |> Array.rev]
        | a, b -> Array.concat [pass.[0..a-1]; pass.[a..b] |> Array.rev; pass.[b+1..]]
    | Regex @"move position (\d+) to position (\d+)" [x; y] ->
        match int x, int y with
        | a, b when a < b -> pass |> Array.mapi (fun i c -> if i = b then pass.[a] else if i < a then c else if i >= a && i < b then pass.[i+1] else c)
        | a, b when a > b -> pass |> Array.mapi (fun i c -> if i = b then pass.[a] else if i < b then c else if i > b && i <= a then pass.[i-1] else c)
        | _ -> failwith "apply - err"
    | _ -> pass

let applyBack (pass : char[]) (instr : string) =
    match instr with
    | Regex @"swap position (\d+) with position (\d+)" [x; y] -> pass |> Array.mapi (fun i c -> if i = int x then pass.[int y] else if i = int y then pass.[int x] else c)
    | Regex @"swap letter (\w) with letter (\w)" [x; y] -> pass |> Array.map (fun c -> if c = x.[0] then y.[0] else if c = y.[0] then x.[0] else c)
    | Regex @"rotate (left|right) (\d+) step" [x; y] -> 
        match x with
        | "left" -> rotate (- (int y)) pass
        | "right" -> rotate (int y) pass
        | _ -> failwith "apply - err"
    | Regex @"rotate based on position of letter (\w)" [x] -> 
        let ind = positions pass.Length |> Seq.find (fun e -> (((fst e) + (snd e)) % pass.Length) = (pass |> Array.findIndex (fun c -> c = x.[0]))) |> snd in
        pass |> rotate ind
    | Regex @"reverse positions (\d+) through (\d+)" [x; y] ->
        match int x, int y with
        | 0, a when a = pass.Length - 1 -> pass |> Array.rev
        | 0, a -> Array.concat [pass.[0..a] |> Array.rev; pass.[a+1..]]
        | a, b when b = pass.Length - 1 -> Array.concat [pass.[0..a-1]; pass.[a..] |> Array.rev]
        | a, b -> Array.concat [pass.[0..a-1]; pass.[a..b] |> Array.rev; pass.[b+1..]]
    | Regex @"move position (\d+) to position (\d+)" [y; x] ->
        match int x, int y with
        | a, b when a < b -> pass |> Array.mapi (fun i c -> if i = b then pass.[a] else if i < a then c else if i >= a && i < b then pass.[i+1] else c)
        | a, b when a > b -> pass |> Array.mapi (fun i c -> if i = b then pass.[a] else if i < b then c else if i > b && i <= a then pass.[i-1] else c)
        | _ -> failwith "apply - err"
    | _ -> pass

let computeFirst = 
    readLines @"input21.txt" 
    |> Seq.fold apply ("abcdefgh".ToCharArray())
    |> String

let computeSecond = 
    readLines @"input21.txt" 
    |> Seq.rev
    |> Seq.fold applyBack ("fbgdceah".ToCharArray())
    |> String