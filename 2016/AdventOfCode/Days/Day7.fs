module Day7

open Reader
open Helper
open System

type Ip = { parts : seq<string>; hypernet  : seq<string> }

let splitByParity list = List.fold (fun (l,r) x -> x::r, l) ([],[]) list 

let toIp (str : string) =
    let input = str.Split([|'['; ']'|]) |> Seq.toList |> splitByParity
    { parts = fst input; hypernet = snd input }

// http://fssnip.net/4s
let isPalindrom (s : string) =
   let l = s.Length-1
   (l >= 1 && s.[0] <> s.[1]) && 
   seq { 0..(l+1)/2 } |> Seq.forall (fun i -> s.[i] = s.[l-i])

let rec createChunks n (l : string) =
    match l.Length with
    | i when i < n -> []
    | i when i = n -> [l]
    | _ -> l.[0..n-1]::(createChunks n l.[1..])

// Tests if at least one element of s in contained in x
let contains (x : string) (s : seq<string>) = 
    Seq.exists (fun str -> x.Contains(str)) s

let abaToBab (s : string) = String([|s.[1]; s.[0];s.[1] |])

let computeFirst =
    let isTls (str : string) =
        let input = toIp str
        (input.parts |> Seq.exists (createChunks 4 >> Seq.exists isPalindrom)) && // at least one palindrom in parts
        (input.hypernet |> Seq.exists (createChunks 4 >> Seq.exists isPalindrom)) |> not // no palindroms in hypernet
    readLines @"input7.txt" |> Seq.filter isTls |> Seq.length 

let computeSecond =
    let isSsl (str : string) =
        let input = toIp str
        let babList = input.parts |> Seq.collect (createChunks 3 >> Seq.filter isPalindrom) |> Seq.map abaToBab
        input.hypernet 
        |> Seq.exists (fun s -> contains s babList)
    readLines @"input7.txt" |> Seq.filter isSsl |> Seq.length 