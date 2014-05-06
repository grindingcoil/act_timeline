using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ACTTimeline
{
    using TimelineInterval = IntervalTree.Interval<double>;

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
        public double WindowBefore { get; set; }
        public double WindowAfter { get; set; }
        public double Window
        {
            set
            {
                WindowBefore = value / 2;
                WindowAfter = value / 2;
            }
        }

        public TimelineInterval Interval
        {
            get
            {
                return new TimelineInterval(TimeFromStart - WindowBefore, TimeFromStart + WindowAfter);
            }
        }

        public const double DefaultWindow = 5.0;

        public TimelineAnchor()
        {
            Window = DefaultWindow;
        }

        public bool ActiveAt(double t)
        {
            return (TimeFromStart - WindowBefore) < t && t < (TimeFromStart + WindowAfter);
        }
    };

    public class TimelineActivity
    {
        public int Index { get; set; }
        const int IndexNotYetSet = -1;

        public string Name { get; set; }
        public double TimeFromStart { get; set; }

        const double Instant = 0.1;
        public double Duration { get; set; }

        public bool Hidden { get; set; }

        public double EndTime
        {
            get
            {
                return TimeFromStart + Duration;
            }
        }

        public TimelineInterval Interval
        {
            get
            {
                return new TimelineInterval(TimeFromStart, TimeFromStart + Duration);
            }
        }

        // for TimeLeft{Column,Cell}
        public TimelineActivity Self { get { return this; } }

        public TimelineActivity()
        {
            Index = IndexNotYetSet;
            Name = "何かすごい攻撃";
            TimeFromStart = 5;
            Duration = Instant;
            Hidden = false;
        }
    }

    public class Timeline
    {
        public string Name { get; private set; }

        SortedList<double, TimelineActivity> items;

        public IEnumerable<TimelineActivity> Items {
            get { return items.Values; }        
        }
        
        private int FindLastItemIndexBeforeEndTime(double t)
        {
            int l = 0;
            int h = items.Count;
            
            int m = 0;
            while (h > l)
            {
                m = l + (h - l) / 2;

                if (items.ElementAt(m).Key < t)
                    l = m + 1;
                else
                    h = m;
            }
            return l;
        }
        private IEnumerable<TimelineActivity> ItemsAfterEndTime(double t)
        {
            int itemsToSkip = FindLastItemIndexBeforeEndTime(t);
            return Items.Skip(itemsToSkip);
        }
        public IEnumerable<TimelineActivity> VisibleItemsAt(double t, int limit)
        {
            return (from e in ItemsAfterEndTime(t)
                    where !e.Hidden
                    select e).Take(limit);
        }

        List<TimelineAnchor> anchors;
        IntervalTree.IntervalTree<double, TimelineAnchor> anchorsTree;
        public IEnumerable<TimelineAnchor> Anchors
        {
            get { return anchors; }
        }
        public IEnumerable<TimelineAnchor> ActiveAnchorsAt(double t)
        {
            return anchorsTree
                .GetIntervalsOverlappingWith(new TimelineInterval(t, t + 0.1))
                .Select(kv => kv.Value);
        }
        public TimelineAnchor FindAnchorMatchingLogline(double t, string line)
        {
            foreach (TimelineAnchor anchor in ActiveAnchorsAt(t))
            {
                if (anchor.Regex.IsMatch(line))
                    return anchor;
            }

            return null;
        }

        List<ActivityAlert> alerts;

        public IEnumerable<ActivityAlert> Alerts { get { return alerts; } }

        public IEnumerable<ActivityAlert> PendingAlertsAt(double t)
        {
            return from alert in alerts
                   where alert.TimeFromStart < t
                   where t - ActivityAlert.TooOldThreshold < alert.TimeFromStart
                   where !alert.Processed
                   select alert;
        }

        public void ResetAllAlerts()
        {
            foreach (ActivityAlert alert in alerts)
                alert.Processed = false;
        }

        public AlertSoundAssets AlertSoundAssets { get; private set; }

        public double EndTime { get; private set; }

        public Timeline(string name, List<TimelineActivity> items_, List<TimelineAnchor> anchors_, List<ActivityAlert> alerts_, AlertSoundAssets soundAssets)
        {
            Name = name;
            items = new SortedList<double, TimelineActivity>();
            int i = 0;
            foreach (TimelineActivity a in items_)
            {
                a.Index = i++;
                items.Add(a.EndTime, a);
            }

            anchors = anchors_.OrderBy(anchor => anchor.TimeFromStart).ToList();
            anchorsTree = new IntervalTree.IntervalTree<double, TimelineAnchor>();
            foreach (TimelineAnchor a in anchors)
                anchorsTree.Add(a.Interval, a);

            alerts = alerts_;
            AlertSoundAssets = soundAssets;

            EndTime = Items.Last().EndTime;
        }
    }

    public class RelativeClock
    {
        System.Diagnostics.Stopwatch sw;
        private double offset;

        public RelativeClock()
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
        }

        public double CurrentTime
        {
            get
            {
                return offset + ((double)sw.ElapsedMilliseconds) / 1000;
            }
            set
            {
                sw.Restart();
                offset = value;
            }
        }
    }
}
