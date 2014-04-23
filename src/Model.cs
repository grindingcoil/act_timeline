using System;
using System.Linq;
using System.Collections.Generic;

namespace ACTTimeline
{
    public class TimelineActivity
    {
        public string Name { get; set; }
        public double TimeFromStart { get; set; }
        public double TimeLeft { get; private set; }

        public TimelineActivity()
        {
            Name = "何かすごい攻撃";
            TimeFromStart = 5;
            TimeLeft = -1;
        }

        public void UpdateTimeLeft(double currentTime)
        {
            TimeLeft = TimeFromStart - currentTime;
        }
    }

    public class Timeline
    {
        private double currentTime;

        public double CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                currentTime = value;
                foreach (TimelineActivity e in Items)
                {
                    e.UpdateTimeLeft(currentTime);
                }
            }
        }

        public List<TimelineActivity> Items { get; private set; }
        public IEnumerable<TimelineActivity> VisibleItems(double threshold)
        {
            return from e in Items where e.TimeLeft > threshold select e;
        }

        public Timeline(List<TimelineActivity> items)
        {
            Items = items;
            currentTime = 0;
        }
    }

    public class RelativeClock
    {
        private long baseTicks;

        public RelativeClock()
        {
            Set(0);
        }

        static private long currentTick() { return DateTime.Now.Ticks; }

        public double CurrentTime()
        {
            long now = currentTick();
            TimeSpan elapsed = new TimeSpan(now - baseTicks);
            return elapsed.TotalSeconds;
        }

        public void Set(double time)
        {
            baseTicks = currentTick() - (long)(time * TimeSpan.TicksPerSecond);
        }
    }
}
