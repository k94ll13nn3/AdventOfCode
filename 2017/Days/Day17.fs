module Day17

let input = 304

let rec insert v i l =
    match i, l with
    | 0, xs -> v::xs
    | i, x::xs -> x::insert v (i - 1) xs
    | _, [] -> failwith "insert - error"

let compute (acc:int list) value current =
    let i = if acc.Length = 0 then 0 else ((current + value + input) % acc.Length) + 1
    if i >= acc.Length then
        (acc@[value], i)
    else
        ((insert value i acc), i)
    
let compute2 accLength value current second =
    let i = if accLength = 0 then 0 else ((current + value + input) % accLength) + 1
    let newSecond = if i = 1 then value else second
    (accLength + 1, i, newSecond)

let computeFirst = 
    Seq.init 2018 id
    |> Seq.fold (fun (acc1, acc2) e -> compute acc1 e acc2) ([], 0)
    ||> (fun a e -> a |> Array.ofList |> (fun x -> Array.get x (e+1)))

let computeSecond =
    Seq.init 50000001 id
    |> Seq.fold (fun (acc1, acc2, acc3) e -> compute2 acc1 e acc2 acc3) (0, 0, 0)
    |||> (fun _ _ a -> a)