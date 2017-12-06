module Day6

open Helper

let getIncrement n l =
    (n/l + if n % l > 0 then 1 else 0)

let redistribute ind n (arr:int []) =
    let getRealIndex i = (i - (ind + 1) + arr.Length) % arr.Length
    Array.init arr.Length (fun i ->  (getIncrement (n-(getRealIndex i)) arr.Length) + (if i = ind then 0 else arr.[i]))

let compute =
    let s = readInput "Input6.txt" 
    let rec computeNext (arr:int []) steps (acc:int[] list) =
        let i = arr |> Array.mapi (fun i x -> i, x)|> Array.maxBy snd |> fst
        let nextArr = redistribute i arr.[i] arr
        if acc |> List.exists (fun e -> e = nextArr) then
            (steps + 1, (acc |> List.findIndex (fun e -> e = nextArr)) + 1)
        else 
            computeNext nextArr (steps + 1) (nextArr::acc)
    computeNext (s.Split '\t' |> Array.map (int)) 0  []

let computeFirst =
    compute |> fst

let computeSecond =
    compute |> snd