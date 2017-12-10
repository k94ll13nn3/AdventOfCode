module Day10

open System

let input = "197,97,204,108,1,29,5,71,0,50,2,255,248,78,254,63"

let getIndex i ind l1 l2 =
    let j = (l2 + i - ind) % l2
    let k = (l2 + (l1 - j)) % l2
    (k + ind) % l2

let compare i ind l1 l2 =
    if l1 = l2 || l1 = 0 then
        true
    elif ind < ((ind + l1) % l2) then
        i < ind || i > ((ind + l1) % l2)
    else
        i < ind && i > ((ind + l1) % l2)

let computeRotation (hash:int[]) (length:int) (index:int) = 
    hash
    |> Array.mapi (fun i e -> if compare i index length hash.Length then e else hash.[getIndex i index length hash.Length])

let rec compute (hash:int[]) (skip:int) (index:int) (l:int list) = 
    match l with
    | [] -> hash
    | h::t -> 
        let newHash = computeRotation hash (max 0 (h - 1)) index
        compute newHash (skip+1) ((index + skip + h) % hash.Length) t

let computeFirst =
    input.Split([|','; ' '|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map (int)
    |> compute (Array.init 256 (id)) 0 0
    |> fun hash -> hash.[0] * hash.[1]

let computeSecond =
    input.ToCharArray()
    |> Array.map (int)
    |> (fun e -> Array.append e [|17; 31; 73; 47; 23|])
    |> Array.toList
    |> (fun e -> Seq.init 64 id |> Seq.fold (fun acc _ -> acc@e) [])
    |> compute (Array.init 256 (id)) 0 0
    |> Array.chunkBySize 16
    |> Array.map ((fun arr -> arr |> Array.reduce (^^^)) >> (fun e -> sprintf "%02x" e))
    |> Array.reduce (+)