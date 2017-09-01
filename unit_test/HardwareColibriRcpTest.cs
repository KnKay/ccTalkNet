using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class HardwareColibriRcpTest
    {

        Byte expected_db = 3; //What ever?
        Byte test_channel = 1;
        string test_coin = "CA001A-0";
        string test_file = "C:\\Users\\kri.NRI\\Documents\\rcpdist\\allFiles\\rcp\\Colibri\\DE0\\Bin\\Db-002\\CA\\CA001A-0.bin";
        string test_file_bad = "C:\\Users\\kri.NRI\\Documents\\rcpdist\\allFiles\\rcp\\Eagle\\DE0\\Bin\\Db-002\\AE\\AE025A-0.bin";

        [TestMethod]
        public void ccTalkRcpColibri_get_information()
        {
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_colibri = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            Byte test = rcp_colibri.database_version;
            string other = rcp_colibri.build_code;
            Byte db = rcp_colibri.database_version;
            Assert.IsTrue(rcp_colibri.is_rcp);
            bus.close();
        }

        [TestMethod]
        public void ccTalkRcpColibri_get_extended_info()
        { //We test as well if the extended messaes are sent correct
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_colibri = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            string test = rcp_colibri.request_extended_id(1);
            Assert.AreEqual(test, "EU001A-0");
            bus.close();
        }

        [TestMethod]
        public void ccTalkRcpColibri_add_coin()
        { //We test as well if the extended messaes are sent correct
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_colibri = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            //Make sure we have any 
            //Be sure the coin we expect is in!
            Assert.AreNotEqual(rcp_colibri.request_extended_id(10), test_coin);

            ccTalkNet.rcp_error error = rcp_colibri.upload_file(test_file, test_channel);
            Assert.AreEqual(rcp_colibri.request_extended_id(test_channel), test_coin);           
            error = rcp_colibri.upload_file(test_file_bad, test_channel);//Upload a wrong file
            Assert.AreEqual(error.error_code, 236);

            bus.close();
        }

        [TestMethod]
        public void ccTalkRcpEagler_remove_coin()
        { //We test as well if the extended messaes are sent correct
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            bus.open("COM4");
            ccTalkNet.ccTalk_RcpAcceptor rcp_colibri = new ccTalkNet.ccTalk_RcpAcceptor(bus, 2);
            //Be sure the coin we expect is in!
            rcp_colibri.remove_coin_signature(test_channel);
            Assert.AreEqual(rcp_colibri.request_extended_id(test_channel), "........");
            bus.close();
        }


    }
}
