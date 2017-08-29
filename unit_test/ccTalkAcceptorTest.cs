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

        public ccTalkNet.ccTalk_Coin event_coin = null;
        public ccTalkNet.Error_event event_error = null;

        void coin_checker (object sender, ccTalkNet.ccTalk_Coin e)
        {
            event_coin = e;
        }

        void error_checker (object sender, ccTalkNet.Error_event e)
        {
            event_error = e;
        }

        [TestMethod]
        public void ccTalkAcceptor_CoinList()
        {
            ccTalkNet.ccTalk_Bus bus = new unit_sim();
            ccTalkNet.ccTalk_acceptor device = new ccTalkNet.ccTalk_acceptor(bus, 2);
            unit_sim.state = unit_state.INIT;
            /* We assume the coins have been read on the init. 
             * We now check if the list is valid!
             */
            Assert.AreEqual(device.get_coin(1), "CI1");
            Assert.AreEqual(device.get_coin(8), "CI8");
            Assert.AreEqual(device.get_sorter_path(2), 3);
            Assert.AreEqual(device.get_sorter_path("CI1"), 2);

        }

        [TestMethod]
        public void ccTalkAcceptor_events()
        {
            ccTalkNet.ccTalk_Bus bus = new unit_sim();
            unit_sim.state = unit_state.INIT;
            ccTalkNet.ccTalk_acceptor acceptor = new ccTalkNet.ccTalk_acceptor(bus, 2);
            unit_sim.state = unit_state.WORKING;
            unit_sim.poll_reply[0] = 0;
            //Check to only get a true if we have new events to handle!
            Assert.IsFalse(acceptor.get_credit_error_codes());
            unit_sim.poll_reply[0] = 1;
            unit_sim.poll_reply[1] = 0;
            unit_sim.poll_reply[2] = 1;
            //We accepted a coin 
            Assert.IsTrue(acceptor.get_credit_error_codes());
            //Check general event handing
            unit_sim.poll_reply[0] = 2;
            unit_sim.poll_reply[1] = 1;
            unit_sim.poll_reply[2] = 1;
            Assert.IsTrue(acceptor.get_credit_error_codes());
            //Test if we detect lost events
            unit_sim.poll_reply[0] = 20;
            acceptor.get_credit_error_codes();
            Assert.IsTrue(acceptor.has_lost_events);
            //Test the event handler....
            //Error handler
            unit_sim.poll_reply[0] = 21;
            unit_sim.poll_reply[1] = 0;
            unit_sim.poll_reply[2] = 1;
            bool has_error = false;
            Assert.IsFalse(has_error);
            acceptor.error_handler += delegate
            {
                has_error = true;
            };
            acceptor.error_handler += error_checker;
            acceptor.get_credit_error_codes();
            Assert.IsTrue(has_error);
            Assert.IsTrue(event_error.error == "Rejected coin");
            //Coin Handler
            unit_sim.poll_reply[0] = 22;
            unit_sim.poll_reply[1] = 2;
            unit_sim.poll_reply[2] = 1;
            bool has_coin = false;
            Assert.IsFalse(has_coin);
            acceptor.coin_handler += coin_checker;
            acceptor.coin_handler += delegate
            {
                has_coin = true;
            };
            acceptor.get_credit_error_codes();
            Assert.IsTrue(event_coin.channel == 2);
            Assert.IsTrue(event_coin.sorter_path == 1);
            Assert.IsTrue(has_coin);            
        }        
    }
    
}
