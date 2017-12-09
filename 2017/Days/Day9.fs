module Day9

open Helper

let computeFirst =
    let rec computeRec (l:char list) acc step last isInGroup =
        match l with
        | [] -> acc
        | h::t -> 
            if isInGroup && last = '!' then
                computeRec t acc step (char 0) true
            elif isInGroup && h = '>' then
                computeRec t acc step h false
            elif isInGroup then
                computeRec t acc step h true
            else 
                match h with 
                | '{' -> computeRec t (acc + step) (step + 1) h false
                | '}' -> computeRec t acc (step - 1) h false
                | '<' -> computeRec t acc step h true
                | _ -> computeRec t acc step h false
    computeRec (readInput "Input9.txt" |> Seq.toList) 0 1 (char 0) false

let computeSecond =
    let rec computeRec (l:char list) acc last isInGroup =
        match l with
        | [] -> acc
        | h::t -> 
            if isInGroup && last = '!' then
                computeRec t acc  (char 0) true
            elif isInGroup && h = '>' then
                computeRec t acc h false
            elif isInGroup && h = '!' then
                computeRec t acc h true
            elif isInGroup then
                computeRec t (acc + 1) h true
            else 
                match h with 
                | '<' -> computeRec t acc h true
                | _ -> computeRec t acc h false
    computeRec (readInput "Input9.txt" |> Seq.toList) 0 (char 0) false