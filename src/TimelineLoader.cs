using System.Collections.Generic;
using System.Globalization;
using Sprache;

namespace ACTTimeline
{
    public class TimelineConfig
    {
        public List<TimelineActivity> Items;
        public AlertSoundAssets AlertSoundAssets;

        public TimelineConfig()
        {
            Items = new List<TimelineActivity>();
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
        static readonly Parser<double> Duration = 
            from spaces in Spaces
            from durationPrefix in Parse.String("duration").Or(Parse.String("効果時間"))
            from spaces2 in Parse.Optional(Spaces)
            from value in Parse.Decimal
            select double.Parse(value, CultureInfo.InvariantCulture);
        static readonly Parser<TimelineActivity> TimelineActivity =
            from timeFromStart in Parse.Decimal
            from optionalSuffix in Parse.Regex(@"秒?後?")
            from spaces in Spaces
            from name in MaybeQuotedString
            from duration in Parse.Optional(Duration)
            select new TimelineActivity {
                TimeFromStart = double.Parse(timeFromStart, CultureInfo.InvariantCulture),
                Name = name,
                Duration = duration.GetOrElse(0)
            };

        static readonly Parser<ConfigOp> TimelineActivityStatement =
            TimelineActivity.Select<TimelineActivity, ConfigOp>(activity => ((TimelineConfig config) => {
                config.Items.Add(activity);
            })).Named("TimelineActivityStatement");

        static readonly Parser<ConfigOp> TimelineStatement =
            TimelineActivityStatement;

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
            return LoadFromText(text);
        }

        static public Timeline LoadFromText(string text)
        {
            TimelineConfig config = TimelineConfigParser.TimelineConfig.Parse(text);
            return new Timeline(config.Items, config.AlertSoundAssets);
        }
    }
}
