module Day7

open Helper

type Tower = {Name : string; StartingWeight : int; Weight : int; Children : string list}

let parseLine s =
    match s with
    | Regex @"(\w+) \((\d+)\) -> ([\w\s,]*)" [l; i; c] -> {Name = l; StartingWeight = (int) i; Weight = (int) i; Children = c.Split ',' |> Seq.map (fun e -> e.Trim()) |> Seq.toList }
    | Regex @"(\w+) \((\d+)\)" [l; i] -> {Name = l; StartingWeight = (int) i; Weight = (int) i; Children = []}
    | _ -> failwith "parseLine - error"

// could also be solved with part 2, but keeping it for the science
let computeFirst =
    readLines "Input7.txt" 
    |> Seq.fold (fun (a, b) s -> let e = parseLine s in (e.Name::a, e.Children@b)) ([],[])
    |> (fun (a, b) -> (set a) - (set b))
    |> Set.maxElement // there should be only one element

let rec findLevel l t =
    match List.isEmpty t.Children with
    | true -> 0
    | false -> l |> Seq.filter (fun e -> t.Children |> List.contains e.Name) |> Seq.map (findLevel l) |> Seq.max |> (+) 1

let computeWeight l =
    let rec computeWeightRec acc l =
        match l with
        | h::t when h.Children.Length = 0 -> computeWeightRec (h::acc) t
        | h::t ->
            let children = acc |> List.filter (fun e -> h.Children |> List.contains e.Name) 
            let groupedChildren = 
                children
                |> List.groupBy (fun e -> e.Weight) 
                |> List.sortBy (fun (_, value) -> value.Length)
            if groupedChildren.Length > 1 then
                match groupedChildren with 
                | ((_, (f::_))::(s, _)::_) -> f.StartingWeight - (f.Weight - s)
                | _ -> failwith "computeWeightRec - error"
            else
                let newWeight = children |> List.sumBy (fun e -> e.Weight) |> (+) h.Weight
                computeWeightRec ({h with Weight =  newWeight}::acc) t
        | _ -> failwith "computeWeightRec - error"
    computeWeightRec [] (l |> Seq.toList)

let computeSecond =
    readLines "Input7.txt" 
    |> Seq.map parseLine
    |> (fun s -> s |> Seq.sortBy (fun e -> findLevel s e))
    |> computeWeight