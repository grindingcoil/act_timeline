using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            : base(String.Format("AlertSound alias \"{0}\" is already used.", alias)) { }
    }
    public class ResourceNotFoundException : ModelException
    {
        public ResourceNotFoundException(string alias)
            : base(String.Format("Resource \"{0}\" could not be found.", alias)) { }
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

            if (!System.IO.File.Exists(filename))
                throw new ResourceNotFoundException(filename);
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

            // not alias => must be filename

            if (!File.Exists(filenameOrAlias))
            {
                // try prepending resource path
                string filenameWithResourcePath = String.Format("{0}/{1}", Globals.SoundFilesRoot, filenameOrAlias);
                if (!File.Exists(filenameWithResourcePath))
                    throw new ResourceNotFoundException(filenameOrAlias);

                sound = Get(filenameWithResourcePath);
                // register filepath without the resource path as an alias
                RegisterAlias(sound, filenameOrAlias);
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
            catch(ArgumentException) {
                throw new DuplicateAlertSoundAliasException(alias);
            }
        }

        public IEnumerable<AlertSound> All
        {
            get { return allAlertSounds; }
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
        public bool Processed { get; set; }

        public const double TooOldThreshold = 3.0;

        public ActivityAlert()
        {
            Processed = false;
        }
    };

    public class TimelineAnchor
    {
        public double TimeFromStart { get; set; }
        public Regex Regex { get; set; }
        public double Window { get; set; }

        public const double DefaultWindow = 5.0;

        public TimelineAnchor()
        {
            Window = DefaultWindow;
        }

        public bool ActiveAt(double t)
        {
            return (TimeFromStart - Window / 2) < t && t < (TimeFromStart + Window / 2);
        }
    };

    public class TimelineActivity
    {
        public string Name { get; set; }
        public double TimeFromStart { get; set; }

        const double Instant = 0;
        public double Duration { get; set; }

        public double EndTime
        {
            get
            {
                return TimeFromStart + Duration;
            }
        }

        // for TimeLeft{Column,Cell}
        public TimelineActivity Self { get { return this; } }

        public TimelineActivity()
        {
            Name = "何かすごい攻撃";
            TimeFromStart = 5;
            Duration = Instant;
        }
    }

    public class Timeline
    {
        public string Name { get; private set; }

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

                if (currentTime == 0)
                {
                    foreach (ActivityAlert alert in alerts)
                        alert.Processed = false;
                }
            }
        }

        List<TimelineActivity> items;
        public IEnumerable<TimelineActivity> Items {
            get { return items; }        
        }
        public IEnumerable<TimelineActivity> VisibleItemsAt(double t)
        {
            return from e in Items where e.EndTime > t select e;
        }

        List<TimelineAnchor> anchors;
        public IEnumerable<TimelineAnchor> Anchors
        {
            get { return anchors; }
        }
        public IEnumerable<TimelineAnchor> ActiveAnchorsAt(double t)
        {
            return from a in anchors where a.ActiveAt(t) select a;
        }

        List<ActivityAlert> alerts;

        public IEnumerable<ActivityAlert> Alerts { get { return alerts; } }
        public IEnumerable<ActivityAlert> PendingAlerts
        {
            get
            {
                return from alert in alerts
                       where alert.TimeFromStart < CurrentTime
                       where CurrentTime - ActivityAlert.TooOldThreshold < alert.TimeFromStart 
                       where !alert.Processed
                       select alert;
            }
        }

        public AlertSoundAssets AlertSoundAssets { get; private set; }

        public double EndTime { get; private set; }

        public Timeline(string name, List<TimelineActivity> items_, List<TimelineAnchor> anchors_, List<ActivityAlert> alerts_, AlertSoundAssets soundAssets)
        {
            Name = name;
            currentTime = 0;
            items = items_.OrderBy(activity => activity.TimeFromStart).ToList();
            anchors = anchors_.OrderBy(anchor => anchor.TimeFromStart).ToList();
            alerts = alerts_;
            AlertSoundAssets = soundAssets;
            EndTime = items.Last().EndTime;
        }
    }

    public class RelativeClock
    {
        private long baseTicks;

        static private long currentTick() { return DateTime.Now.Ticks; }

        public RelativeClock()
        {
            CurrentTime = 0;
        }

        public double CurrentTime
        {
            get
            {
                long now = currentTick();
                TimeSpan elapsed = new TimeSpan(now - baseTicks);
                return elapsed.TotalSeconds;
            }
            set
            {
               baseTicks = currentTick() - (long)(value * TimeSpan.TicksPerSecond);
            }
        }
    }
}
