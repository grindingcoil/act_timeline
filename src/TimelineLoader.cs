using System.Collections.Generic;
using System.Globalization;
using Sprache;
using System;

namespace ACTTimeline
{
    public class AlertAll
    {
        public string ActivityName;
        public double ReminderTime;
        public AlertSound AlertSound;
    }

    public class TimelineConfig
    {
        public List<TimelineActivity> Items;
        public List<TimelineAnchor> Anchors;
        public List<AlertAll> AlertAlls;
        public List<string> HideAlls;
        public List<ActivityAlert> Alerts;
        public AlertSoundAssets AlertSoundAssets;

        public TimelineConfig()
        {
            Items = new List<TimelineActivity>();
            Anchors = new List<TimelineAnchor>();
            AlertAlls = new List<AlertAll>();
            HideAlls = new List<string>();
            Alerts = new List<ActivityAlert>();
            AlertSoundAssets = new AlertSoundAssets();
        }
    }

    public class TimelineConfigParser
    {
        const int TRASH = 0;

        public delegate void ConfigOp(TimelineConfig config);

        static readonly Parser<int> Comment = Parse.Regex(@"^#.*").Return(TRASH);

        static readonly Parser<string> QuotedString =
            from quoteStart in Parse.Char('"')
            from quotedString in Parse.CharExcept('"').Many().Text()
            from quoteEnd in Parse.Char('"')
            select quotedString;
        static readonly Parser<string> Spaces = Parse.Regex(@"^[ 　]+");
        static readonly Parser<string> NonWhiteSpaces = Parse.Char(c => !char.IsWhiteSpace(c), "non whitespace char").Many().Text();
        static readonly Parser<string> MaybeQuotedString = QuotedString.XOr(NonWhiteSpaces);

        static readonly Parser<char> RegexEscapedSlash =
            from escape in Parse.Char('\\')
            from slash in Parse.Char('/')
            select '/';
        static readonly Parser<char> RegexChar = RegexEscapedSlash.Or(Parse.CharExcept('/'));
        static readonly Parser<string> Regex =
            from slash in Parse.Char('/')
            from regex in RegexChar.Many().Text()
            from slash2 in Parse.Char('/')
            select regex;

        static readonly Parser<double> Duration = 
            from spaces in Spaces
            from durationPrefix in Parse.String("duration").Or(Parse.String("効果時間"))
            from spaces2 in Parse.Optional(Spaces)
            from value in Parse.Decimal
            select double.Parse(value, CultureInfo.InvariantCulture);
        static readonly Parser<double> SyncWindow =
            from spaces in Spaces
            from window in Parse.String("window")
            from spaces2 in Spaces
            from value in Parse.Decimal
            select double.Parse(value, CultureInfo.InvariantCulture);
        static readonly Parser<Tuple<string, double>> Sync =
            from spaces in Spaces
            from sync in Parse.String("sync")
            from spaces2 in Parse.Optional(Spaces)
            from regex in Regex
            from window in Parse.Optional(SyncWindow)
            select new Tuple<string, double>(regex, window.GetOrElse(TimelineAnchor.DefaultWindow));
        static readonly Parser<Tuple<TimelineActivity, Tuple<string, double>>> TimelineActivity =
            from timeFromStart in Parse.Decimal
            from spaces in Spaces
            from name in MaybeQuotedString
            from duration in Parse.Optional(Duration)
            from sync in Parse.Optional(Sync)
            select new Tuple<TimelineActivity, Tuple<string, double>>(new TimelineActivity {
                TimeFromStart = double.Parse(timeFromStart, CultureInfo.InvariantCulture),
                Name = name,
                Duration = duration.GetOrElse(0)
            }, sync.GetOrElse(null));

        static readonly Parser<ConfigOp> TimelineActivityStatement =
            TimelineActivity.Select<Tuple<TimelineActivity, Tuple<string, double>>, ConfigOp>(t => ((TimelineConfig config) =>
            {
                config.Items.Add(t.Item1);
                if (t.Item2 != null)
                {
                    config.Anchors.Add(new TimelineAnchor { TimeFromStart = t.Item1.TimeFromStart, Regex = new System.Text.RegularExpressions.Regex(t.Item2.Item1), Window = t.Item2.Item2 });
                }
            })).Named("TimelineActivityStatement");
        
        static readonly Parser<Tuple<string, string>> AlertSoundAlias =
            from define_alertsound in Parse.Regex(@"^define\s+alertsound\s+")
            from alias in MaybeQuotedString
            from spaces in Spaces
            from target in MaybeQuotedString
            select new Tuple<string, string>(alias, target);

        static readonly Parser<ConfigOp> AlertSoundAliasStatement =
            AlertSoundAlias.Select<Tuple<string, string>, ConfigOp>((Tuple<string, string> t) => ((TimelineConfig config) => {
                AlertSound alertSound = config.AlertSoundAssets.Get(t.Item2);
                config.AlertSoundAssets.RegisterAlias(alertSound, t.Item1);
            }));

        static readonly Parser<Tuple<string, string, string>> AlertAll =
            from alertall in Parse.String("alertall")
            from spaces in Spaces
            from activityName in MaybeQuotedString
            from spaces2 in Spaces
            from before in Parse.String("before")
            from spaces3 in Spaces
            from reminderTime in Parse.Decimal
            from spaces4 in Spaces
            from sound_keyword in Parse.String("sound")
            from spaces5 in Spaces
            from soundName in MaybeQuotedString
            select new Tuple<string, string, string>(activityName, reminderTime, soundName);

        static readonly Parser<ConfigOp> AlertAllStatement =
            AlertAll.Select<Tuple<string, string, string>, ConfigOp>((Tuple<string, string, string> t) => ((TimelineConfig config) =>
            {
                var alertSound = config.AlertSoundAssets.Get(t.Item3);
                config.AlertAlls.Add(new AlertAll { ActivityName = t.Item1, ReminderTime = Double.Parse(t.Item2), AlertSound = alertSound });
            }));

        static readonly Parser<string> HideAll =
            from hideall in Parse.String("hideall")
            from spaces in Spaces
            from activityName in MaybeQuotedString
            select activityName;

        static readonly Parser<ConfigOp> HideAllStatement =
            HideAll.Select<string, ConfigOp>((string targetActivityName) => ((TimelineConfig config) =>
            {
                config.HideAlls.Add(targetActivityName);
            }));

        static readonly Parser<ConfigOp> TimelineStatement =
            AlertSoundAliasStatement.Or(AlertAllStatement).Or(HideAllStatement).Or(TimelineActivityStatement);

        static readonly Parser<int> LineBreak = Parse.Or(Parse.Char('\n'), Parse.Char('\r')).AtLeastOnce().Return(TRASH);
        static readonly Parser<int> StatementSeparator =
            from beforeSpaces in Parse.Optional(Spaces)
            from comment in Parse.Optional(Comment)
            from lb in LineBreak
            from afterSpaces in Parse.Optional(Spaces)
            select TRASH;

        static readonly Parser<IEnumerable<ConfigOp>> TimelineStatements =
            from startSpaces in StatementSeparator.Many()
            from stmts in (
                from stmtop in TimelineStatement
                from sep in StatementSeparator.AtLeastOnce()
                select stmtop
                ).Many()
            select stmts;

        public static readonly Parser<TimelineConfig> TimelineConfig = TimelineStatements.Select(stmts => {
           TimelineConfig config = new TimelineConfig();
           foreach (ConfigOp op in stmts)
               op(config);
           return config;
        }).End();
    }

    public class TimelineLoader
    {
        static public Timeline LoadFromFile(string path)
        {
            string text = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
            return LoadFromText(System.IO.Path.GetFileName(path), text);
        }

        static public Timeline LoadFromText(string name, string text)
        {
            TimelineConfig config = TimelineConfigParser.TimelineConfig.Parse(text);
            foreach (AlertAll alertAll in config.AlertAlls)
            {
                foreach (TimelineActivity matchingActivity in config.Items.FindAll(activity => activity.Name == alertAll.ActivityName))
                {
                    var alert = new ActivityAlert { Activity = matchingActivity, ReminderTimeOffset = alertAll.ReminderTime, Sound = alertAll.AlertSound };
                    config.Alerts.Add(alert);
                }
            }
            foreach (string activityName in config.HideAlls)
            {
                foreach (TimelineActivity matchingActivity in config.Items.FindAll(activity => activity.Name == activityName))
                {
                    matchingActivity.Hidden = true;
                }
            }
            return new Timeline(name, config.Items, config.Anchors, config.Alerts, config.AlertSoundAssets);
        }
    }
}
