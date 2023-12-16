open Day16
open System.Diagnostics
open ColoredConsole
open Humanizer

printfn $"Day {number}: {title}"
printfn ""

let mutable timer = Stopwatch.StartNew()
let firstPart = computeFirst ()
timer.Stop()

printColoredText $"    {firstPart} " darkGrayColor

printColoredText
    $"({timer.ElapsedMilliseconds.Milliseconds()}ms/{timer.ElapsedTicks} ticks)"
    (timer.ElapsedMilliseconds |> getColorForElapsedTime)

printfn ""

timer <- Stopwatch.StartNew()
let secondPart = computeSecond ()
timer.Stop()

printColoredText $"    {secondPart} " darkGrayColor

printColoredText
    $"({timer.ElapsedMilliseconds.Milliseconds()}ms/{timer.ElapsedTicks} ticks)"
    (timer.ElapsedMilliseconds |> getColorForElapsedTime)

printfn ""
