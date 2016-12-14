module Day14

open System
open System.Text
open System.IO
open System.Security.Cryptography
open System.Collections.Generic

// -----  MD5  -----
// Byte[] -> string : http://stackoverflow.com/a/624379/3836163

let md5Hasher = CryptoConfig.CreateFromName("MD5") :?> HashAlgorithm
let dict = new Dictionary<string, string>()
let internalMd5 (str : string) = 
    let tmp = 
        str
        |> Encoding.UTF8.GetBytes
        |> md5Hasher.ComputeHash
        |> BitConverter.ToString
    tmp.Replace("-","").ToLower() 
let basicMd5 (iter : int) (str : string) =
    match dict.TryGetValue(str) with
    | (true, v) -> v
    | _ ->
        let tmp = seq {0..iter} |> Seq.fold (fun acc elem -> internalMd5 acc) str // DO NOT STORE EACH MD5, NOOB
        dict.Add(str, tmp)
        tmp

// -----  ---  -----

module List =
    let pack xs =
        let imp x = function
            | (i, count) :: ta when i = x -> (i, count + 1) :: ta
            | ta -> (x, 1) :: ta
        List.foldBack imp xs []

let input = "zpqevtbw"

let md5 (i : int) = 
    input + i.ToString() |> basicMd5 0

let md5WithExtra (i : int) = 
    input + i.ToString() |> basicMd5 2016

let isFiveOfKind (i : int) (key : char) md5Hash =
    md5Hash i 
    |> Seq.toList
    |> List.pack
    |> List.exists (fun (k, s) -> k = key && s >= 5)

let isKey (i : int) md5Hash = 
    let elem =
        md5Hash i 
        |> Seq.toList
        |> List.pack
        |> List.tryFind (fun (k, s) -> s >= 3)
    match elem with
    | None -> false
    | Some(k, s) -> seq {i+1..i+1001} |> Seq.exists (fun i -> isFiveOfKind i k md5Hash)

let resolve md5Hash = 
    let rec resolveRec (i : int) (count : int) =
        if count = 64
            then i - 1
            else if isKey i md5Hash
                then resolveRec (i + 1) (count + 1)
                else resolveRec (i + 1) count
    resolveRec 0 0

let computeFirst =
    dict.Clear()
    resolve md5
    
let computeSecond =
    dict.Clear()
    resolve md5WithExtra