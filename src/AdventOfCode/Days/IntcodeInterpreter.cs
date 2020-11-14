using System;
using System.Collections.Generic;

namespace AdventOfCode.Days
{
    public class IntcodeInterpreter
    {
        public const string Stopped = nameof(Stopped);
        public const string NotStarted = nameof(NotStarted);
        public const string InputNeeded = nameof(InputNeeded);

        private readonly int[] _program;

        public IntcodeInterpreter(int[] program)
        {
            _program = program;
        }

        public List<int> Outputs { get; } = new();

        public string State { get; set; } = NotStarted;

        public int Cursor { get; private set; }

        public void Run(int input)
        {
            Run(new Queue<int>(new[] { input }));
        }

        public void Run(Queue<int>? inputs)
        {
            Outputs.Clear();
            int par1 = 0;
            int par2 = 0;
            while (true)
            {
                string instruction = _program[Cursor].ToString().PadLeft(5, '0');
                switch (instruction[^2..])
                {
                    case "99":
                        State = Stopped;
                        return;

                    case "01":
                        par1 = instruction[^3] == '0' ? _program[_program[Cursor + 1]] : _program[Cursor + 1];
                        par2 = instruction[^4] == '0' ? _program[_program[Cursor + 2]] : _program[Cursor + 2];
                        _program[_program[Cursor + 3]] = par1 + par2;
                        Cursor += 4;
                        break;

                    case "02":
                        par1 = instruction[^3] == '0' ? _program[_program[Cursor + 1]] : _program[Cursor + 1];
                        par2 = instruction[^4] == '0' ? _program[_program[Cursor + 2]] : _program[Cursor + 2];
                        _program[_program[Cursor + 3]] = par1 * par2;
                        Cursor += 4;
                        break;

                    case "03":
                        if (inputs?.Count > 0)
                        {
                            _program[_program[Cursor + 1]] = inputs.Dequeue();
                            Cursor += 2;
                        }
                        else
                        {
                            State = InputNeeded;
                            return;
                        }

                        break;

                    case "04":
                        Outputs.Add(instruction[^3] == '0' ? _program[_program[Cursor + 1]] : _program[Cursor + 1]);
                        Cursor += 2;
                        break;

                    case "05":
                        par1 = instruction[^3] == '0' ? _program[_program[Cursor + 1]] : _program[Cursor + 1];
                        par2 = instruction[^4] == '0' ? _program[_program[Cursor + 2]] : _program[Cursor + 2];
                        if (par1 != 0)
                        {
                            Cursor = par2;
                        }
                        else
                        {
                            Cursor += 3;
                        }

                        break;

                    case "06":
                        par1 = instruction[^3] == '0' ? _program[_program[Cursor + 1]] : _program[Cursor + 1];
                        par2 = instruction[^4] == '0' ? _program[_program[Cursor + 2]] : _program[Cursor + 2];
                        if (par1 == 0)
                        {
                            Cursor = par2;
                        }
                        else
                        {
                            Cursor += 3;
                        }

                        break;

                    case "07":
                        par1 = instruction[^3] == '0' ? _program[_program[Cursor + 1]] : _program[Cursor + 1];
                        par2 = instruction[^4] == '0' ? _program[_program[Cursor + 2]] : _program[Cursor + 2];
                        _program[_program[Cursor + 3]] = par1 < par2 ? 1 : 0;
                        Cursor += 4;
                        break;

                    case "08":
                        par1 = instruction[^3] == '0' ? _program[_program[Cursor + 1]] : _program[Cursor + 1];
                        par2 = instruction[^4] == '0' ? _program[_program[Cursor + 2]] : _program[Cursor + 2];
                        _program[_program[Cursor + 3]] = par1 == par2 ? 1 : 0;
                        Cursor += 4;
                        break;
                }
            }

            throw new InvalidOperationException("Program incomplete");
        }
    }
}
