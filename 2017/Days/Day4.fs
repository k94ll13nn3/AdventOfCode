module Day4

open Helper

let compute (map:string[]->string[]) = 
    readLines "Input4.txt"
    |> Array.map ((fun e -> e.Split ' ') >> map)
    |> Array.filter (fun e -> e.Length = (e |> Seq.distinct |> Seq.length))
    |> Array.length

let computeFirst =
    compute id

let computeSecond =
    compute (Array.map (fun e -> e.ToCharArray() |> Array.sort |> System.String))