// stylecop.header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AdventOfCode.Days
{
    internal class Day12 : Day
    {
        public override object ProcessFirst()
        {
            return Regex.Matches(this.Content, @"-?\d+").OfType<Match>().Select(m => int.Parse(m.Value)).Sum();
        }

        public override object ProcessSecond()
        {
            return ComputeSum(this.Content);
        }

        private static int ComputeSum(string json)
        {
            var sum = 0;
            dynamic dynJson;

            if (int.TryParse(json, out sum))
            {
                return sum;
            }

            try
            {
                dynJson = JsonConvert.DeserializeObject(json);
            }
            catch (Exception)
            {
                return 0;
            }

            if (dynJson?.Type?.ToString() == "Array")
            {
                for (int i = 0; i < dynJson.Count; i++)
                {
                    sum += ComputeSum(dynJson[i].ToString());
                }
            }
            else
            {
                dynJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                foreach (var item in dynJson)
                {
                    if (item.Value.ToString() == "red")
                    {
                        return 0;
                    }

                    sum += ComputeSum(item.Value.ToString());
                }
            }

            return sum;
        }
    }
}