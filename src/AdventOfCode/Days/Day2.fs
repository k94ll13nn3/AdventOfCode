module Day2

open Helper
open System

let number = 2

let title = "Cube Conundrum"

type Draw = { R: int; G: int; B: int }

let parseGame (input: string) =
    let getValue =
        function
        | Some(s: string) -> s.Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.head |> int
        | None -> 0

    let parseDraw (draw: string) =
        let a = draw.Split ','
        let red = a |> Array.tryFind (fun line -> line.Contains "red") |> getValue
        let green = a |> Array.tryFind (fun line -> line.Contains "green") |> getValue
        let blue = a |> Array.tryFind (fun line -> line.Contains "blue") |> getValue
        { R = red; G = green; B = blue }

    let parts = input.Split ':'
    let gameNumber = parts.[0].Split ' ' |> (fun arr -> arr.[1]) |> int
    let draws = parts.[1].Split ';' |> Array.map parseDraw

    (gameNumber, draws)

let computeFirst () =
    readLinesSeq "Input2.txt"
    |> Seq.map parseGame
    |> Seq.filter (fun (_, draws) -> draws |> Array.exists (fun d -> d.R > 12 || d.G > 13 || d.B > 14) |> not)
    |> Seq.sumBy (fun (gameNumber, _) -> gameNumber)

let computeSecond () =
    readLinesSeq "Input2.txt"
    |> Seq.map parseGame
    |> Seq.map (fun (g, draws) ->
        (draws |> Array.maxBy _.R |> _.R, draws |> Array.maxBy _.G |> _.G, draws |> Array.maxBy _.B |> _.B))
    |> Seq.sumBy (fun (r, g, b) -> r * g * b)
