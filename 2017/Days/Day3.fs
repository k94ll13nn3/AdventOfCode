module Day3

let input = 289326

 

let nearestOddRoot (n:int) =

    let root = n |> float |> sqrt |> floor |> int

    let realRoot = root + (if root * root = n then 0 else 1)

    realRoot + (if realRoot % 2 = 1 then 0 else 1)

 

let leftMid root =

    let rec leftMidRec n acc =

        match n with

        | 1 -> acc + 2

        | _ -> leftMidRec (n - 1) (acc + 1 + 8 * (n-1))

    (leftMidRec (root / 2) 0)

 

// first, we need to find the first odd root greater than the input (5 for 24, 5 for 25, 7 for 26, ...)

// then we need to find the mids (the numbers that are aligned to 1 (top, bottom, left, right) on the circle that ends with root²)

// the formula to find the left mid is : (1+8*0) + (1+8*1) + (1+8*2) + (1+8*3) + ...

// this formula stops at i = root / 2 (as an int)

// for each 'mid' ((leftMid root) + (root - 1) * i, i in [0;1;2;3]), we compute the distance between the mid and the input, in absolute

// -> the min is the value to add to the base distance, which is (root / 2) (each mid is at (root / 2) from 1)

let computeFirst =

    let root = nearestOddRoot input

    List.init 4 (fun i -> input |> (-) ((leftMid root) + (root - 1) * i) |> abs |> (+) (root / 2))

    |> List.min

 

type Point = {Value : int; X : int; Y : int}

type Direction = Up | Down | Left | Right

 

let getNextPoint p dir =

    match dir with

    | Up -> { p with Y = p.Y + 1}

    | Down -> { p with Y = p.Y - 1}

    | Left -> { p with X = p.X - 1}

    | Right -> { p with X = p.X + 1}

 

let getNextDir p dir =

    if p.X = p.Y && p.X > 0 then

        Left

    elif p.X = p.Y && p.X < 0 then

        Right

    elif p.X = -p.Y && p.X < 0 then

        Down

    elif (p.X - 1) = -p.Y && p.X > 0  then

        Up

    else

        dir

 

let computeSecond =

    let rec findNearestGreater (acc:Point list) dir =

        let newPoint = getNextPoint acc.Head dir

        let newDir = getNextDir newPoint dir

        let value =

            acc

            |> List.filter (fun e -> e.X >= newPoint.X-1 && e.X <= newPoint.X+1 && e.Y >= newPoint.Y-1 && e.Y <= newPoint.Y+1)

            |> List.sumBy (fun e -> e.Value)

        if value >= input then

            value

        else

            findNearestGreater ({ newPoint with Value = value }::acc) newDir

    findNearestGreater [{ Value = 1; X = 0; Y = 0}] Right