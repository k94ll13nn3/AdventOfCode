module Container

type Container<'a> =
    | Queue of 'a list * 'a list
    | Stack of 'a list

let emptyQueue = Queue([], [])

let emptyStack = Stack([])

let add q e =
    match q with
    | Queue(fs, bs) -> Queue(e :: fs, bs)
    | Stack(l) -> Stack(e :: l)

let isEmpty q =
    match q with
    | Queue(_) -> q = emptyQueue
    | Stack(_) -> q = emptyStack

let rec addList q (e: 'a list) =
    match e with
    | [] -> q
    | [ a ] -> add q a
    | h :: t -> let newContainer = add q h in addList newContainer t

let take q =
    match q with
    | Queue([], []) -> failwith "take - err"
    | Stack([]) -> failwith "take - err"
    | Stack(h :: t) -> h, Stack(t)
    | Queue(fs, b :: bs) -> b, Queue(fs, bs)
    | Queue(fs, []) ->
        let bs = List.rev fs
        bs.Head, Queue([], bs.Tail)

let peek q =
    match q with
    | Queue([], []) -> None
    | Stack([]) -> None
    | Stack(h :: t) -> Some h
    | Queue(fs, b :: bs) -> Some b
    | Queue(fs, []) ->
        let bs = List.rev fs
        Some bs.Head

let length q =
    match q with
    | Queue(fs, bs) -> List.length (fs @ bs)
    | Stack(l) -> List.length l

let countBy projection q =
    match q with
    | Queue(fs, bs) -> List.countBy projection (fs @ bs)
    | Stack(l) -> List.countBy projection l

let takeAndAdd e q =
    let (_, newContainer) = take q in add newContainer e

let (-->) e q = add q e
let (==>) e q = addList q e
