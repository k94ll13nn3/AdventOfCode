module Helper

let intersect x y = Set.intersect (Set.ofList x) (Set.ofList y) |> Set.toList