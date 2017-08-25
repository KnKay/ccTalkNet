using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class ccTalkAcceptorTest
    {
        [TestMethod]
        public void ccTalkAcceptor_InitPhase_with_bus()
        {
            /* The simulator will return a string corresponding 
             * the header during init
             * 
             * Due to this the returned string's are the number of the info
             */
            ccTalkNet.ccTalk_Bus bus = new unit_sim();
            ccTalkNet.ccTalk_acceptor device = new ccTalkNet.ccTalk_acceptor(bus, 2);            
            Assert.AreEqual(device.manu_id, "246");
            Assert.AreEqual(device.equip_cat_id, "245");
            Assert.AreEqual(device.prod_code, "244");
            Assert.AreEqual(device.build_code, "192");
            Assert.IsTrue(device.is_available());
        }

        [TestMethod]
        public void ccTalkDevice_CoinList()
        {
            ccTalkNet.ccTalk_Bus bus = new unit_sim();
            ccTalkNet.ccTalk_acceptor device = new ccTalkNet.ccTalk_acceptor(bus, 2);
            /* We assume the coins have been read on the init. 
             * We now check if the list is valid!
             */
            Assert.AreEqual(device.get_coin(1), "CI1");
            Assert.AreEqual(device.get_coin(8), "CI8");
            Assert.AreEqual(device.get_sorter_path(2), 3);
            Assert.AreEqual(device.get_sorter_path("CI1"), 2);

        }

        [TestMethod]
        public void ccTalkDevice_events()
        {

        }
    }
}
