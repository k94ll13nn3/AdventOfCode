module Day10

let input = "197,97,204,108,1,29,5,71,0,50,2,255,248,78,254,63"

let rotate (l:int) (arr: 'a[]) = let l1 = arr.Length in arr |> Array.mapi (fun i _ -> arr.[(l1+i+l)%l1])
let reverse (l:int) (arr: 'a[]) = Array.append (arr.[0..l-1] |> Array.rev) arr.[l..]
let computeRotation (length:int) (index:int) = rotate index >> reverse length >> rotate -index

let compute l =
    let rec computeRec (skip:int) (index:int) (l:int list) (hash:int[]) = 
        match l with
        | [] -> hash
        | h::t -> computeRotation h index hash |> computeRec (skip+1) ((index + skip + h) % hash.Length) t
    computeRec 0 0 (l |> Seq.toList) (Array.init 256 id)

let computeFirst =
    input.Split ','
    |> Array.map int
    |> compute 
    |> fun hash -> hash.[0] * hash.[1]

let computeSecond =
    input.ToCharArray()
    |> Array.map int
    |> (fun e -> Array.append e [|17; 31; 73; 47; 23|])
    |> (fun e -> Seq.init 64 id |> Seq.fold (fun acc _ -> Array.append acc e) [||])
    |> compute
    |> Array.chunkBySize 16
    |> Array.map (Array.reduce (^^^) >> (fun e -> sprintf "%02x" e))
    |> Array.reduce (+)