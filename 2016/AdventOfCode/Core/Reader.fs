module Reader

open System.IO

let readLines filePath = 
    File.ReadLines(Path.Combine(@"D:\Developpement\Repos\AdventOfCode\2016\AdventOfCode\Inputs", filePath))

let readInput filePath = 
    File.ReadAllText(Path.Combine(@"D:\Developpement\Repos\AdventOfCode\2016\AdventOfCode\Inputs", filePath))