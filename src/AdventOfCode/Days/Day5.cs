namespace AdventOfCode.Days
{
    public class Day5 : Day
    {
        public override string Title => "Sunny with a Chance of Asteroids";

        public override string ProcessFirst()
        {
            return IntcodeInterpreter.Run(GetContentAsIntArray(','), 1).ToString();
        }

        public override string ProcessSecond()
        {
            return IntcodeInterpreter.Run(GetContentAsIntArray(','), 5).ToString();
        }
    }
}
