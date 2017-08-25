using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class ccTalkEventsTest
    {
        [TestMethod]
        public void ccTalkErrorEvents()
        {
            ccTalkNet.Error_event error = ccTalkNet.ccTalk_Error.get_error(1);
            Assert.AreEqual(error.error, "Rejected coin");             
        }
    }
}
