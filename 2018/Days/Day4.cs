using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day4 : Day
    {
        public override string Title => "Repose Record";

        public override string ProcessFirst()
        {
            var lines = GetLinesAsStrings();
            var entries = lines.Select(x => ParseEntry(x)).OrderBy(x => x.time);
            var numberOfGuards = entries.Select(x => x.id).Distinct().Count();

            var times = new Dictionary<int, int[]>();

            var currentId = 0;
            var lastTimeAwake = 0;
            foreach (var entry in entries)
            {
                if (entry.action == 0)
                {
                    currentId = entry.id;
                    if (!times.ContainsKey(currentId))
                    {
                        times[currentId] = new int[60];
                    }
                }
                else if (entry.action == 1)
                {
                    lastTimeAwake = entry.time.Minute;
                }
                else
                {
                    for (int i = lastTimeAwake; i < entry.time.Minute; i++)
                    {
                        times[currentId][i]++;
                    }
                }
            }

            var mostAsleepGuard = times.OrderByDescending(x => x.Value.Sum()).First();
            var mostAsleepMinute = mostAsleepGuard.Value.Select((x, i) => (x, i)).OrderByDescending(x => x.x).First().i;

            return (mostAsleepMinute * mostAsleepGuard.Key).ToString();
        }

        public override string ProcessSecond()
        {
            var lines = GetLinesAsStrings();
            var entries = lines.Select(x => ParseEntry(x)).OrderBy(x => x.time);
            var numberOfGuards = entries.Select(x => x.id).Distinct().Count();

            var times = new Dictionary<int, int[]>();

            var currentId = 0;
            var lastTimeAwake = 0;
            foreach (var entry in entries)
            {
                if (entry.action == 0)
                {
                    currentId = entry.id;
                    if (!times.ContainsKey(currentId))
                    {
                        times[currentId] = new int[60];
                    }
                }
                else if (entry.action == 1)
                {
                    lastTimeAwake = entry.time.Minute;
                }
                else
                {
                    for (int i = lastTimeAwake; i < entry.time.Minute; i++)
                    {
                        times[currentId][i]++;
                    }
                }
            }

            var minutes = new (int minute, int guard, int numberOfTimesAsleep)[60];
            for (int i = 0; i < 60; i++)
            {
                minutes[i] = (0, 0, 0);
                foreach (var time in times)
                {
                    if (time.Value[i] > minutes[i].numberOfTimesAsleep)
                    {
                        minutes[i] = (i, time.Key, time.Value[i]);
                    }
                }
            }

            var guard = minutes.OrderByDescending(x => x.numberOfTimesAsleep).First();

            return (guard.minute * guard.guard).ToString();
        }

        public (DateTime time, int id, int action) ParseEntry(string line)
        {
            // [1518-10-05 00:10] falls asleep
            var parts = line.Split(new[] { ']', '[' }, StringSplitOptions.RemoveEmptyEntries);
            var time = DateTime.Parse(parts[0]);
            var action = 0;
            var id = 0;

            switch (parts[1].Trim())
            {
                case "falls asleep":
                    action = 1;
                    break;
                case "wakes up":
                    action = 2;
                    break;
            }

            if (action == 0)
            {
                id = int.Parse(parts[1].Split(new[] { ' ', '#' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }

            return (time, id, action);
        }
    }
}