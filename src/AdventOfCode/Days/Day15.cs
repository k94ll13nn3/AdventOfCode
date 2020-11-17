using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day15 : Day
    {
        public override string Title => "Oxygen System";

        public override string ProcessFirst()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
            int result = FindOxygen(interpreter.Clone(), 1, 1, (0, 0), new HashSet<(int x, int y)>());

            return result.ToString();
        }

        public override string ProcessSecond()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));

            IEnumerable<(int x, int y, long data)> positions = GenerateMap(interpreter.Clone(), 1, (0, 0), new HashSet<(int x, int y, long data)>());

            var map = positions.Where(x => x.data != 0).ToDictionary(p => (p.x, p.y), _ => int.MaxValue);

            (int x, int y, long data) oxygen = positions.First(x => x.data == 2);
            FillOxygen(0, (oxygen.x, oxygen.y), map);

            return map.Max(x => x.Value).ToString();
        }

        private static HashSet<(int x, int y, long data)> GenerateMap(IntcodeInterpreter interpreter, int direction, (int x, int y) current, HashSet<(int x, int y, long data)> seenPositions)
        {
            interpreter.Run(direction);

            seenPositions.Add((current.x, current.y, interpreter.Outputs[0]));
            if (interpreter.Outputs.Count > 0 && interpreter.Outputs[0] == 0)
            {
                return seenPositions;
            }

            var possiblePositions = new (int x, int y, int direction)[]
            {
                (current.x, current.y + 1, 1),
                (current.x, current.y - 1, 2),
                (current.x - 1, current.y, 3),
                (current.x + 1, current.y, 4)
            };

            var newSeenPositions = seenPositions.ToHashSet();
            var positions = new HashSet<(int x, int y, long data)>();
            foreach ((int x, int y, int direction) possiblePosition in possiblePositions.Where(p => !seenPositions.Any(x => (x.x, x.y) == (p.x, p.y))))
            {
                positions.UnionWith(GenerateMap(interpreter.Clone(), possiblePosition.direction, new(possiblePosition.x, possiblePosition.y), newSeenPositions));
            }

            return positions;
        }

        private static int FindOxygen(IntcodeInterpreter interpreter, int direction, int currentMoves, (int x, int y) current, HashSet<(int x, int y)> seenPositions)
        {
            interpreter.Run(direction);

            if (interpreter.Outputs.Count > 0 && interpreter.Outputs[0] == 2)
            {
                return currentMoves;
            }

            if (interpreter.Outputs.Count > 0 && interpreter.Outputs[0] == 0)
            {
                return int.MaxValue;
            }

            var possiblePositions = new (int x, int y, int direction)[]
            {
                (current.x, current.y + 1, 1),
                (current.x, current.y - 1, 2),
                (current.x - 1, current.y, 3),
                (current.x + 1, current.y, 4)
            };

            var newSeenPositions = seenPositions.ToHashSet();
            newSeenPositions.Add(current);
            int result = int.MaxValue;
            foreach ((int x, int y, int direction) possiblePosition in possiblePositions.Where(p => !seenPositions.Contains(new(p.x, p.y))))
            {
                int movesForDirection = FindOxygen(interpreter.Clone(), possiblePosition.direction, currentMoves + 1, new(possiblePosition.x, possiblePosition.y), newSeenPositions);
                result = Math.Min(result, movesForDirection);
            }

            return result;
        }

        private static void FillOxygen(int currentMoves, (int x, int y) current, Dictionary<(int, int), int> seenPositions)
        {
            var possiblePositions = new (int x, int y)[]
            {
                (current.x, current.y + 1),
                (current.x, current.y - 1),
                (current.x - 1, current.y),
                (current.x + 1, current.y)
            };

            if (seenPositions.ContainsKey(current) && seenPositions[current] > currentMoves)
            {
                seenPositions[current] = currentMoves;

                foreach ((int x, int y) in possiblePositions)
                {
                    FillOxygen(currentMoves + 1, new(x, y), seenPositions);
                }
            }
        }
    }
}
