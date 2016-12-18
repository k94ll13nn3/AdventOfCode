module Day18

let input = "^..^^.^^^..^^.^...^^^^^....^.^..^^^.^.^.^^...^.^.^.^.^^.....^.^^.^.^.^.^.^.^^..^^^^^...^.....^....^."

let getNextVal (map : char[]) (i : int) =
    match i with
    | _ when i = 0 -> if '.' = map.[i + 1] then '.' else '^'
    | _ when i + 1 = input.Length -> if map.[i - 1] = '.' then '.' else '^'
    | _ -> if map.[i - 1] = map.[i + 1] then '.' else '^'

let getNextLine (previousLine : char[]) =
    previousLine |> Array.mapi (fun i _ -> getNextVal previousLine i)

let compute (rows : int) =
    seq {1..rows} // this will compute one more line in order to have the result as the second element of the accumulator
    |> Seq.fold (fun acc _ ->  let (a, b) = acc in ((getNextLine a), b + (a |> Array.filter (fun c -> c = '.') |> Array.length))) (input.ToCharArray(), 0)
    |> snd

let computeFirst = compute 40
let computeSecond = compute 400000