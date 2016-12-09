module Day9

open Reader
open Helper
open System

type Mode = Classic | Deep
    
let matchPattern (pat : string) (rem : string) =
    let s = pat.Split([|'x'|])
    let (plength, nocc) = (int s.[0], uint64 s.[1])
    match rem.Length with
    | i when i <= plength -> (nocc, rem, "")
    | _ -> (nocc, rem.[0..plength - 1], rem.Substring(plength)) // number of rep, str to rep, remaining str

let rec computeLength mode (str : string)  =
    let s = str.Split([|'('; ')'|], 3)
    match s.Length with
    | 1 -> s.[0].LongLength
    | _ -> 
        let (nrep, strToRep, rem) = matchPattern s.[1] s.[2] in 
            match mode with
            | Classic -> s.[0].LongLength + (nrep * strToRep.LongLength) + computeLength mode rem
            | Deep -> s.[0].LongLength + (nrep * computeLength mode strToRep) + computeLength mode rem

let computeFirst =
    readInput @"input9.txt"
    |> computeLength Classic

let computeSecond =
    readInput @"input9.txt"
    |> computeLength Deep