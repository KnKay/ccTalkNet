using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace unit_test
{
    [TestClass]
    public class ccTalkHostTest
    {

        public ccTalkNet.ccTalk_Coin event_coin = null;
        public ccTalkNet.Error_event event_error = null;
        object device = null;
        bool coin_handler_called = false;
        bool error_handler_called = false;

        void coin_checker(object sender, ccTalkNet.ccTalk_Coin_speficic e)
        {
            event_coin = e.coin;
            device = e.device;
            coin_handler_called = true;
        }

        void error_checker(object sender, ccTalkNet.Error_event_specific e)
        {
            event_error = e.error;
            device = e.device;
            error_handler_called = true;
        }

        [TestMethod]
        public void ccTalkHost_constructor()
        {
            unit_sim bus = new unit_sim();
            ccTalkNet.ccTalk_Host host = new ccTalkNet.ccTalk_Host(bus);
            ccTalkNet.ccTalk_acceptor acceptor = new ccTalkNet.ccTalk_acceptor(host.bus, 2);
            host.add_validator(acceptor);
            Assert.IsTrue(true);
            host.close();
        }

        [TestMethod]
        public void ccTalkHost_poll()
        {
            unit_sim bus = new unit_sim();
            unit_sim.state = unit_state.INIT;
            unit_sim.poll_reply[0] = 0;
            HostSim host = new HostSim(bus);
            ccTalkNet.ccTalk_acceptor acceptor = new ccTalkNet.ccTalk_acceptor(host.bus, 2);
            host.add_validator(acceptor);
            //Test the event handler....
            //Error handler
            unit_sim.poll_reply[0] = 1;
            unit_sim.poll_reply[1] = 0;
            unit_sim.poll_reply[2] = 1;
            bool has_error = false;
            Assert.IsFalse(has_error);
            host.error_handler += delegate
            {
                has_error = true;
            };
            host.error_handler += error_checker;
            host.poll_acceptors();
            Assert.IsTrue(has_error);
            Assert.IsTrue(event_error.error == "Rejected coin");
            //Coin Handler
            unit_sim.poll_reply[0] = 22;
            unit_sim.poll_reply[1] = 2;
            unit_sim.poll_reply[2] = 1;
            bool has_coin = false;
            Assert.IsFalse(has_coin);
            host.coin_handler += coin_checker;
            host.coin_handler += delegate
            {
                has_coin = true;
            };
            host.poll_acceptors();
            Assert.IsTrue(event_coin.channel == 2);
            Assert.IsTrue(has_coin);
            host.close();
        }

        [TestMethod]
        public void ccTalkHost_poll_by_timer()
        {
            unit_sim bus = new unit_sim();
            unit_sim.state = unit_state.INIT;
            unit_sim.poll_reply[0] = 0;
            HostSim host = new HostSim(bus);
            ccTalkNet.ccTalk_acceptor acceptor = new ccTalkNet.ccTalk_acceptor(host.bus, 2);
            host.add_validator(acceptor);
            unit_sim.state = unit_state.WORKING;
            /****************************************************************************/
            Assert.IsTrue(host.is_running(),"Timer not started");
            event_coin = null;
            event_error = null;
            coin_handler_called = false;
            error_handler_called = false;
            host.coin_handler += coin_checker;
            host.error_handler += error_checker;
            //Set the event codes to have a failure and an error
            unit_sim.poll_reply[0] = 2;
            unit_sim.poll_reply[1] = 1;
            unit_sim.poll_reply[2] = 1;
            unit_sim.poll_reply[3] = 0;
            unit_sim.poll_reply[4] = 1;
            Assert.IsTrue(host.is_running(), "Timer not running anymore");
            DateTime _desired = DateTime.Now.AddSeconds(1);
            while (DateTime.Now < _desired)
            {
                Thread.Sleep(1);
                
            }
            Assert.IsTrue(coin_handler_called, "Coin handler not called");
            Assert.IsTrue(error_handler_called, "error handler not called");            
            /*******************************************************************************/
            bus.close();
        }
    }
}
