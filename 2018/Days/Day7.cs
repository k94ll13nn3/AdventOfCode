using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day7 : Day
    {
        private readonly Regex pattern = new Regex(@"Step (\w+) must be finished before step (\w+) can begin", RegexOptions.Compiled);

        public override string Title => "The Sum of Its Parts";

        public override string ProcessFirst()
        {
            var steps = new Dictionary<string, IList<string>>();
            var lines = GetLinesAsStrings();
            foreach (var line in lines)
            {
                var match = pattern.Match(line);
                if (steps.ContainsKey(match.Groups[2].Value))
                {
                    steps[match.Groups[2].Value].Add(match.Groups[1].Value);
                }
                else
                {
                    steps[match.Groups[2].Value] = new List<string> { match.Groups[1].Value };
                }

                if (!steps.ContainsKey(match.Groups[1].Value))
                {
                    steps[match.Groups[1].Value] = new List<string>();
                }
            }

            var result = "";

            while (steps.Keys.Any())
            {
                var currentStep = steps.Where(x => x.Value.Count == 0).OrderBy(x => x.Key).First();
                result += currentStep.Key;
                steps.Remove(currentStep.Key);
                foreach (var step in steps)
                {
                    if (step.Value.Contains(currentStep.Key))
                    {
                        step.Value.Remove(currentStep.Key);
                    }
                }
            }

            return result;
        }

        public override string ProcessSecond()
        {
            var steps = new Dictionary<string, IList<string>>();
            var lines = GetLinesAsStrings();
            foreach (var line in lines)
            {
                var match = pattern.Match(line);
                if (steps.ContainsKey(match.Groups[2].Value))
                {
                    steps[match.Groups[2].Value].Add(match.Groups[1].Value);
                }
                else
                {
                    steps[match.Groups[2].Value] = new List<string> { match.Groups[1].Value };
                }

                if (!steps.ContainsKey(match.Groups[1].Value))
                {
                    steps[match.Groups[1].Value] = new List<string>();
                }
            }

            var time = 0;
            var workers = new List<(int time, string step)> { (0, ""), (0, ""), (0, ""), (0, ""), (0, "") };

            while (steps.Keys.Any())
            {
                foreach (var worker in workers.Where(x => x.time == 1))
                {
                    foreach (var step in steps)
                    {
                        if (step.Value.Contains(worker.step))
                        {
                            step.Value.Remove(worker.step);
                        }
                    }
                }


                for (int i = 0; i < workers.Count; i++)
                {
                    workers[i] = workers[i].time <= 1 ? (0, "") : (workers[i].time - 1, workers[i].step);
                }

                var currentStep = steps.Where(x => x.Value.Count == 0).OrderBy(x => x.Key).FirstOrDefault();
                while (currentStep.Key != null && workers.Any(x => x.time == 0))
                {
                    steps.Remove(currentStep.Key);

                    var firstWorkerAvailable = workers.IndexOf((0, ""));
                    workers[firstWorkerAvailable] = (currentStep.Key[0] - 4, currentStep.Key);
                    currentStep = steps.Where(x => x.Value.Count == 0).OrderBy(x => x.Key).FirstOrDefault();
                }

                time++;
            }

            return (time - 1 + workers.Max(x => x.time)).ToString();
        }
    }
}