using System;
using System.Collections.Generic;

namespace AdventOfCode.Days
{
    public class IntcodeInterpreter
    {
        public const string Stopped = nameof(Stopped);
        public const string NotStarted = nameof(NotStarted);
        public const string InputNeeded = nameof(InputNeeded);

        private readonly long[] _program;
        private long _relativeBase;

        public IntcodeInterpreter(long[] program)
        {
            _ = program ?? throw new ArgumentNullException(nameof(program));

            _program = new long[100000];
            program.CopyTo(_program, 0);
        }

        public List<long> Outputs { get; } = new();
        public string State { get; set; } = NotStarted;
        public long Cursor { get; private set; }

        public long this[int index] => _program[index];

        public void Run(long input)
        {
            Run(new Queue<long>(new[] { input }));
        }

        public void Run(Queue<long>? inputs)
        {
            Outputs.Clear();
            long par1 = 0;
            long par2 = 0;
            while (true)
            {
                string instruction = _program[Cursor].ToString().PadLeft(5, '0');
                switch (instruction[^2..])
                {
                    case "99":
                        State = Stopped;
                        return;

                    case "01":
                        par1 = GetParameter(instruction, 1);
                        par2 = GetParameter(instruction, 2);
                        _program[GetPosition(instruction, 3)] = par1 + par2;
                        Cursor += 4;
                        break;

                    case "02":
                        par1 = GetParameter(instruction, 1);
                        par2 = GetParameter(instruction, 2);
                        _program[GetPosition(instruction, 3)] = par1 * par2;
                        Cursor += 4;
                        break;

                    case "03":
                        if (inputs?.Count > 0)
                        {
                            _program[GetPosition(instruction, 1)] = inputs.Dequeue();
                            Cursor += 2;
                        }
                        else
                        {
                            State = InputNeeded;
                            return;
                        }

                        break;

                    case "04":
                        Outputs.Add(GetParameter(instruction, 1));
                        Cursor += 2;
                        break;

                    case "05":
                        par1 = GetParameter(instruction, 1);
                        par2 = GetParameter(instruction, 2);
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
                        par1 = GetParameter(instruction, 1);
                        par2 = GetParameter(instruction, 2);
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
                        par1 = GetParameter(instruction, 1);
                        par2 = GetParameter(instruction, 2);
                        _program[GetPosition(instruction, 3)] = par1 < par2 ? 1 : 0;
                        Cursor += 4;
                        break;

                    case "08":
                        par1 = GetParameter(instruction, 1);
                        par2 = GetParameter(instruction, 2);
                        _program[GetPosition(instruction, 3)] = par1 == par2 ? 1 : 0;
                        Cursor += 4;
                        break;

                    case "09":
                        par1 = GetParameter(instruction, 1);
                        _relativeBase += par1;
                        Cursor += 2;
                        break;
                }
            }

            throw new InvalidOperationException("Program incomplete");
        }

        private long GetParameter(string instruction, int parameterNumber)
        {
            return instruction[^(2 + parameterNumber)] switch
            {
                '0' => _program[_program[Cursor + parameterNumber]],
                '1' => _program[Cursor + parameterNumber],
                '2' => _program[_program[Cursor + parameterNumber] + _relativeBase],
                _ => throw new InvalidOperationException(),
            };
        }

        private long GetPosition(string instruction, int parameterNumber)
        {
            return instruction[^(2 + parameterNumber)] switch
            {
                '0' => _program[Cursor + parameterNumber],
                '2' => _program[Cursor + parameterNumber] + _relativeBase,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
