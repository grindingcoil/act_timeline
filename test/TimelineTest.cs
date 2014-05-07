using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACTTimeline;

namespace test
{
    [TestClass]
    public class TimelineTest
    {
        [TestMethod]
        public void ShouldAllowEmptyTimeline()
        {
            Timeline t = TimelineLoader.LoadFromText("test", "");
            Assert.AreEqual(0, t.Items.Count());
            Assert.IsFalse(t.VisibleItemsAt(0, 10).Any());
        }

        [TestMethod]
        public void AnchorWindowShouldBeRespected()
        {
            Timeline t = TimelineLoader.LoadFromText("test", "100 テスト sync /a/ window 20,30\n");
            TimelineAnchor a = t.Anchors.First();

            Assert.AreSame(a, t.FindAnchorMatchingLogline(100, "a"), "With in window");
            Assert.AreSame(a, t.FindAnchorMatchingLogline(81, "a"), "With in before window");
            Assert.AreSame(a, t.FindAnchorMatchingLogline(129, "a"), "With in after window");

            Assert.IsNull(t.FindAnchorMatchingLogline(79, "a"), "Out of before window");
            Assert.IsNull(t.FindAnchorMatchingLogline(131, "a"), "Out of after window");
        }

        [TestMethod]
        public void ActivitiesWithSameTimeToStartShouldBeAccepted()
        {
            string txt = "";
            for (int i = 0; i < 10; ++i)
            {
                txt += String.Format("1 {0}\n", i);
            }
            Timeline t = TimelineLoader.LoadFromText("test", txt);
        }

        [TestMethod]
        public void VisibleItemsAtShouldReturnUnfinishedActivities()
        {
            string txt = "";
            for (int i = 0; i < 10; ++i)
            {
                txt += String.Format("{0} {0}\n", i);
            }
            Timeline t = TimelineLoader.LoadFromText("test", txt);

            {
                var visibleItems = t.VisibleItemsAt(5.1, 10).ToList();
                Assert.AreEqual(4, visibleItems.Count);
            }
            {
                var visibleItems = t.VisibleItemsAt(5, 10).ToList();
                Assert.AreEqual(4, visibleItems.Count);
            }
            {
                var visibleItems = t.VisibleItemsAt(4.9, 10).ToList();
                Assert.AreEqual(5, visibleItems.Count);
            }
        }

        [TestMethod]
        public void VisibleItemsAtShouldReturnActivitiesWithSameStartTime()
        {
            string txt = "";
            for (int i = 0; i < 10; ++i)
            {
                txt += String.Format("{0} {0}a\n", i);
                txt += String.Format("{0} {0}b\n", i);
                txt += String.Format("{0} {0}c\n", i);
            }
            Timeline t = TimelineLoader.LoadFromText("test", txt);

            {
                var visibleItemsStr = t.VisibleItemsAt(5.1, 10).Select(a => a.Name).Aggregate((a, n) => a + " " + n);
                Assert.AreEqual("6a 6b 6c 7a 7b 7c 8a 8b 8c 9b", visibleItemsStr);
            }
            {
                var visibleItemsStr = t.VisibleItemsAt(5, 10).Select(a => a.Name).Aggregate((a, n) => a + " " + n);
                Assert.AreEqual("6a 6b 6c 7a 7b 7c 8a 8b 8c 9b", visibleItemsStr);
            }
        }

        [TestMethod]
        public void AlertLookupShouldReturnValidResults()
        {
            TimelineActivity activity = new TimelineActivity { TimeFromStart = 10 };
            List<TimelineActivity> activities = new List<TimelineActivity>() { activity };
            List<ActivityAlert> alerts = new List<ActivityAlert>();
            for (int i = 0; i < 10; ++i)
                alerts.Add(new ActivityAlert { ReminderTimeOffset = (double)i, Activity = activity });

            Timeline t = new Timeline("foobar", activities, new List<TimelineAnchor>(), alerts, new AlertSoundAssets());

            // t.alertsTimeFromStart == 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            Assert.AreEqual(4, t.FindFirstAlertIndexAfterStartTime(4.9));
            Assert.AreEqual(5, t.FindFirstAlertIndexAfterStartTime(5));
            Assert.AreEqual(5, t.FindFirstAlertIndexAfterStartTime(5.1));

            {
                var pending = t.PendingAlertsAt(0);
                CollectionAssert.AreEqual(
                    new List<double> { },
                    pending.Select(a => a.TimeFromStart).ToList());
            }
            {
                var pending = t.PendingAlertsAt(1.1);
                CollectionAssert.AreEqual(
                    new List<double> { 1.0 },
                    pending.Select(a => a.TimeFromStart).ToList());
            }
            {
                var pending = t.PendingAlertsAt(2.1);
                CollectionAssert.AreEqual(
                    new List<double> { 1.0, 2.0 },
                    pending.Select(a => a.TimeFromStart).ToList());
            }
            {
                var pending = t.PendingAlertsAt(3.1);
                CollectionAssert.AreEqual(
                    new List<double> { 1.0, 2.0, 3.0 },
                    pending.Select(a => a.TimeFromStart).ToList());
            }
            {
                var pending = t.PendingAlertsAt(5.1);
                CollectionAssert.AreEqual(
                    new List<double> { 3.0, 4.0, 5.0 },
                    pending.Select(a => a.TimeFromStart).ToList());
            }
        }
    }
}
