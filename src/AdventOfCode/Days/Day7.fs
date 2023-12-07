module Day7

open Helper

let number = 7

let title = "Camel Cards"

let lines = readLines "Input7.txt"

let getCardValue j =
    function
    | 'A' -> 0xE
    | 'K' -> 0xD
    | 'Q' -> 0xC
    | 'J' -> j
    | 'T' -> 0xA
    | i -> charToInt i

let computeHandValue j =
    Array.fold (fun acc c -> acc * 16 + getCardValue j c) 1

let computeStrengh (hand: char[]) =
    let groups =
        hand |> Array.groupBy id |> Array.sortByDescending (snd >> Array.length)

    match groups.Length with
    | 1 -> 7
    | 2 when (snd groups.[0]).Length = 4 -> 6
    | 2 when (snd groups.[0]).Length = 3 && (snd groups.[1]).Length = 2 -> 5
    | 3 when (snd groups.[0]).Length = 3 -> 4
    | 3 when (snd groups.[0]).Length = 2 && (snd groups.[1]).Length = 2 -> 3
    | 4 when (snd groups.[0]).Length = 2 -> 2
    | _ -> 1

let findBestWithJoker (hand: char[]) =
    if hand |> Array.contains 'J' then
        [| '2'; '3'; '4'; '5'; '6'; '7'; '8'; '9'; 'T'; 'Q'; 'K'; 'A' |]
        |> Array.map (fun rep -> hand |> Array.map (fun c -> if c = 'J' then rep else c))
        |> Array.map (fun nw -> (computeStrengh nw, computeHandValue 0xB nw))
        |> Array.sortByDescending id
        |> Array.head
        |> fst
    else
        computeStrengh hand

let computeFirst () =
    lines
    |> Array.map (splitC ' ')
    |> Array.map (fun parts ->
        (let hand = parts.[0] |> Seq.toArray in (hand |> computeStrengh, hand |> computeHandValue 0xB), int parts.[1]))
    |> Array.sortBy fst
    |> Array.mapi (fun i (_, bid) -> (i + 1) * bid)
    |> Array.reduce (+)

let computeSecond () =
    lines
    |> Array.map (splitC ' ')
    |> Array.map (fun parts ->
        (let hand = parts.[0] |> Seq.toArray in (hand |> findBestWithJoker, hand |> computeHandValue 1), int parts.[1]))
    |> Array.sortBy fst
    |> Array.mapi (fun i (_, bid) -> (i + 1) * bid)
    |> Array.reduce (+)
