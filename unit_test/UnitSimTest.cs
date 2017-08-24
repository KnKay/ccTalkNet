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
            unit_sim.state = unit_state.ACKFAIL;
            result = bus_sim.ack_ccTalk_Bytes(new Byte[2]);
            Assert.IsFalse(result);
            unit_sim.state = unit_state.WORKING;
            result = bus_sim.ack_ccTalk_Message(new ccTalkNet.ccTalk_Message());
            Assert.IsTrue(result);
            unit_sim.state = unit_state.ACKFAIL;
            result = bus_sim.ack_ccTalk_Message(new ccTalkNet.ccTalk_Message());
            Assert.IsFalse(result);
            unit_sim.state = unit_state.WORKING;
        }

        [TestMethod]
        public void Simulators_unit_Useable()
        {
            unit_sim bus_sim = new unit_sim();
            ccTalkNet.ccTalk_acceptor acceptor = new ccTalkNet.ccTalk_acceptor(bus_sim, 2);
            Assert.IsTrue(acceptor.is_available());
        }

        [TestMethod]
        public void Simulators_unit_Inits()
        {
            unit_sim bus_sim = new unit_sim();
            unit_sim.state = unit_state.INIT;
            bus_sim.answer_request = unit_sim.generate_answer;
            Byte[] kri_payload = new Byte[3] {(Byte)'K', (Byte)'R', (Byte) 'I' };
            
            ccTalkNet.ccTalk_Message request_message = new ccTalkNet.ccTalk_Message(new Byte[5] { 2, 0x00, 0x01, 246, 0x00 });
            //Take the payload of our answer and stringify it!
            Byte[]  answer = bus_sim.send_ccTalk_Message(request_message).payload;
            Assert.AreEqual(kri_payload[1], answer[1]);
            Assert.AreEqual(kri_payload[1], answer[1]);
            Assert.AreEqual(kri_payload[1], answer[1]);
            
            request_message.header = 245;
            answer = bus_sim.send_ccTalk_Message(request_message).payload;
            Assert.AreEqual(kri_payload[0], answer[0]);
            Assert.AreEqual(kri_payload[1], answer[1]);
            Assert.AreEqual(kri_payload[2], answer[2]);
          
            request_message.header = 244;
            answer = bus_sim.send_ccTalk_Message(request_message).payload;
            Assert.AreEqual(kri_payload[0], answer[0]);
            Assert.AreEqual(kri_payload[1], answer[1]);
            Assert.AreEqual(kri_payload[2], answer[2]);

            request_message.header = 192;
            answer = bus_sim.send_ccTalk_Message(request_message).payload;
            Assert.AreEqual(kri_payload[0], answer[0]);
            Assert.AreEqual(kri_payload[1], answer[1]);
            Assert.AreEqual(kri_payload[2], answer[2]);
            //Step back to normal operation
            unit_sim.state = unit_state.WORKING;
        }

    }
}
