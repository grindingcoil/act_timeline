using System.Collections.Generic;
using System.Linq;
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
        public TtsSpeaker TtsSpeaker;
        public string TtsSentence;
        public AlertType Type;

        public enum AlertType
        {
            Sound,
            Tts
        }
    }

    public class TimelineConfig
    {
        public List<TimelineActivity> Items;
        public List<TimelineAnchor> Anchors;
        public List<AlertAll> AlertAlls;
        public List<string> HideAlls;
        public List<ActivityAlert> Alerts;
        public AlertSoundAssets AlertSoundAssets;
        public List<TtsSpeaker> Speakers;

        public TimelineConfig()
        {
            Items = new List<TimelineActivity>();
            Anchors = new List<TimelineAnchor>();
            AlertAlls = new List<AlertAll>();
            HideAlls = new List<string>();
            Alerts = new List<ActivityAlert>();
            AlertSoundAssets = new AlertSoundAssets();
            Speakers = new List<TtsSpeaker>();
        }
    }

    public class TimelineConfigParser
    {
        const int TRASH = 0;

        public delegate void ConfigOp(TimelineConfig config);

        static readonly Parser<Double> DecimalDouble = Parse.Decimal.Select(str => Double.Parse(str, CultureInfo.InvariantCulture));

        static readonly Parser<int> Comment = Parse.Regex(@"^#.*").Return(TRASH);

        static readonly Parser<string> QuotedString =
            from quoteStart in Parse.Char('"')
            from quotedString in Parse.CharExcept('"').Many().Text()
            from quoteEnd in Parse.Char('"')
            select quotedString;
        static readonly Parser<string> Spaces = Parse.Regex(@"^[ 　\t]+");
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
            from value in DecimalDouble
            select value;

        public struct SyncWindowSettings
        {
            public double WindowBefore;
            public double WindowAfter;
        };
        static readonly SyncWindowSettings DefaultWindow = new SyncWindowSettings
        {
            WindowBefore = TimelineAnchor.DefaultWindow / 2,
            WindowAfter = TimelineAnchor.DefaultWindow / 2
        };
        static readonly Parser<SyncWindowSettings> SingleWindow =
            from value in DecimalDouble
            select new SyncWindowSettings { WindowBefore = value / 2, WindowAfter = value / 2};
        static readonly Parser<SyncWindowSettings> BeforeAfterWindow =
            from beforeWindow in DecimalDouble
            from sep in Parse.Regex(@"[, 　]+")
            from afterWindow in DecimalDouble
            select new SyncWindowSettings { WindowBefore = beforeWindow, WindowAfter = afterWindow };
        static readonly Parser<SyncWindowSettings> SyncWindow =
            from spaces in Spaces
            from window in Parse.String("window")
            from spaces2 in Spaces
            from value in BeforeAfterWindow.Or(SingleWindow)
            select value;

        static readonly Parser<Tuple<string, SyncWindowSettings>> Sync =
            from spaces in Spaces
            from sync in Parse.String("sync")
            from spaces2 in Parse.Optional(Spaces)
            from regex in Regex
            from window in Parse.Optional(SyncWindow)
            select new Tuple<string, SyncWindowSettings>(regex, window.GetOrElse(DefaultWindow));
        static readonly Parser<Tuple<TimelineActivity, Tuple<string, SyncWindowSettings>>> TimelineActivity =
            from timeFromStart in Parse.Decimal
            from spaces in Spaces
            from name in MaybeQuotedString
            from duration in Parse.Optional(Duration)
            from sync in Parse.Optional(Sync)
            select new Tuple<TimelineActivity, Tuple<string, SyncWindowSettings>>(new TimelineActivity {
                TimeFromStart = double.Parse(timeFromStart, CultureInfo.InvariantCulture),
                Name = name,
                Duration = duration.GetOrElse(0)
            }, sync.GetOrElse(null));

        static readonly Parser<ConfigOp> TimelineActivityStatement =
            TimelineActivity.Select<Tuple<TimelineActivity, Tuple<string, SyncWindowSettings>>, ConfigOp>(t => ((TimelineConfig config) =>
            {
                config.Items.Add(t.Item1);
                if (t.Item2 != null)
                {
                    var windowSettings = t.Item2.Item2;
                    config.Anchors.Add(new TimelineAnchor {
                        TimeFromStart = t.Item1.TimeFromStart,
                        Regex = new System.Text.RegularExpressions.Regex(t.Item2.Item1),
                        WindowBefore = windowSettings.WindowBefore,
                        WindowAfter = windowSettings.WindowAfter
                    });
                }
            })).Named("TimelineActivityStatement");

        static readonly Parser<Tuple<string, string>> AlertSoundAlias =
            from define_keyword in Parse.String("define")
            from spaces in Spaces
            from alertsound_keyword in Parse.String("alertsound")
            from spaces2 in Spaces
            from alias in MaybeQuotedString
            from spaces3 in Spaces
            from target in MaybeQuotedString
            select new Tuple<string, string>(alias, target);

        static readonly Parser<Tuple<string, string, string>> TtsAlias =
            from define_keyword in Parse.String("define")
            from spaces in Spaces
            from tts_keyword in Parse.String("speaker")
            from spaces2 in Spaces
            from name in MaybeQuotedString
            from spaces3 in Spaces
            from rate in Parse.Regex(@"^\-?[0-9]+")
            from spaces4 in Spaces
            from volume in Parse.Regex("^[0-9]+")
            select new Tuple<string, string, string>(name, rate, volume);

        static readonly Parser<Tuple<string, string, string, string>> NewTtsAlias =
            from define_keyword in Parse.String("define")
            from spaces in Spaces
            from tts_keyword in Parse.String("speaker")
            from spaces2 in Spaces
            from name in MaybeQuotedString
            from spaces3 in Spaces
            from voiceName in MaybeQuotedString
            from spaces4 in Spaces
            from rate in Parse.Regex(@"^\-?[0-9]+")
            from spaces5 in Spaces
            from volume in Parse.Regex("^[0-9]+")
            select new Tuple<string, string, string, string>(name, voiceName, rate, volume);
            

        static readonly Parser<ConfigOp> AlertSoundAliasStatement =
            AlertSoundAlias.Select<Tuple<string, string>, ConfigOp>((Tuple<string, string> t) => ((TimelineConfig config) => {
                AlertSound alertSound = config.AlertSoundAssets.Get(t.Item2);
                config.AlertSoundAssets.RegisterAlias(alertSound, t.Item1);
            }));

        static readonly Parser<ConfigOp> TtsAliasStatement =
            TtsAlias.Select<Tuple<string, string, string>, ConfigOp>((Tuple<string, string, string> t) => ((TimelineConfig config) =>
            {
                var speaker = new TtsSpeaker(t.Item1, null, int.Parse(t.Item2), int.Parse(t.Item3));
                config.Speakers.Add(speaker);
            }));

        static readonly Parser<ConfigOp> NewTtsAliasStatement =
            NewTtsAlias.Select<Tuple<string, string, string, string>, ConfigOp>((Tuple<string, string, string, string> t) => ((TimelineConfig config) =>
            {
                var speaker = new TtsSpeaker(t.Item1, t.Item2, int.Parse(t.Item3), int.Parse(t.Item4));
                config.Speakers.Add(speaker);
            }));

        static readonly Parser<Tuple<string, string>> Sound =
            from before_keyword in Parse.String("before")
            from spaces in Spaces
            from reminderTime in Parse.Decimal
            from spaces2 in Spaces
            from sound_keyword in Parse.String("sound")
            from spaces3 in Spaces
            from soundName in MaybeQuotedString
            select new Tuple<string, string>(reminderTime, soundName);

        static readonly Parser<Tuple<string, string, string>> Tts =
            from before_keyword in Parse.String("before")
            from spaces in Spaces
            from reminderTime in Parse.Decimal
            from spaces2 in Spaces
            from tts_keyword in Parse.String("speak")
            from spaces3 in Spaces
            from ttsName in MaybeQuotedString
            from spaces4 in Spaces
            from sentence in MaybeQuotedString
            select new Tuple<string, string, string>(reminderTime, ttsName, sentence);

        static readonly Parser<Tuple<string, string, string>> AlertAllSound =
            from alertall in Parse.String("alertall")
            from spaces in Spaces
            from activityName in MaybeQuotedString
            from spaces2 in Spaces
            from sound in Sound
            select new Tuple<string, string, string>(
                activityName,
                sound.Item1,
                sound.Item2);

        static readonly Parser<Tuple<string, string, string, string>> AlertAllTts =
            from alertall in Parse.String("alertall")
            from spaces in Spaces
            from activityName in MaybeQuotedString
            from spaces2 in Spaces
            from tts in Tts
            select new Tuple<string, string, string, string>(
                activityName,
                tts.Item1,
                tts.Item2,
                tts.Item3);

        static readonly Parser<ConfigOp> AlertAllSoundStatement =
            AlertAllSound.Select<Tuple<string, string, string>, ConfigOp>((Tuple<string, string, string> t) => ((TimelineConfig config) =>
            {
                var alertSound = config.AlertSoundAssets.Get(t.Item3);
                config.AlertAlls.Add(new AlertAll
                {
                    ActivityName = t.Item1,
                    ReminderTime = Double.Parse(t.Item2),
                    AlertSound = alertSound,
                    Type = AlertAll.AlertType.Sound
                });
            }));

        static readonly Parser<ConfigOp> AlertAllTtsStatement =
            AlertAllTts.Select<Tuple<string, string, string, string>, ConfigOp>((Tuple<string, string, string, string> t) => ((TimelineConfig config) =>
            {
                var speakers = config.Speakers.Where(x => x.Name == t.Item3);
                if (speakers.Any())
                {
                    config.AlertAlls.Add(new AlertAll
                    {
                        ActivityName = t.Item1,
                        ReminderTime = Double.Parse(t.Item2),
                        TtsSpeaker = speakers.First(),
                        TtsSentence = t.Item4,
                        Type = AlertAll.AlertType.Tts
                    });
                }
                else
                {
                    throw new ParseException(string.Format("The TTS speaker named '{0}' is not defined.", t.Item3));
                }
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

        static readonly Parser<ConfigOp> EmptyStatement = Parse.Return<ConfigOp>((TimelineConfig config) => { });

        static readonly Parser<ConfigOp> TimelineStatement =
            AlertSoundAliasStatement
                .Or(TtsAliasStatement)
                .Or(NewTtsAliasStatement)
                .Or(AlertAllSoundStatement)
                .Or(AlertAllTtsStatement)
                .Or(HideAllStatement)
                .Or(TimelineActivityStatement)
                .Or(EmptyStatement);

        static readonly Parser<int> LineBreak = Parse.Or(Parse.Char('\n'), Parse.Char('\r')).AtLeastOnce().Return(TRASH);
        static readonly Parser<int> StatementSeparator =
            from beforeSpaces in Parse.Optional(Spaces)
            from comment in Parse.Optional(Comment)
            from lb in LineBreak
            from afterSpaces in Parse.Optional(Spaces)
            select TRASH;

        static readonly Parser<IEnumerable<ConfigOp>> TimelineStatements =
            from stmts in TimelineStatement.DelimitedBy(StatementSeparator.Many())
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
                    var alert = new ActivityAlert
                    {
                        Activity = matchingActivity,
                        ReminderTimeOffset = alertAll.ReminderTime, 
                        Sound = alertAll.AlertSound,
                        TtsSpeaker = alertAll.TtsSpeaker,
                        TtsSentence = alertAll.TtsSentence
                    };
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
