module Day15

open Helper

let number = 15

let title = "Lens Library"

let compute part =
    part |> Seq.fold (fun acc c -> (c |> int |> (+) acc |> (*) 17) % 256) 0

let fillBoxes lines =
    let rec fillBox (map: (string * string)[][]) (hash: string list) =
        match hash with
        | [] ->
            map
            |> Array.mapi (fun i a -> a |> Array.mapi (fun j (_, v) -> (i + 1) * (j + 1) * (int v)) |> Array.sum)
            |> Array.sum
        | h :: t ->
            let v = h |> splitA [| '='; '-' |]
            let box = compute v.[0]
            let mode = h.Contains '='

            let idx = map.[box] |> Array.tryFindIndex (fun (s, _) -> s = v.[0])

            if mode && idx.IsSome then
                map.[box].[idx.Value] <- (v.[0], v.[1])
            elif mode then
                map.[box] <- Array.append map.[box] [| (v.[0], v.[1]) |]
            else
                map.[box] <- map.[box] |> Array.filter (fun (s, _) -> s <> v.[0])

            fillBox map t

    fillBox (Array.init 256 (fun _ -> [||])) lines

let computeFirst () =
    (readInput "Input15.txt").Trim() |> splitC ',' |> Array.sumBy compute

let computeSecond () =
    (readInput "Input15.txt").Trim() |> splitC ',' |> Array.toList |> fillBoxes
