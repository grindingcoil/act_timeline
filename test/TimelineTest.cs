using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACTTimeline;

namespace test
{
    [TestClass]
    public class TimelineTest
    {
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
                Assert.AreEqual(5, visibleItems.Count);
            }
        }
    }
}
