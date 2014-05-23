using System;
using ACTTimeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace test
{
    [TestClass]
    public class RemoteVersionInfoTest
    {
        public static readonly string TestVersionInfoXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<versionInfo>
  <version>0.1.1.0</version>
  <changeSummary>
    <jp>ほげふが改善</jp>
  </changeSummary>
  <downloadUrl>https://github.com/grindingcoil/act_timeline</downloadUrl>
</versionInfo>
";
        public static readonly string TestUrl = "https://mock.server/version_info.xml";

        public void AreEqualToTestInfo(RemoteVersionInfo info)
        {
            Assert.AreEqual("0.1.1.0", info.Version);
            Assert.AreEqual("ほげふが改善", info.ChangeSummaryJp);
            Assert.AreEqual("https://github.com/grindingcoil/act_timeline", info.DownloadUrl);
        }

        [TestMethod]
        public void TestFromXml()
        {
            var info = RemoteVersionInfo.FromXml(TestVersionInfoXml);
            AreEqualToTestInfo(info);
        }

        public static string MockFetchUrl(string url)
        {
            if (url == TestUrl)
                return TestVersionInfoXml;
            else
                throw new System.Net.WebException("404!");
        }

        [TestMethod]
        public void TestFetchUrl()
        {
            var origFetch = Globals.FetchUrlImpl;
            Globals.FetchUrlImpl = MockFetchUrl;

            AreEqualToTestInfo(RemoteVersionInfo.FetchUrl(TestUrl));

            Globals.FetchUrlImpl = origFetch;
        }
    }

    [TestClass]
    public class UpdateCheckerTest
    {
        [TestMethod]
        public void TestCompareVersionString()
        {
            Assert.IsTrue(UpdateChecker.CompareVersionString("1.2.3.4", "1.2.3.03") > 0);
            Assert.IsTrue(UpdateChecker.CompareVersionString("1.2.3.10", "1.2.3.3") > 0);

            Assert.IsTrue(UpdateChecker.CompareVersionString("0.1.1.0", "0.1.0.4") > 0);
            Assert.IsTrue(UpdateChecker.CompareVersionString("0.1.1.0", "0.1.1.0") == 0);
            Assert.IsTrue(UpdateChecker.CompareVersionString("0.1.1.0", "0.1.1.1") < 0);

            Assert.IsTrue(UpdateChecker.CompareVersionString("1.0.0.0", "0.1.0.4") > 0);
        }

        public void TestPerformCheck(UpdateChecker checker)
        {
            var origFetch = Globals.FetchUrlImpl;
            Globals.FetchUrlImpl = RemoteVersionInfoTest.MockFetchUrl;

            checker.PerformCheck();

            Globals.FetchUrlImpl = origFetch;
        }

        List<string> logLines;

        [TestInitialize]
        public void Initialize()
        {
            logLines = new List<string>();
            Globals.WriteLogImpl = (line) => { logLines.Add(line); };
        }

        [TestCleanup]
        public void Cleanup()
        {
            // No err log should have been written.
            Assert.AreEqual(0, logLines.Count);
        }

        [TestMethod]
        public void TestPerformCheckSameVersion()
        {
            TestPerformCheck(new UpdateChecker("0.1.1.0", RemoteVersionInfoTest.TestUrl));
        }

        [TestMethod]
        public void TestPerformCheckNewerThanPublishedVersion()
        {
            TestPerformCheck(new UpdateChecker("0.1.1.1", RemoteVersionInfoTest.TestUrl));
        }

        [Ignore] // Run manually
        [TestMethod]
        public void TestPerformCheckOldVersion()
        {
            TestPerformCheck(new UpdateChecker("0.1.0.9", RemoteVersionInfoTest.TestUrl));
        }

        [TestMethod]
        public void TestPerformCheckBadUrl()
        {
            try
            {
                TestPerformCheck(new UpdateChecker("0.1.1.0", "https://bad.url"));
            }
            catch(Exception)
            {
                Assert.Fail("TestPerformCheck should not throw exception even on bad url.");
            }

            // but with err log
            Assert.IsTrue(logLines.Count > 0);
            logLines.Clear();
        }
    }
}
