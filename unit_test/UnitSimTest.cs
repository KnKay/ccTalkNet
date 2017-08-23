using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class UnitSimTest
    {
        [TestMethod]
        public void Simulators_unit_LogicReturns()
        {
            unit_sim bus_sim = new unit_sim();
            bool result = bus_sim.ack_ccTalk_Bytes(new Byte[2]);
            Assert.IsTrue(result);
            bus_sim.should_fail = true;
            result = bus_sim.ack_ccTalk_Bytes(new Byte[2]);
            Assert.IsFalse(result);
            bus_sim.should_fail = false;
            result = bus_sim.ack_ccTalk_Message(new ccTalkNet.ccTalk_Message());
            Assert.IsTrue(result);
            bus_sim.should_fail = true;
            result = bus_sim.ack_ccTalk_Message(new ccTalkNet.ccTalk_Message());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Simulators_unit_Useable()
        {
            unit_sim bus_sim = new unit_sim();
            ccTalkNet.ccTalk_acceptor acceptor = new ccTalkNet.ccTalk_acceptor(bus_sim, 2);
            Assert.IsTrue(acceptor.is_available());
        }
    }
}
