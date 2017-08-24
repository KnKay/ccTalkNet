using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class ccTalk_acceptorTest
    {
        

        [TestMethod]
        public void ccTalkDevice_InitPhase()
        {
            ccTalkNet.ccTalk_Bus bus = new unit_sim();
            ccTalkNet.ccTalk_device device = new ccTalkNet.ccTalk_acceptor(bus, 2);
            string test = "test";
            Assert.AreEqual(test, test);
            Assert.IsTrue(device.is_available());
        }
        
    }
}
