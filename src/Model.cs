﻿using System;
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

    public class TtsSpeaker : IDisposable
    {
        private System.Speech.Synthesis.SpeechSynthesizer synthesizer;
        public System.Speech.Synthesis.SpeechSynthesizer Synthesizer
        {
            get
            {
                if (this.synthesizer == null)
                {
                    PopulateSynthesizer();
                }

                return this.synthesizer;
            }
        }

        public string Name { get; private set; }
        public int Rate { get; private set; }
        public int Volume { get; private set; }

        public TtsSpeaker(string name, int rate, int volume)
        {
            this.Name = name;
            this.Rate = rate;
            this.Volume = volume;
        }

        private void PopulateSynthesizer()
        {
            this.synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            synthesizer.Rate = this.Rate;
            synthesizer.Volume = this.Volume;
        }

        public void Dispose()
        {
            if (this.synthesizer != null)
            {
                this.synthesizer.Dispose();
                this.synthesizer = null;
            }
        }
    }

    public class ActivityAlert : IComparable<ActivityAlert>
    {
        public double TimeFromStart {
            get {
                return Activity.TimeFromStart - ReminderTimeOffset;
            }  
        }
        public double ReminderTimeOffset { get; set; }
        public AlertSound Sound { get; set; }
        public TtsSpeaker TtsSpeaker { get; set; }
        public string TtsSentence { get; set; }
        public TimelineActivity Activity { get; set; }
        public bool Processed { get; set; }

        public const double TooOldThreshold = 3.0;

        public ActivityAlert()
        {
            Processed = false;
        }
    
        public int CompareTo(ActivityAlert other)
        {
            return TimeFromStart.CompareTo(other.TimeFromStart);
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

        private class CompareByEndTimeKlass : IComparer<TimelineActivity>
        {
            int IComparer<TimelineActivity>.Compare(TimelineActivity x, TimelineActivity y)
            {
                return x.EndTime.CompareTo(y.EndTime);
            }
        }
        static public readonly IComparer<TimelineActivity> CompareByEndTime = new CompareByEndTimeKlass();

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

        List<TimelineActivity> items;
        List<double> itemsEndTime;

        public IEnumerable<TimelineActivity> Items {
            get { return items; }        
        }
        
        private int FindFirstItemIndexAfterEndTime(double t)
        {
            int i = itemsEndTime.BinarySearch(t);
            if (i < 0)
                return ~i;

            for (; i < itemsEndTime.Count && itemsEndTime[i] == t; ++ i)
                ;

            return i;               
        }
        private IEnumerable<TimelineActivity> ItemsBeforeEndTime(double t)
        {
            int itemsToSkip = FindFirstItemIndexAfterEndTime(t);
            return Items.Skip(itemsToSkip);
        }
        public IEnumerable<TimelineActivity> VisibleItemsAt(double t, int limit)
        {
            return (from e in ItemsBeforeEndTime(t)
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
                .GetIntervalsIncludingPoint(t)
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
        List<double> alertsTimeFromStart;
        public IEnumerable<ActivityAlert> Alerts { get { return alerts; } }
        public int FindFirstAlertIndexAfterStartTime(double t)
        {
            int i = alertsTimeFromStart.BinarySearch(t);
            if (i < 0)
                return ~i;

            for (; i < alertsTimeFromStart.Count && alertsTimeFromStart[i] == t; ++ i)
                ;

            return i;
        }
        public IEnumerable<ActivityAlert> PendingAlertsAt(double t)
        {
            int firstAlertIndex = FindFirstAlertIndexAfterStartTime(t - ActivityAlert.TooOldThreshold);
            return alerts
                .Skip(firstAlertIndex)
                .TakeWhile(a => (a.TimeFromStart < t))
                .Where(a => !a.Processed);
        }

        public void ResetAllAlerts()
        {
            foreach (ActivityAlert alert in Alerts)
                alert.Processed = false;
        }

        public AlertSoundAssets AlertSoundAssets { get; private set; }

        public double EndTime { get; private set; }

        public Timeline(string name, List<TimelineActivity> items_, List<TimelineAnchor> anchors_, List<ActivityAlert> alerts_, AlertSoundAssets soundAssets)
        {
            Name = name;
            items = items_;
            int i = 0;
            foreach (TimelineActivity a in items_)
            {
                a.Index = i++;
            }
            items.Sort(TimelineActivity.CompareByEndTime);
            itemsEndTime = items.Select(a => a.EndTime).ToList();

            anchors = anchors_.OrderBy(anchor => anchor.TimeFromStart).ToList();
            anchorsTree = new IntervalTree.IntervalTree<double, TimelineAnchor>();
            foreach (TimelineAnchor a in anchors)
                anchorsTree.Add(a.Interval, a);

            alerts = alerts_;
            alerts.Sort();
            alertsTimeFromStart = alerts.Select(a => a.TimeFromStart).ToList();
            AlertSoundAssets = soundAssets;

            EndTime = Items.Any() ? Items.Last().EndTime : 0;
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
