module Day8

open Helper

let number = 8

let title = "Haunted Wasteland"

let lines = readLines "Input8.txt"

let lcm a b =
    let rec gcd a b =
        match b with
        | 0L -> abs a
        | _ -> gcd b (a % b)

    a * b / (gcd a b)

let parseInput =
    let map =
        lines.[2..]
        |> Array.map (splitA [| ' '; '='; '('; ')'; ',' |])
        |> Array.map (fun parts -> (parts.[0], (parts.[1], parts.[2])))
        |> Map.ofArray

    (lines.[0], map)

let rec move current index count endOp ((direction, map): (string * Map<string, (string * string)>)) =
    match current with
    | _ when endOp current -> count
    | _ ->
        let node =
            if direction.[index % direction.Length] = 'R' then
                map |> Map.find current |> snd
            else
                map |> Map.find current |> fst

        move node (index + 1) (count + 1) endOp (direction, map)

let computeFirst () =
    parseInput |> move "AAA" 0 0 ((=) "ZZZ")

let computeSecond () =
    let (direction, map) = parseInput

    map
    |> Map.filter (fun key _ -> key.[2] = 'A')
    |> Map.map (fun a _ -> move (a) 0 0 (fun c -> c.[2] = 'Z') (direction, map))
    |> Map.values
    |> Seq.fold (fun a acc -> lcm (int64 a) acc) 1L
