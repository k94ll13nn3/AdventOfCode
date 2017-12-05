
module Day5

open Helper

let compute offseter (instr:int []) =
    let rec move (instr:int []) (steps) (ind) =
        let jump = instr.[ind]
        if jump + ind >= instr.Length then 
            steps + 1
        else
            instr.[ind] <-   offseter instr.[ind]
            move instr (steps + 1) (jump + ind)
    move instr 0 0

let computeFirst =
    readLines "input5.txt" 
    |> Array.map (int)
    |> compute ((+) 1)

let computeSecond =
    readLines "input5.txt" 
    |> Array.map (int)
    |> compute (fun e -> if e >= 3 then e - 1 else e + 1)