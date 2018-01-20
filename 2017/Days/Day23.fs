module Day23

open Helper

let getIndex (str : string) = (int str.[0]) - (int 'a')

let (|Operator|_|) (acc : int64[]) (input : string) =
    match input with
    | Regex @"(\w+) ([a-z]+) (-?[0-9]+)" [op; x; y] -> Some (op, getIndex x, int64 y)
    | Regex @"(\w+) ([a-z]+) ([a-z]+)" [op; x; y] -> Some (op, getIndex x, acc.[getIndex y])
    | _ -> None

let compute1 (input : string[]) = 
    let rec doInstruction (index : int) (acc : int64[]) mulInstructions = 
        if index >= input.Length then 
            mulInstructions 
        else 
            match input.[index] with
                | Regex @"jnz (-?[0-9]+) (-?[0-9]+)" [x; y] -> doInstruction (if int x <> 0 then index + (int y) else index + 1) acc mulInstructions
                | Operator acc (operator, x, y) -> 
                    match operator with
                    | "set" -> doInstruction (index + 1) (acc |> Array.copySet x y) mulInstructions
                    | "sub" -> doInstruction (index + 1) (acc |> Array.copySet x (acc.[x] - y)) mulInstructions
                    | "mul" -> doInstruction (index + 1) (acc |> Array.copySet x (acc.[x] * y)) (mulInstructions + 1L)
                    | "jnz" -> doInstruction (index + (if acc.[x] <> 0L then int y else 1)) acc mulInstructions
                    | _ -> failwith "compute - err"
                | _ -> failwith "compute - err"
    doInstruction 0 (Array.zeroCreate 26) 0L

let computeFirst =
    readLines "Input23.txt" |> Seq.toArray |> compute1

let computeSecond =
    905

// Optimization for part 2 in C#: the part l11-20 can be summarized as : if b % d == 0 then f = 0
//long b = 67 * 100 + 100000;
//long c = b + 17000;
//long d = 0;
//long f = 0;
//long h = 0;
//
//do
//{
//    f = 1;
//    d = 2;
//
//    do
//    {
//        if (b % d == 0)
//        {
//            f = 0;
//            break;
//        }
//
//        d = d + 1;
//    } while (d != b);
//
//    if (f == 0)
//    {
//        h = h + 1;
//    }
//
//    if (b == c)
//    {
//        Console.WriteLine(h);
//        return;
//    }
//
//    b = b + 17;
//} while (true);