module Day16

open Helper

let rotate (l:int) (arr: 'a[]) = let l1 = arr.Length in arr |> Array.mapi (fun i _ -> arr.[(l1+i+l)%l1])

let swapIndex (arr: 'a[]) i j =
    let newArr = arr |> Array.copy
    newArr.[i] <- arr.[j]
    newArr.[j] <- arr.[i]
    newArr

let swapValue (arr: 'a[]) (i:'a) (j:'a) =
    let i1 = arr |> Array.findIndex ((=) i)
    let i2 = arr |> Array.findIndex ((=) j)
    swapIndex arr i1 i2

let start = "abcdefghijklmnop".ToCharArray()

let moves = (readInput "Input16.txt").Split ','

let apply acc e =
    match e with
    | Regex "s(\d+)" [i] -> rotate -(int i) acc
    | Regex "x(\d+)/(\d+)" [i; j] -> swapIndex acc (int i) (int j)
    | Regex "p(\w+)/(\w+)" [i; j] -> swapValue acc i.[0] j.[0]
    | _ -> failwith "apply - error"

let applyDance startArray = moves |> Array.fold apply (startArray)

let computeFirst = applyDance start |> System.String

let rec findCycle i acc =
    let newAcc = applyDance acc
    match newAcc = start with
    | true -> i
    | false -> findCycle (i+1) newAcc

let computeSecond =
    let cycle = findCycle 1 start
    Seq.init (100000000%cycle) id
    |> Seq.fold (fun acc _ -> applyDance acc) start
    |> System.String