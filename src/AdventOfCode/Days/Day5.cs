namespace AdventOfCode.Days
{
    public class Day5 : Day
    {
        public override string Title => "Sunny with a Chance of Asteroids";

        public override string ProcessFirst()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsIntArray(','));
            interpreter.Run(1);
            return interpreter.Outputs[^1].ToString();
        }

        public override string ProcessSecond()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsIntArray(','));
            interpreter.Run(5);
            return interpreter.Outputs[^1].ToString();
        }
    }
}
