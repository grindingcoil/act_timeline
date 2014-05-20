using System;
using System.Linq;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACTTimeline;
using System.Text.RegularExpressions;

namespace test
{
    [TestClass]
    public class TimelineLoaderTest
    {
        [TestMethod]
        public void TestTimelineStatementBasic()
        {
            Timeline timeline = TimelineLoader.LoadFromText("test", "1 テスト\n");
            var items = timeline.Items.ToList();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1.0, items.First().TimeFromStart);
            Assert.AreEqual("テスト", items.First().Name);
        }

        [TestMethod]
        public void EOLAtEOFShouldBeOptional()
        {
            Timeline timeline = TimelineLoader.LoadFromText("test", "1 テスト");
            var items = timeline.Items.ToList();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1.0, items.First().TimeFromStart);
            Assert.AreEqual("テスト", items.First().Name);
        }

        [TestMethod]
        public void TestTimelineStatementWithDuration()
        {
            Timeline timeline = TimelineLoader.LoadFromText("test", "1 テスト duration 5\n");
            var items = timeline.Items.ToList();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1.0, items.First().TimeFromStart);
            Assert.AreEqual("テスト", items.First().Name);
            Assert.AreEqual(5.0, items.First().Duration);
        }

        [TestMethod]
        public void TestTimelineStatementWithSync()
        {
            Timeline timeline = TimelineLoader.LoadFromText("test", "1 テスト sync /「テスト」の構え/\n");
            var items = timeline.Items.ToList();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1.0, items.First().TimeFromStart);
            Assert.AreEqual("テスト", items.First().Name);

            var anchors = timeline.Anchors.ToList();
            Assert.AreEqual(1, anchors.Count);
            Assert.AreEqual("「テスト」の構え", anchors.First().Regex.ToString());
            Assert.AreEqual(TimelineAnchor.DefaultWindow / 2, anchors.First().WindowBefore);
            Assert.AreEqual(TimelineAnchor.DefaultWindow / 2, anchors.First().WindowAfter);
        }

        [TestMethod]
        public void TestTimelineStatementWithSingleWindow()
        {
            Timeline timeline = TimelineLoader.LoadFromText("test", "1 テスト sync /「テスト」の構え/ window 30\n");
            var items = timeline.Items.ToList();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1.0, items.First().TimeFromStart);
            Assert.AreEqual("テスト", items.First().Name);

            var anchors = timeline.Anchors.ToList();
            Assert.AreEqual(1, anchors.Count);
            Assert.AreEqual("「テスト」の構え", anchors.First().Regex.ToString());
            Assert.AreEqual(30 / 2, anchors.First().WindowBefore, "WindowBefore");
            Assert.AreEqual(30 / 2, anchors.First().WindowAfter, "WindowAfter");
        }

        [TestMethod]
        public void TestTimelineStatementWithBeforeAndAfterWindow()
        {
            Timeline timeline = TimelineLoader.LoadFromText("test", "1 テスト sync /「テスト」の構え/ window 10, 20\n");
            var items = timeline.Items.ToList();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1.0, items.First().TimeFromStart);
            Assert.AreEqual("テスト", items.First().Name);

            var anchors = timeline.Anchors.ToList();
            Assert.AreEqual(1, anchors.Count);
            Assert.AreEqual("「テスト」の構え", anchors.First().Regex.ToString());
            Assert.AreEqual(10, anchors.First().WindowBefore);
            Assert.AreEqual(20, anchors.First().WindowAfter);
        }

        [TestMethod]
        public void TestTimelineStatementRegression()
        {
            Timeline timeline = TimelineLoader.LoadFromText("test", "0 \"asdf\" sync /aaa/ window 999\n1 bbb\n");
            var items = timeline.Items.ToList();
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(0.0, items.First().TimeFromStart);
            Assert.AreEqual("asdf", items.First().Name);
        }

        [TestMethod]
        [DeploymentItem(@"..\..\..\..\resources\")]
        public void TestIncludedTimelineFiles()
        {
            Globals.ResourceRoot = ".";
            foreach (var filepath in Globals.TimelineTxtsInResourcesDir)
                TimelineLoader.LoadFromFile(filepath);
        }
    }
}
