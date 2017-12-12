module Day12

open Helper

type Program = {Name : int; Children : seq<int>}

let parseLine s =
    match s with
    | Regex @"(\d+) <-> ([\d\s,]+)" [i; c] -> {Name = int i; Children = c.Split ',' |> Array.map ((fun e -> e.Trim()) >> int)}
    | _ -> failwith "parseLine - error"

let group (programs:Program list) =
    let rec group1 (l:Program list) (g:int list list) (noGroup:Program list) =
        match l with 
        | [] -> (g, noGroup)
        | h::t -> 
            let f = g |> List.tryFindIndex (fun x -> Set.intersect (Set x) (Set h.Children) <> Set.empty)
            if f.IsSome then
                let gg = g |> List.mapi (fun i e -> if i = f.Value then h.Name::e else e)
                group1 t gg noGroup
            elif (h.Children |> Seq.length) = 1 && (h.Children |> Seq.head) = h.Name then
                group1 t ([h.Name]::g) noGroup         
            else     
                group1 t g (h::noGroup)     
    let rec groupRec (l:Program list) (g:int list list) =
        if l = List.Empty then
            g
        else
            let (newGroup, newList) = group1 l g []
            if (newList.Length = l.Length) then 
                groupRec newList.Tail ([newList.Head.Name]::newGroup)
            else
                groupRec newList newGroup
    groupRec programs.Tail [[programs.Head.Name]]            

let input =     
    readLines "Input12.txt"
    |> Array.toList
    |> List.map parseLine
    |> group

let computeFirst =
    input
    |> List.find (fun x -> x |> List.contains 0)
    |> List.length


let computeSecond =
    input
    |> List.length