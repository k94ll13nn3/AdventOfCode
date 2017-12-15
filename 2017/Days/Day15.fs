module Day15

let generator i j =  
    let rec f a = seq { let newValue = (a * j) % 2147483647UL in yield newValue; yield! f newValue }
    f i

let generatorA = generator 289UL 16807UL |> Seq.map ((&&&) 0xFFFFUL) |> Seq.take 40000000
let generatorB = generator 629UL 48271UL |> Seq.map ((&&&) 0xFFFFUL) |> Seq.take 40000000

let computeFirst =
    (generatorA, generatorB) ||> Seq.fold2 (fun acc e1 e2 -> if e1 = e2 then acc + 1 else acc) 0

let newGenerator i j k =  
    let rec f a = seq { 
            let newValue = (a * j) % 2147483647UL
            if (newValue % k) = 0UL then 
                yield newValue; yield! f newValue 
            else 
                yield! f newValue}
    f i

let newGeneratorA = newGenerator 289UL 16807UL 4UL |> Seq.map ((&&&) 0xFFFFUL) |> Seq.take 5000000
let newGeneratorB = newGenerator 629UL 48271UL 8UL |> Seq.map ((&&&) 0xFFFFUL) |> Seq.take 5000000

let computeSecond =
    (newGeneratorA, newGeneratorB) ||> Seq.fold2 (fun acc e1 e2 -> if e1 = e2 then acc + 1 else acc) 0