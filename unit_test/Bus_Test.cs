using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class Bus_Test
    {
        [TestMethod]
        public void ccTalk_bus_Init()
        {
            //Assert.IsTrue(false);
        }

        [TestMethod]
        public void ccTalk_bus_open()
        {
            ccTalkNet.ccTalkBus bus = new ccTalkNet.ccTalkBus();
            Assert.IsTrue(bus.open("COM1"));
            bus.close();
            Assert.IsFalse(bus.open("COM7"));
            bus.close();
        }

    }
}
