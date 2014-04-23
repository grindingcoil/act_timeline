using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace hw
{
    public class TimelineLoader
    {
        static public Timeline LoadFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            return LoadFromLines(lines);
        }

        static Regex CommentRegex = new Regex(@"^(#|\s*$)");
        static Regex ActivityRegex = new Regex(@"^(?<time>[\d\.]+)秒?後?\s+(?<name>.+)$");

        static public Timeline LoadFromLines(string[] lines)
        {
            List<TimelineActivity> items = new List<TimelineActivity>();
            foreach (string line in lines)
            {
                // skip comment lines
                if (CommentRegex.IsMatch(line))
                    continue;

                // parse event lines
                Match match = ActivityRegex.Match(line);
                if (match.Success)
                {
                    double time = double.Parse(match.Groups["time"].Value, CultureInfo.InvariantCulture);
                    string name = match.Groups["name"].Value;
                    items.Add(new TimelineActivity { TimeFromStart = time, Name = name });
                    continue;
                }

                Console.WriteLine("failed to parse: "+line);
            }

            return new Timeline(items);
        }
    }
}
