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

        [TestMethod]
        public void Hardware_Eagle_Inhibts()
        {
            
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_acceptor eagle = new ccTalkNet.ccTalk_acceptor(bus, 2);            
            eagle.master_inhibit = true;
            Assert.IsTrue(eagle.master_inhibit);
            eagle.master_inhibit = false;
            Assert.IsFalse(eagle.master_inhibit);            
            eagle.coin_inhibits = new Byte[]{0x00,0x00 };
            Byte[] inh = eagle.coin_inhibits;
            Assert.IsTrue(inh[0] == 0);
            Assert.IsTrue(inh[1] == 0);
            eagle.coin_inhibits = new Byte[] { 0xff, 0xff };
            inh = eagle.coin_inhibits;
            Assert.IsTrue(inh[0] == 0xff);
            Assert.IsTrue(inh[1] == 0xff);
            bus.close();
        }
    }
}
