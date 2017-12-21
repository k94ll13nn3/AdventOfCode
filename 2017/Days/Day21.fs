module Day21

open Helper

// for future reference : https://www.reddit.com/r/adventofcode/comments/7l78eb/2017_day_21_solutions/drk4ohr/
// much simpler that the current solution

let flip flip1 flip2  (a:char[,]) = 
    let l = a|> Array2D.length1
    Array2D.init l l (fun i j -> a.[(if flip1 then l - i - 1 else i),(if flip2 then l - j - 1 else j)])

let rotate (a:char[,]) =
    let l = a|> Array2D.length1
    Array2D.init l l (fun i j -> a.[l - j - 1, i])

let rotate1 = rotate
let rotate2 = rotate >> rotate
let rotate3 = rotate >> rotate >> rotate
let flip1 = flip true false
let flip2 = flip false true
let flip3 = flip true true

let parse (line:string) =
    match line with
    | Regex "([.#/]+) => ([.#/]+)" [a1; b1] ->
        let parsedLineA = a1.Split '/' |> Array.map (fun s -> s.ToCharArray())
        let parsedLineB = b1.Split '/' |> Array.map (fun s -> s.ToCharArray())
        let a = Array2D.init parsedLineA.Length parsedLineA.Length (fun i j -> parsedLineA.[i].[j])
        let b = Array2D.init parsedLineB.Length parsedLineB.Length (fun i j -> parsedLineB.[i].[j])
        [(a, b); 
            (rotate1 a, b); (flip1 (rotate1 a), b); (flip2 (rotate1 a), b); (flip3 (rotate1 a), b); 
            (rotate2 a, b); (flip1 (rotate2 a), b); (flip2 (rotate2 a), b); (flip3 (rotate2 a), b); 
            (rotate3 a, b); (flip1 (rotate3 a), b); (flip2 (rotate3 a), b); (flip3 (rotate3 a), b); 
            (flip1 a, b); (rotate1 (flip1 a), b); (rotate2 (flip1 a), b); (rotate3 (flip1 a), b); 
            (flip2 a, b); (rotate1 (flip2 a), b); (rotate2 (flip2 a), b); (rotate3 (flip2 a), b);  
            (flip3 a, b); (rotate1 (flip3 a), b); (rotate2 (flip3 a), b); (rotate3 (flip3 a), b); ]
    | _ -> failwith "parse - error"

let parsedInput =
    readLines "Input21.txt"
    |> Array.map parse
    |> Array.fold (fun acc e -> acc@e) []

let baseArray = array2D  [['.';'#';'.'];['.';'.';'#'];['#';'#';'#']]

let findReplacement a = 
    let found = parsedInput |> List.tryFind (fun (e, _) -> e = a)
    if found.IsSome then
        found.Value |> snd
    else
        failwith "findReplacement - error"

let computeNextStepBy2 (current:char [,]) =
    let l = (current |> Array2D.length1) / 2
    let a = Array2D.init l l (fun i j -> current.[i*2..((i+1)*2-1),j*2..((j+1)*2-1)]) |> Array2D.map findReplacement
    Seq.init l id |> Seq.map (fun i -> a.[i,0..] |> Array2D.joinMany Array2D.joinByCols) |> Array2D.joinMany Array2D.joinByRows

let computeNextStepBy3 (current:char [,]) =
    let l = (current |> Array2D.length1) / 3
    let a = Array2D.init l l (fun i j -> current.[i*3..((i+1)*3-1),j*3..((j+1)*3-1)]) |> Array2D.map findReplacement
    Seq.init l id |> Seq.map (fun i -> a.[i,0..] |> Array2D.joinMany Array2D.joinByCols) |> Array2D.joinMany Array2D.joinByRows

let computeNextStep (current:char [,]) =
    if (current |> Array2D.length1) % 2 = 0 then
        computeNextStepBy2 current
    else
        computeNextStepBy3 current

let computeFirst =
    Seq.init 5 id 
    |> Seq.fold (fun acc _ -> computeNextStep acc) baseArray
    |> Array2D.foldi (fun acc e -> acc + if e = '#' then 1 else 0) 0

let computeSecond =
    Seq.init 18 id 
    |> Seq.fold (fun acc _ -> computeNextStep acc) baseArray
    |> Array2D.foldi (fun acc e -> acc + if e = '#' then 1 else 0) 0
