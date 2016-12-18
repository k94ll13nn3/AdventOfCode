module Day18

let input = "^..^^.^^^..^^.^...^^^^^....^.^..^^^.^.^.^^...^.^.^.^.^^.....^.^^.^.^.^.^.^.^^..^^^^^...^.....^....^."
let input2 = input.ToCharArray() |> Array.map (fun elem -> elem = '.')

let getNextVal (map : bool[]) (i : int) =
    match i with
    | _ when i = 0 -> map.[i + 1]
    | _ when i + 1 = input.Length -> map.[i - 1]
    | _ -> map.[i - 1] = map.[i + 1]

let getNextLine (previousLine : bool[]) = previousLine |> Array.mapi (fun i _ -> getNextVal previousLine i)

let countTrue a = a |> Array.filter id |> Array.length

let compute (rows : int) =
    seq {1..rows-1} // this will compute one more line in order to have the result as the second element of the accumulator
    |> Seq.fold (fun acc _ ->  let (a, b) = acc in let c = getNextLine a in (c, b + (c |> countTrue))) (input2, (input2 |> countTrue)) |> snd

let computeFirst = compute 40
let computeSecond = compute 400000