module Day3

open Helper
open System

let number = 3

let title = "Gear Ratios"

let schematic = readLines "Input3.txt"

let parseLine (symbolComparator: char -> bool) (lineIndex: int) (line: string) =
    let findIsAdjacent (index: int) =
        seq {
            for x in (lineIndex - 1) .. (lineIndex + 1) do
                for y in (index - 1) .. (index + 1) do
                    if
                        (x <> lineIndex || y <> index)
                        && x >= 0
                        && x < schematic.Length
                        && y >= 0
                        && y < line.Length
                        && symbolComparator schematic.[x].[y]
                        && (Char.IsDigit schematic.[x].[y] |> not)
                    then
                        yield (x, y)
        }
        |> Seq.tryHead


    let rec parseLineRec (index: int) (acc: int) (accList: (int * (int * int)) list) (adj: (int * int) option) =
        match (index, adj) with
        | _ when index = line.Length && adj.IsSome -> (acc, adj.Value) :: accList
        | _ when index = line.Length -> accList
        | _ when Char.IsDigit line.[index] ->
            (if adj.IsSome then adj else findIsAdjacent index)
            |> parseLineRec (index + 1) (acc * 10 + (charToInt line.[index])) accList
        | _ when adj.IsSome -> parseLineRec (index + 1) 0 ((acc, adj.Value) :: accList) None
        | _ -> parseLineRec (index + 1) 0 accList None

    parseLineRec 0 0 [] None

let computeFirst () =
    schematic
    |> Array.mapi (parseLine ((<>) '.'))
    |> Array.map (List.sumBy fst)
    |> Array.sum

let computeSecond () =
    let rec compute (acc: int list) =
        function
        | []
        | [ _ ] -> acc
        | (hv1, h1) :: (hv2, h2) :: t when h1 = h2 -> compute ((hv1 * hv2) :: acc) t
        | _ :: t -> compute acc t

    schematic
    |> Array.mapi (parseLine ((=) '*'))
    |> Array.reduce (@)
    |> List.sortBy snd
    |> compute []
    |> List.sum
