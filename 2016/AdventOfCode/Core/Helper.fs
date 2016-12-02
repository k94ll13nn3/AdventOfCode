module Helper

open Microsoft.FSharp.Reflection

let intersect x y = Set.intersect (Set.ofList x) (Set.ofList y) |> Set.toList

// http://www.fssnip.net/9l
let fromString<'a> (s:string) =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
    |[|case|] -> FSharpValue.MakeUnion(case,[||]) :?> 'a
    |_ -> failwith (sprintf "%s is not a valid %A value" s typeof<'a>)