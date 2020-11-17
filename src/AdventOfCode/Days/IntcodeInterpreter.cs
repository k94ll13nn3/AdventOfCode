﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class IntcodeInterpreter
    {
        public const string Stopped = nameof(Stopped);
        public const string NotStarted = nameof(NotStarted);
        public const string InputNeeded = nameof(InputNeeded);

        private long[] _program;
        private long _relativeBase;
        private string _currentInstruction = string.Empty;
        private long _cursor;

        public IntcodeInterpreter(long[] program)
        {
            _ = program ?? throw new ArgumentNullException(nameof(program));

            _program = program.ToArray();
        }

        public List<long> Outputs { get; } = new();

        public string State { get; private set; } = NotStarted;

        public long this[int index] => _program[index];

        public IntcodeInterpreter Clone()
        {
            return new IntcodeInterpreter(_program)
            {
                _relativeBase = _relativeBase,
                _currentInstruction = _currentInstruction,
                _cursor = _cursor,
                State = State,
            };
        }

        public void Run(long input)
        {
            Run(new Queue<long>(new[] { input }));
        }

        public void Run(Queue<long>? inputs)
        {
            Outputs.Clear();
            while (true)
            {
                _currentInstruction = _program[_cursor].ToString().PadLeft(5, '0');
                switch (_currentInstruction[^2..])
                {
                    case "99":
                        State = Stopped;
                        return;

                    case "01":
                        Write(GetValue(1) + GetValue(2), GetPosition(3));
                        _cursor += 4;
                        break;

                    case "02":
                        Write(GetValue(1) * GetValue(2), GetPosition(3));
                        _cursor += 4;
                        break;

                    case "03":
                        if (inputs?.Count > 0)
                        {
                            Write(inputs.Dequeue(), GetPosition(1));
                            _cursor += 2;
                        }
                        else
                        {
                            State = InputNeeded;
                            return;
                        }

                        break;

                    case "04":
                        Outputs.Add(GetValue(1));
                        _cursor += 2;
                        break;

                    case "05":
                        _cursor = GetValue(1) != 0 ? GetValue(2) : _cursor + 3;
                        break;

                    case "06":
                        _cursor = GetValue(1) == 0 ? GetValue(2) : _cursor + 3;
                        break;

                    case "07":
                        Write(GetValue(1) < GetValue(2) ? 1 : 0, GetPosition(3));
                        _cursor += 4;
                        break;

                    case "08":
                        Write(GetValue(1) == GetValue(2) ? 1 : 0, GetPosition(3));
                        _cursor += 4;
                        break;

                    case "09":
                        _relativeBase += GetValue(1);
                        _cursor += 2;
                        break;
                }
            }

            throw new InvalidOperationException("Program incomplete");
        }

        private long GetValue(int parameterNumber)
        {
            long position = GetPosition(parameterNumber);
            EnsureProgramSize(position);
            return _program[position];
        }

        private long GetPosition(int parameterNumber)
        {
            return _currentInstruction[^(2 + parameterNumber)] switch
            {
                '0' => _program[_cursor + parameterNumber],
                '1' => _cursor + parameterNumber,
                '2' => _program[_cursor + parameterNumber] + _relativeBase,
                _ => throw new InvalidOperationException(),
            };
        }

        private void Write(long value, long position)
        {
            EnsureProgramSize(position);
            _program[position] = value;
        }

        private void EnsureProgramSize(long position)
        {
            if (position >= _program.Length)
            {
                long[] array = new long[position + 1];
                _program.CopyTo(array, 0);
                _program = array;
            }
        }
    }
}
