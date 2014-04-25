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

        AlertSoundAssets assets;

        List<string> aliases;
        public IEnumerable<string> Aliases { get { return aliases; } }

        public void AddAlias(string alias)
        {
            aliases.Add(alias);
            assets.RegisterAlias(this, alias);
        }

        public AlertSound(AlertSoundAssets assets_, string filename)
        {
            assets = assets_;
            aliases = new List<string>();
            Filename = filename;

            assets.RegisterAlias(this, Filename);
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

            return new AlertSound(this, filenameOrAlias);
        }

        public void RegisterAlias(AlertSound sound, string alias)
        {
            try
            {
                aliasMap.Add(alias, sound);
            }
            catch(ArgumentException){
                throw new DuplicateAlertSoundAliasException(alias);
            }
        }
    };

    public class ActivityAlert
    {
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

        public List<TimelineActivity> Items { get; private set; }
        public IEnumerable<TimelineActivity> VisibleItems(double threshold)
        {
            return from e in Items where e.TimeLeft > threshold select e;
        }

        public AlertSoundAssets AlertSoundAssets { get; private set; }

        public Timeline(List<TimelineActivity> items, AlertSoundAssets soundAssets)
        {
            currentTime = 0;
            Items = items;
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
