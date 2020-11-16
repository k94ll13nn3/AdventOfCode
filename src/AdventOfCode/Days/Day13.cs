namespace AdventOfCode.Days
{
    public class Day13 : Day
    {
        public override string Title => "Care Package";

        public override string ProcessFirst()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
            interpreter.Run(0);

            int count = 0;
            for (int i = 2; i < interpreter.Outputs.Count; i += 3)
            {
                if (interpreter.Outputs[i] == 2)
                {
                    count++;
                }
            }

            return count.ToString();
        }

        public override string ProcessSecond()
        {
            long[] program = GetContentAsLongArray(',');
            program[0] = 2;
            var interpreter = new IntcodeInterpreter(program);

            long input = 0;
            while (interpreter.State != IntcodeInterpreter.Stopped)
            {
                interpreter.Run(input);

                long ball = 0;
                long paddle = 0;
                for (int i = 2; i < interpreter.Outputs.Count; i += 3)
                {
                    if (interpreter.Outputs[i] == 4)
                    {
                        ball = interpreter.Outputs[i - 2];
                    }

                    if (interpreter.Outputs[i] == 3)
                    {
                        paddle = interpreter.Outputs[i - 2];
                    }
                }

                input = ball.CompareTo(paddle);
            }

            long result = 0;
            for (int i = 0; i < interpreter.Outputs.Count; i += 3)
            {
                if (interpreter.Outputs[i] == -1 && interpreter.Outputs[i + 1] == 0)
                {
                    result = interpreter.Outputs[i + 2];
                }
            }

            return result.ToString();
        }
    }
}
