module Types

type Point = {X : int; Y : int}

// http://stackoverflow.com/a/2050402
type Container<'a> = Queue of 'a list * 'a list | Stack of 'a list

let emptyQueue = Queue([], [])

let emptyStack = Stack([])

let add q e = 
    match q with
    | Queue(fs, bs) -> Queue(e::fs, bs)
    | Stack(l) -> Stack(e::l)

let rec addList q (e : 'a list) =
    match e with
    | [] -> q
    | [a] -> add q a
    | h::t -> let newContainer = add q h in addList newContainer t

let take q = 
    match q with
    | Queue([], []) -> failwith "take - err"
    | Stack([]) -> failwith "take - err"
    | Stack(h::t) -> h, Stack(t)
    | Queue(fs, b :: bs) -> b, Queue(fs, bs)
    | Queue(fs, []) -> 
        let bs = List.rev fs
        bs.Head, Queue([], bs.Tail)