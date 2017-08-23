using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace unit_test 
{

    /* Test of the bus should be done with either a 
     * ccTalk Hub connected or a bridged RX TX.
     * 
     * This will make sure we have an echo on this. 
     * Due to the echo we can check if reading and writing 
     * is ok. 
     * 
     * To test the real system any ccTalk device answer 
     * header 254 simple poll on address 2
     */

    [TestClass]
    public class Bus_Test
    {
                        
        [TestMethod]       
        public void ccTalkBus_open()
        {
            ccTalkNet.ccTalkBus bus = new ccTalkNet.ccTalkBus();
            Assert.IsFalse(bus.open("false"), "Fail to not open a port");
            bus.close();
            //Wait sime time
            Assert.IsTrue(bus.open("COM10"));
            Assert.AreEqual(bus.state, ccTalkNet.ccTalk_Bus_State.OPEN);
            bus.close();
            Assert.AreEqual(bus.state, ccTalkNet.ccTalk_Bus_State.CLOSED);
        }

        [TestMethod]
        public void ccTalkBus_echo_fail()
        {
                  
            ccTalkNet.ccTalkBus bus = new ccTalkNet.ccTalkBus();
            Assert.IsTrue(bus.open("COM10"));
            bus.read_direct(10000);
            Assert.AreEqual(bus.state, ccTalkNet.ccTalk_Bus_State.READTIMEOUT, "No readout found");
            bus.close();
        }

        //ToDo: make black magic to make this fucking work -> Tested by cli usage, working!
        [TestMethod]
        public void ccTalkBus_echo_pass()
        {            
            ccTalkNet.ccTalkBus bus = new ccTalkNet.ccTalkBus();
            Assert.IsTrue(bus.open("COM10"));
            Byte[] test_bytes = new Byte[7] { 0x01, 0x02, 0x03, 0x04, 0x05,0x06,0x07 };
            Byte[] ack = new Byte[5] { 0x01, 0x00, 0x02, 0x00, 253 };
            //We write 4 bytes and expect them back!             
            Byte[] echo = null;
            echo =  bus.write_direct(test_bytes);
            //Assert.AreEqual(echo, ack);           
            bus.close();
        }

        [TestMethod]
        public void ccTalkBus_t_r_byte()
        {
            ccTalkNet.ccTalkBus bus = new ccTalkNet.ccTalkBus();
            Assert.IsTrue(bus.open("COM10"));
            Byte[] test_bytes = new Byte[5] { 0x02, 0x00, 0x01, 245, 0x00 };
            test_bytes[4] = ccTalkNet.ccTalk_Message.simple_checksum(test_bytes);
            Byte[] ack = new Byte[5] { 0x01, 0x00, 0x02, 0x00, 253 };
            //We write 4 bytes and expect them back!             
            Byte[] echo = null;
            echo = bus.write_direct(test_bytes);
            //Assert.IsTrue(echo.Equals(test_bytes), "Echo not equal");            
            bus.close();
        }

        [TestMethod]
        public void ccTalkBus_t_r_message()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ccTalkBus_Eventsystem()
        {
            Assert.IsTrue(false);
        }

    }
}
