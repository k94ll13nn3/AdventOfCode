module Day20

open Helper
open Types

type Point = {X: int64; Y: int64; Z: int64}
type Particle = {Id: int; P: Point; V: Point; A: Point}

let origin = {X = 0L; Y = 0L; Z = 0L}

let parse i line =
    match line with
    | Regex "p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>" [pX; pY; pZ; vX; vY; vZ; aX; aY; aZ] ->
        {Id = i; P = {X = int64 pX; Y = int64 pY; Z = int64 pZ}; V = {X = int64 vX; Y = int64 vY; Z = int64 vZ}; A = {X = int64 aX; Y = int64 aY; Z = int64 aZ}}
    | _ -> failwith "parse - error"

let transformParticle p =
    {p with P = {X = p.P.X + p.V.X + p.A.X; Y = p.P.Y + p.V.Y + p.A.Y; Z = p.P.Z + p.V.Z + p.A.Z}; V = {X = p.A.X + p.V.X; Y = p.A.Y + p.V.Y; Z = p.A.Z + p.V.Z}}

let distance p = abs (p.P.X) + abs (p.P.Y) + abs (p.P.Z)

let baseQueue = List.init 1000 id ==> emptyQueue

let findClosest input =
    let applyTransform particles lastMinParticles = 
        let newParticles = particles |> Array.map transformParticle
        let min = newParticles |> Array.minBy distance
        (newParticles, takeAndAdd min.Id lastMinParticles)
    let rec getMinParticle particles lastMinParticle =
        let (p, m) = applyTransform particles lastMinParticle
        let count = lastMinParticle |> countBy id
        if  count |> List.length = 1 then
            count.Head |> fst
        else 
            getMinParticle p m     
    getMinParticle input baseQueue

let findClosest2 (input:Particle[]) =
    let applyTransform particles lastMinParticles = 
        let newParticles = particles |> Array.map transformParticle
        let n = newParticles |> Array.groupBy (fun p -> p.P) |> Array.filter (fun e -> snd e |> Array.length = 1) |> Array.collect snd
        (n, takeAndAdd (n |> Array.length) lastMinParticles)
    let rec getMinParticle particles lastMinParticle tick =
        let (p, m) = applyTransform particles lastMinParticle
        let count = lastMinParticle |> countBy id
        if  p.Length <> input.Length && count |> List.length = 1 then
            p.Length
        else 
            getMinParticle p m (tick+1)    
    getMinParticle input baseQueue 0

let computeFirst =
    readLines "Input20.txt"
    |> Array.mapi parse
    |> findClosest

let computeSecond =
    readLines "Input20.txt"
    |> Array.mapi parse
    |> findClosest2