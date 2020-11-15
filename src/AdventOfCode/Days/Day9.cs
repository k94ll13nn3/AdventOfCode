namespace AdventOfCode.Days
{
    public class Day9 : Day
    {
        public override string Title => "Sensor Boost";

        public override string ProcessFirst()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
            interpreter.Run(1);

            return interpreter.Outputs[^1].ToString();
        }

        public override string ProcessSecond()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
            interpreter.Run(2);

            return interpreter.Outputs[^1].ToString();
        }
    }
}
