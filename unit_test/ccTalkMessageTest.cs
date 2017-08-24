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
        public void ccTalkMessageTest_simple_checksum()
        {
            Byte[] data_bytes = new Byte[5] {0x01,0x00,0x02,0x00,0x00};
            Byte result = ccTalkNet.ccTalk_Message.simple_checksum(data_bytes);            
            Assert.IsTrue(result.Equals(253), "Creation of checksum Failed");
            data_bytes[4] = result;
            Assert.IsTrue(ccTalkNet.ccTalk_Message.validate_simple_checksum(data_bytes), "Validation failed");
        }

        [TestMethod]
        public void ccTalkMessageTest_constructor_test_no_payload()
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
        public void ccTalkMessageTest_constructor_test_payload()
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
        public void ccTalkMessageTest_implode()
        {
            Byte[] data_bytes = new Byte[5] { 0x01, 0x00, 0x02, 0x00,253 };
            ccTalkNet.ccTalk_Message message = new ccTalkNet.ccTalk_Message(data_bytes);
            Byte[] imploded = message.implode();
            CollectionAssert.AreEqual(data_bytes, imploded);
        }

        [TestMethod]
        public void ccTalkMessageTest_get_pl_from_bytes()
        {
            //dest, pl-size, src, header, databyte0-n, checksum
            Byte[] data_bytes = new Byte[9] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
            Byte[] payload = ccTalkNet.ccTalk_Message.get_bytes_payload(data_bytes);
            Assert.AreEqual(payload.Length, data_bytes[1], "Payload Sizesize not correct");
            Assert.AreEqual(payload[0], data_bytes[4], "Payload first byte not correct");
            Assert.AreEqual(payload[1], data_bytes[5], "Payload  last not correct");
            data_bytes = new Byte[9] { 0x01, 0x04, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
         //   Assert.AreEqual(payload.Length, data_bytes[1], "Payload Sizesize not correct");
        }
    }
}
