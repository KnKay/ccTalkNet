using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class HardwareEagleTest
    {
        [TestMethod]
        public void Hardware_Eagle_Init_Unit()
        {
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_acceptor eagle = new ccTalkNet.ccTalk_acceptor(bus, 2);
            Assert.AreEqual(eagle.equip_cat_id, "Coin Acceptor");
            bus.close();
        }
    }
}
