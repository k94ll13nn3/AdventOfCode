module Day5

open System
open System.IO
open System.Security.Cryptography

let md5Hasher = CryptoConfig.CreateFromName("MD5") :?> HashAlgorithm

let input = "cxdnnyjw"

let md5 (input : string) =
    input
    |> System.Text.Encoding.ASCII.GetBytes
    |> md5Hasher.ComputeHash
    |> Seq.map (fun c -> c.ToString("X2"))
    |> Seq.reduce (+)

let isStartingWithZeroes (a : string) =
    a.StartsWith("00000")

let rec find (i : uint64) (a : string) =
    let s = a + i.ToString() |> md5
    match isStartingWithZeroes s with
    | true -> (s.[5], i, s.[6])
    | false -> find (i + 1UL) a

let computeFirst =
    let rec findPassword (acc : int) (i : uint64) =
        let (newChar, newInt, _) = find i input
        match acc with
        | 8 -> []
        | _ -> (Char.ToLower newChar)::(findPassword (acc + 1) (newInt + 1UL))
    String (findPassword 0 0UL |> List.toArray)

let isValidPosition (pos : char) (acc : (char * char) list) =
    (pos < '0') || (pos > '7') || List.exists (fun (p, c) -> p = pos) acc

let computeSecond =
    let rec findPassword (acc : (char * char) list) (i : uint64) =
        let (pos, newInt, newChar) = find i input
        match List.length acc with
        | 8 -> acc |> List.sortBy (fun (p, c) -> p) |> List.map (fun (p, c) -> c)
        | _ -> match isValidPosition pos acc with
               | true -> findPassword acc (newInt + 1UL) // invalid position or already known position
               | false -> findPassword ((pos, Char.ToLower newChar)::acc) (newInt + 1UL)
    String (findPassword [] 0UL |> List.toArray)