using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class ccTalk_DeviceTest
    {
        
        [TestMethod]
        public void ccTalkDevice_InitPhase_with_bus()
        {
            /* The simulator will return a string corresponding 
             * the header during init
             * 
             * Due to this the returned string's are the number of the info
             */
            ccTalkNet.ccTalk_Bus bus = new unit_sim();
            ccTalkNet.ccTalk_device device = new ccTalkNet.ccTalk_acceptor(bus, 2);                        
            Assert.AreEqual(device.manu_id, "246");
            Assert.AreEqual(device.equip_cat_id, "245");
            Assert.AreEqual(device.prod_code, "244");
            Assert.AreEqual(device.build_code, "192");
            Assert.IsTrue(device.is_available());
        }        
    }
}
