using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day04
    {
        public static (int, int) Solve1(List<string> data)
        {
            var totalsByGuardAndMinute = Analyse(data);

            var totalsByGuard = totalsByGuardAndMinute.Keys.ToDictionary(k => k, k => totalsByGuardAndMinute[k].Values.Sum());
            var max = totalsByGuard.Max(kvp => kvp.Value);
            var guardWithMax = totalsByGuard.Single(kvp => kvp.Value == max).Key;
            var max2 = totalsByGuardAndMinute[guardWithMax].Max(kvp => kvp.Value);
            var minuteWithMax = totalsByGuardAndMinute[guardWithMax].Single(kvp => kvp.Value == max2).Key;

            return (guardWithMax, minuteWithMax);
        }

        public static (int, int) Solve2(List<string> data)
        {
            var totalsByGuardAndMinute = Analyse(data);

            var max = totalsByGuardAndMinute.SelectMany(kvp => kvp.Value.Values).Max();
            var guardWithMax = totalsByGuardAndMinute.Single(kvp => kvp.Value.ContainsValue(max));
            var minuteWithMax = guardWithMax.Value.Single(kvp => kvp.Value == max).Key;

            return (guardWithMax.Key, minuteWithMax);
        }

        private static Record Parse(string line, int previousId)
        {
            var s1 = line.Split("] ");
            var date = DateTime.Parse(s1[0].Substring(1));
            var eventType = GetEventType(s1[1]);

            var id = previousId;
            if (eventType == EventType.StartedShift)
            {
                id = int.Parse(s1[1].Split(" ")[1].Substring(1));
            }

            return new Record
            {
                GuardId = id,
                Timestamp = date,
                EventType = eventType,
            };
        }

        private static Dictionary<int, Dictionary<int, int>> Analyse(List<string> data)
        {
            var records = data.Select(Parse).ToList();
            var totalsByGuardAndMinute = new Dictionary<int, Dictionary<int, int>>();

            records = records.OrderBy(r => r.Timestamp).ToList();

            var currentGuardId = 0;
            var fellAsleepAt = DateTime.MinValue;
            foreach (var record in records)
            {
                switch (record.EventType)
                {
                    case EventType.StartedShift:
                        currentGuardId = record.GuardId;
                        break;
                    case EventType.FellAsleep:
                        fellAsleepAt = record.Timestamp;
                        break;
                    case EventType.WokeUp:
                        var currentTime = fellAsleepAt;
                        if (!totalsByGuardAndMinute.ContainsKey(currentGuardId))
                        {
                            totalsByGuardAndMinute[currentGuardId] = new Dictionary<int, int>();
                        }

                        var totalsForGuardByMinute = totalsByGuardAndMinute[currentGuardId];
                        while (currentTime < record.Timestamp)
                        {
                            var minute = currentTime.Minute;
                            if (!totalsForGuardByMinute.ContainsKey(minute))
                            {
                                totalsForGuardByMinute[minute] = 0;
                            }

                            totalsForGuardByMinute[minute]++;
                            currentTime = currentTime.AddMinutes(1);
                        }

                        break;
                }
            }

            return totalsByGuardAndMinute;
        }

        private static EventType GetEventType(string s)
        {
            if (s.Contains("begins"))
                return EventType.StartedShift;
            if (s.Contains("asleep"))
                return EventType.FellAsleep;
            if (s.Contains("wakes"))
                return EventType.WokeUp;
            throw new Exception($"Cannot parse event type from '{s}'");
        }

        private struct Record
        {
            public int GuardId;
            public DateTime Timestamp;
            public EventType EventType;
        }

        private enum EventType
        {
            StartedShift,
            FellAsleep,
            WokeUp
        }
    }
}
