module Helper

open System
open Microsoft.FSharp.Reflection
open System.Text.RegularExpressions

let intersect x y = Set.intersect (Set.ofList x) (Set.ofList y) |> Set.toList

// http://www.fssnip.net/9l
let fromString<'a> (s:string) =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
    |[|case|] -> FSharpValue.MakeUnion(case,[||]) :?> 'a
    |_ -> failwith (sprintf "%s is not a valid %A value" s typeof<'a>)

//http://fssnip.net/29
let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let removeChars (stripChars:string) (text:string) =
    text.Split(stripChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
    |> String.Concat

let flip f a b = f b a

let rotate (a:'T[][]) =
    [| for x in {0 .. a.[0].Length-1} -> [| for y in {0 .. a.Length-1} -> a.[y].[x] |] |]

let rotateString (a:string[]) =
    [| for x in {0 .. a.[0].Length-1} -> [| for y in {0 .. a.Length-1} -> a.[y].[x] |] |]

let seqToString (s : seq<char>) = s |> Seq.toArray |> String 

type System.String with
    member x.LongLength = uint64 x.Length

let isEven i = i % 2 = 0