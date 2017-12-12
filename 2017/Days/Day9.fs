module Day9

open Helper

let computeFirst =
    let rec computeRec (l:char list) acc step last isInGroup =
        match (last, isInGroup, l) with
        | (_, _, []) -> acc
        | ('!', _, _::t) -> computeRec t acc step '\000' true // by def, '!' only appears in group, so no need to check isInGroup
        | (_, true, h::t) -> computeRec t acc step h (h <> '>')
        | (_, false, h::t) ->
            match h with 
            | '{' -> computeRec t (acc + step) (step + 1) h false
            | _ -> computeRec t acc (step - (if h = '}' then 1 else 0)) h (h = '<') // when not in group, '<' marks the start of a group and step is reduced by 1 if h = '}'
    computeRec (readInput "Input9.txt" |> Seq.toList) 0 1 '\000'  false

let computeSecond =
    let rec computeRec (l:char list) acc last isInGroup =
        match (last, isInGroup, l) with
        | (_, _, []) -> acc
        | ('!', _, _::t) -> computeRec t acc '\000'  true // don't count the character following a '!'
        | (_, true, '>'::t) ->  computeRec t acc '>' false // quit the group
        | (_, true, '!'::t) -> computeRec t acc '!' true // don't count the '!'
        | (_, true, h::t) -> computeRec t (acc + 1) h true
        | (_, false, h::t) -> computeRec t acc h (h = '<') // when not in group, '<' marks the start of a group
    computeRec (readInput "Input9.txt" |> Seq.toList) 0 '\000' false