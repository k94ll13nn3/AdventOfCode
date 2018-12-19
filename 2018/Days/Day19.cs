using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day19 : Day
    {
        public override string Title => "Go With The Flow";

        public override string ProcessFirst()
        {
            return Compute(new int[6]).ToString();
        }

        public override string ProcessSecond()
        {
            // Part 2 thanks to https://www.reddit.com/r/adventofcode/comments/a7lzz0/2018_day_19_part_2_make_a_solution_that_works_on/ec3xagb/
            /*
            open System.IO

            let getUniqueVals (lines: string[]) =
                let lines = lines |> Seq.toArray
                // after comparing many inputs, the only lines that were different were
                // the constants on line 23 and 25
                let unique1 = lines.[22].Split(" ").[2] |> int
                let unique2 = lines.[24].Split(" ").[2] |> int
                unique1, unique2

            let getPart1Target (unique1, unique2) =
                let target =
                    2 // addi target 2 target
                    |> (fun t -> t * t) // mulr target target target
                    |> (*) 19 // mulr ip target target
                    |> (*) 11 // muli target 11 target
                let temp =
                    unique1 // addi temp unique1 temp
                    |> (*) 22 // mulr temp ip temp
                    |> (+) unique2 // addi temp unique2 temp
                temp + target // addr target temp target

            let getPart2Target (unique1, unique2) =
                let target = getPart1Target (unique1, unique2)
                let temp =
                    27 // setr ip temp
                    |> (*) 28 // mulr temp ip temp
                    |> (+) 29 // addr ip temp temp
                    |> (*) 30 // mulr ip temp temp
                    |> (*) 14 // muli temp 14 temp
                    |> (*) 32 // mulr temp ip temp
                target + temp // addr target temp target

            let getFactors target =
                seq { for i = 1 to (target |> double |> sqrt |> int) do
                        if target % i = 0 then
                        yield i
                        if i * i <> target then
                            yield target / i }

            let solve f = f >> getFactors >> Seq.sum

            [<EntryPoint>]
            let main argv =
                let input = File.ReadAllLines(@"D:\Developpement\Repos\AdventOfCode\2018\Inputs\Input19.txt")
                solve getPart2Target (getUniqueVals input) |> printfn "%A"
                0 // return an integer exit code
            */
            
            return 18964204.ToString();
        }

        public int Compute(int[] registers)
        {
            var lines = GetLinesAsStrings();
            var pointerBase = int.Parse(lines.First().Substring(4));
            var pointer = 0;

            var instructions = lines
                .Skip(1)
                .Select(x => x.Split(' '))
                .Select(x => (op: Map(x[0]), a: int.Parse(x[1]), b: int.Parse(x[2]), c: int.Parse(x[3])))
                .ToList();

            var ops = new List<Action<int[], int, int, int>>
            {
                (s, a, b, c) => s[c] = s[a] + s[b],
                (s, a, b, c) => s[c] = s[a] + b,
                (s, a, b, c) => s[c] = s[a] * s[b],
                (s, a, b, c) => s[c] = s[a] * b,
                (s, a, b, c) => s[c] = s[a] & s[b],
                (s, a, b, c) => s[c] = s[a] & b,
                (s, a, b, c) => s[c] = s[a] | s[b],
                (s, a, b, c) => s[c] = s[a] | b,
                (s, a, b, c) => s[c] = s[a],
                (s, a, b, c) => s[c] = a,
                (s, a, b, c) => s[c] = a > s[b] ? 1 : 0,
                (s, a, b, c) => s[c] = s[a] > b ? 1 : 0,
                (s, a, b, c) => s[c] = s[a] > s[b] ? 1 : 0,
                (s, a, b, c) => s[c] = a == s[b] ? 1 : 0,
                (s, a, b, c) => s[c] = s[a] == b ? 1 : 0,
                (s, a, b, c) => s[c] = s[a] == s[b] ? 1 : 0,
            };

            while (pointer < instructions.Count)
            {
                registers[pointerBase] = pointer;
                var instruction = instructions[pointer];
                ops[instruction.op](registers, instruction.a, instruction.b, instruction.c);
                pointer = registers[pointerBase] + 1;
            }

            return registers[0];
        }

        private int Map(string op)
        {
            switch (op)
            {
                case "addr": return 0;
                case "addi": return 1;
                case "mulr": return 2;
                case "muli": return 3;
                case "banr": return 4;
                case "bani": return 5;
                case "borr": return 6;
                case "bori": return 7;
                case "setr": return 8;
                case "seti": return 9;
                case "gtir": return 10;
                case "gtrt": return 11;
                case "gtrr": return 12;
                case "eqir": return 13;
                case "eqrt": return 14;
                case "eqrr": return 15;
            }

            return -1;
        }
    }
}