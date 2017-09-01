using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class HardwareColibriTest
    {
        [TestMethod]
        public void ccTalk_Colibri_Init()
        {
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_acceptor colibri = new ccTalkNet.ccTalk_acceptor(bus, 2);
            Assert.AreEqual(colibri.equip_cat_id, "Coin Acceptor");
            Assert.AreEqual(colibri.prod_code, "Colibri");
            bus.close();
        }

        [TestMethod]
        public void ccTalk_Colibri_Inhibits()
        {
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_acceptor colibri = new ccTalkNet.ccTalk_acceptor(bus, 2);
            colibri.master_inhibit = true;
            Assert.IsTrue(colibri.master_inhibit);
            colibri.master_inhibit = false;
            Assert.IsFalse(colibri.master_inhibit);
            colibri.coin_inhibits = new Byte[] { 0x00, 0x00 };
            Byte[] inh = colibri.coin_inhibits;
            Assert.IsTrue(inh[0] == 0);
            Assert.IsTrue(inh[1] == 0);
            colibri.coin_inhibits = new Byte[] { 0xff, 0xff };
            inh = colibri.coin_inhibits;
            Assert.IsTrue(inh[0] == 0xff);
            Assert.IsTrue(inh[1] == 0xff);
            bus.close();
        }

    }
}
