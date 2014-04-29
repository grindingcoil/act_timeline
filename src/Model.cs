using System;
using System.Linq;
using System.Collections.Generic;

namespace ACTTimeline
{
    public class ModelException : Exception
    {
        public ModelException() { }
        public ModelException(string message) : base(message) { }
    }

    public class DuplicateAlertSoundAliasException : ModelException
    {
        public DuplicateAlertSoundAliasException(string alias)
            : base(String.Format("AlertSound alias {0} is already used.", alias)) { }
    }

    public class AlertSound
    {
        public string Filename { get; private set; }

        List<string> aliases;
        public IEnumerable<string> Aliases { get { return aliases; } }

        public void AddAlias(string alias)
        {
            aliases.Add(alias);
        }

        public AlertSound(string filename)
        {
            aliases = new List<string>();
            Filename = filename;
        }
    };

    public class AlertSoundAssets
    {
        List<AlertSound> allAlertSounds;
        Dictionary<string, AlertSound> aliasMap;

        public AlertSoundAssets()
        {
            allAlertSounds = new List<AlertSound>();
            aliasMap = new Dictionary<string, AlertSound>();
        }

        public AlertSound Get(string filenameOrAlias)
        {
            AlertSound sound;
            if (aliasMap.TryGetValue(filenameOrAlias, out sound))
            {
                return sound;
            }

            sound = new AlertSound(filenameOrAlias);
            allAlertSounds.Add(sound);
            return sound;
        }

        public void RegisterAlias(AlertSound sound, string alias)
        {
            try
            {
                sound.AddAlias(alias);
                aliasMap.Add(alias, sound);
            }
            catch(ArgumentException){
                throw new DuplicateAlertSoundAliasException(alias);
            }
        }
    };

    public class ActivityAlert
    {
        public double TimeFromStart {
            get {
                return Activity.TimeFromStart - ReminderTimeOffset;
            }  
        }
        public double ReminderTimeOffset { get; set; }
        public AlertSound Sound { get; set; }
        public TimelineActivity Activity { get; set; }
    };

    public class TimelineActivity
    {
        public string Name { get; set; }
        public double TimeFromStart { get; set; }
        public double TimeLeft { get; private set; }

        const double Instant = 0;
        public double Duration { get; set; }

        public TimelineActivity()
        {
            Name = "何かすごい攻撃";
            TimeFromStart = 5;
            TimeLeft = -1;
            Duration = Instant;
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

        List<TimelineActivity> items;
        public IEnumerable<TimelineActivity> Items {
            get { return items; }        
        }
        public IEnumerable<TimelineActivity> VisibleItems(double threshold)
        {
            return from e in Items where e.TimeLeft > threshold select e;
        }

        List<ActivityAlert> alerts;

        public IEnumerable<ActivityAlert> Alerts { get { return alerts; } }

        public AlertSoundAssets AlertSoundAssets { get; private set; }

        public Timeline(List<TimelineActivity> items_, List<ActivityAlert> alerts_, AlertSoundAssets soundAssets)
        {
            currentTime = 0;
            items = items_;
            alerts = alerts_;
            AlertSoundAssets = soundAssets;
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
