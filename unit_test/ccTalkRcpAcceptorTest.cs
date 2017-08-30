using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class ccTalkRcpAcceptorTest
    {
        Byte expected_db = 1;
        Byte test_channel = 15;
        string test_coin = "AE025A-0";
        string test_file = "C:\\Users\\kri.NRI\\Documents\\rcpdist\\allFiles\\rcp\\Eagle\\DE0\\Bin\\Db-001\\AE\\AE025A-0.bin";
        string test_file_bad = "C:\\Users\\kri.NRI\\Documents\\rcpdist\\allFiles\\rcp\\Eagle\\DE0\\Bin\\Db-002\\AE\\AE025A-0.bin";

        [TestMethod]
        public void ccTalkRcpAcceptor_get_information()
        {
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_eagle = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            Byte test = rcp_eagle.database_version;
            Assert.IsTrue(rcp_eagle.is_rcp);
            Assert.IsTrue(rcp_eagle.database_version == expected_db);     
            bus.close();
        }
       
        [TestMethod]
        public void ccTalkRcpAcceptor_get_extended_info()
        { //We test as well if the extended messaes are sent correct
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_eagle = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            Assert.AreEqual(rcp_eagle.request_extended_id(8),"........");            
            bus.close();
        }


        [TestMethod]
        public void ccTalkRcpAcceptor_add_coin()
        { //We test as well if the extended messaes are sent correct
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_eagle = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            //Make sure we have any 
            //Be sure the coin we expect is in!
            Assert.AreNotEqual(rcp_eagle.request_extended_id(10), test_coin);
            rcp_eagle.upload_file(test_file, test_channel);
            Assert.AreEqual(rcp_eagle.request_extended_id(test_channel), test_coin);
            //Make a bad upload            
            ccTalkNet.rcp_error error = rcp_eagle.upload_file(test_file_bad, test_channel);
            Assert.AreEqual(error.error_code,236);
            bus.close();
        }


        [TestMethod]
        public void ccTalkRcpAcceptor_remove_coin()
        { //We test as well if the extended messaes are sent correct
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_eagle = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            //Be sure the coin we expect is in!
            //Assert.AreEqual(rcp_eagle.request_extended_id(10), "EU001A-0");
            rcp_eagle.remove_coin_signature(test_channel);
            Assert.AreEqual(rcp_eagle.request_extended_id(test_channel), "........");
            bus.close();
        }



    }
}
