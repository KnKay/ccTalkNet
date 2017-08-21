using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace unit_test
{
    [TestClass]
    public class ccTalkMessageTest
    {
        [TestMethod]
        /* Testing ccTalk simple checksum according to manual!
         * 
         * For example, the message [ 1 ] [ 0 ] [ 2 ] [ 0 ] would be followed by the
         * checksum [ 253 ] because 1 + 0 + 2 + 0 + 253 = 256 = 0.
         * 
         * Part 1, Page 28
         */
        public void simple_checksum()
        {
            Byte[] data_bytes = new Byte[4] {0x01,0x00,0x02,0x00};
            Byte result = ccTalkNet.ccTalk_Message.simple_checksum(data_bytes);
            Assert.IsTrue(result.Equals(253));
        }

        [TestMethod]
        public void constructor_test_no_payload()
        {
            //Test with simple ack!
            Byte[] data_bytes = new Byte[5] { 0x01, 0x00, 0x02, 0x00, 0xff};
            ccTalkNet.ccTalk_Message message = new ccTalkNet.ccTalk_Message(data_bytes);
            Assert.IsTrue(message.dest.Equals(0x01));
            Assert.IsTrue(message.src.Equals(0x02));
            Assert.IsTrue(message.header.Equals(0x00));
            Assert.IsTrue(message.checksum.Equals(253));
            Assert.IsFalse(message.had_valid_checksum);


        }

        [TestMethod]
        public void constructor_test_payload()
        {
            //Test with payload
            Byte[] data_bytes = new Byte[7] {   0x01, //dest
                                         0x02, //payload_size
                                         0x02, //src
                                         0x00, //header
                                         0xff, //data 1
                                         0xf0, //data 1
                                         0x12 //chechsum
                                      };
            ccTalkNet.ccTalk_Message message = new ccTalkNet.ccTalk_Message(data_bytes);
            Assert.IsTrue(message.payload[0].Equals(0xff));
            Assert.IsTrue(message.payload[1].Equals(0xf0));
        }


        [TestMethod]
        public void Test_implode()
        {
            Assert.IsTrue(false);
        }
    }
}
