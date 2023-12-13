module Day13

open Helper
open System

let number = 13

let title = "Point of Incidence"

let lines = readLines "Input13.txt"

let rec getPatterns (acc: char[][] list) (input: string[]) =
    match input |> Array.tryFindIndex (fun s -> s.Length = 0) with
    | None -> (input |> Array.map Seq.toArray) :: acc
    | Some 0 -> getPatterns acc input.[1..]
    | Some i -> getPatterns ((input.[.. i - 1] |> Array.map Seq.toArray) :: acc) input.[i..]

let findReflection original (pattern: char[][]) =
    let mutable res = 0

    for i in 0 .. pattern.Length / 2 + 1 do
        if
            (res = 0 || res = original)
            && (pattern.[..i] |> Array.rev) = pattern[i + 1 .. i + (pattern.[..i].Length)]
        then
            res <- (i + 1) * 100

    for i in pattern.Length / 2 .. pattern.Length - 1 do
        if
            (res = 0 || res = original)
            && (pattern.[i..] |> Array.rev) = pattern[i - (pattern.[i..].Length) .. i - 1]
        then
            res <- i * 100

    let rotated = rotateA pattern

    for i in 0 .. rotated.Length / 2 + 1 do
        if
            (res = 0 || res = original)
            && (rotated.[..i] |> Array.rev) = rotated[i + 1 .. i + (rotated.[..i].Length)]
        then
            res <- i + 1

    for i in rotated.Length / 2 .. rotated.Length - 1 do
        if
            (res = 0 || res = original)
            && (rotated.[i..] |> Array.rev) = rotated[i - (rotated.[i..].Length) .. i - 1]
        then
            res <- i

    res

let findReflectionWithSmudge (pattern: char[][]) =
    let original = findReflection 0 pattern

    seq {
        for i in 0 .. pattern.Length - 1 do
            for j in 0 .. pattern.[0].Length - 1 do
                let newValue =
                    Array.copySet2 i j (if pattern.[i].[j] = '.' then '#' else '.') pattern
                    |> findReflection original

                if newValue <> 0 && newValue <> original then
                    yield newValue
    }
    |> Seq.head

let computeFirst () =
    lines |> getPatterns [] |> List.rev |> List.sumBy (findReflection 0)

let computeSecond () =
    lines |> getPatterns [] |> List.rev |> List.sumBy findReflectionWithSmudge
