module Day13

open Reader
open Helper
open System
open Types

let input = 1350

let isOpenSpace (p : Point) =
    Convert.ToString(p.X*p.X + 3*p.X + 2*p.X*p.Y + p.Y + p.Y*p.Y + input, 2)
    |> Seq.filter (fun c -> c = '1')
    |> Seq.length
    |> isEven

let isValid (p : Point) (visited : Point list) =
        p.X >= 0 &&
        p.Y >= 0 &&
        (isOpenSpace p) && 
        (visited |> Seq.exists (fun elem -> elem = p) |> not)

let computeFirst =
    let rec findShortestPath (q : Container<Point * int>) (visited : Point list) =
        let ((p, length), newQueue) = take q
        if p.X = 31 && p.Y = 39 
            then length
            else let (up, down, left, right) = ({X = p.X; Y = p.Y - 1}, {X = p.X; Y = p.Y + 1}, {X = p.X - 1; Y = p.Y}, {X = p.X + 1; Y = p.Y})
                 let neighboors = [ up; down; left; right ] |> List.filter (fun elem -> isValid elem visited)
                 let neighboorsToEnqueue = neighboors |> List.map (fun elem -> elem, length + 1)
                 findShortestPath (addList newQueue neighboorsToEnqueue) (visited@neighboors)
    findShortestPath (add emptyQueue ({X = 1; Y = 1}, 0)) [{X = 1; Y = 1}]

let computeSecond =
    let rec findShortestPath (q : Container<Point * int>) (visited : Point list) =
        if q = emptyQueue 
            then visited.Length
            else let ((p, length), newQueue) = take q
                 if length >= 50 // since all items are enqueued, the length of the first is the miminum amongst the queue, so all items after are invalid
                    then visited.Length
                    else
                        let (up, down, left, right) = ({X = p.X; Y = p.Y - 1}, {X = p.X; Y = p.Y + 1}, {X = p.X - 1; Y = p.Y}, {X = p.X + 1; Y = p.Y})
                        let neighboors = [ up; down; left; right ] |> List.filter (fun elem -> isValid elem visited)
                        let neighboorsToEnqueue = neighboors |> List.map (fun elem -> elem, length + 1)
                        findShortestPath (addList newQueue neighboorsToEnqueue) (visited@neighboors)
    findShortestPath (add emptyQueue ({X = 1; Y = 1}, 0)) [{X = 1; Y = 1}]