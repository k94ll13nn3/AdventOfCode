module Day12

open Helper
open System

let number = 12

let title = "Hot Springs"

let lines = readLines "Input12.txt"

let isMatch (groups: int[]) (line: char[]) =
    let rec parseGroups (currentLine: char[]) (acc: int) (remainingGroups: int[]) =
        match currentLine.Length with
        | 0 ->
            (remainingGroups.Length = 0 && acc = 0)
            || (remainingGroups.Length = 1 && acc = remainingGroups.[0])
        | _ when currentLine.[0] = '.' ->
            if acc = 0 then
                parseGroups currentLine.[1..] 0 remainingGroups
            elif remainingGroups.Length > 0 && remainingGroups.[0] = acc then
                parseGroups currentLine.[1..] 0 remainingGroups.[1..]
            else
                false
        | _ -> parseGroups currentLine.[1..] (acc + 1) remainingGroups

    parseGroups line 0 groups

let rec getPermutations (s: char[]) =
    match s |> Array.tryFindIndex ((=) '?') with
    | None -> [| s |]
    | Some i -> Array.append (s |> Array.copySet i '.' |> getPermutations) (s |> Array.copySet i '#' |> getPermutations)

let extractInput line =
    let parts = line |> splitC ' '
    (parts.[0] |> Seq.toArray, parts.[1] |> splitC ',' |> Array.map int)

let computeFirst () =
    lines
    |> Array.map extractInput
    |> Array.map (fun (a, b) -> (a |> getPermutations, b))
    |> Array.sumBy (fun (arr, groups) -> arr |> Array.filter (isMatch groups) |> Array.length)

let computeSecond () =
    lines
    |> Array.map extractInput
    |> Array.map (fun (a, b) ->
        (Array.concat [ a; [| '?' |]; a; [| '?' |]; a; [| '?' |]; a; [| '?' |]; a ], Array.concat [ b; b; b; b; b ]))
    |> Array.map (fun (a, b) -> (a |> getPermutations, b))
    |> Array.sumBy (fun (arr, groups) -> arr |> Array.filter (isMatch groups) |> Array.length)
