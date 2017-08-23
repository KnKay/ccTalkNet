using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unit_test
{
    [TestClass]
    public class ccTalk_acceptorTest
    {
        

        [TestMethod]
        public void ccTalkAcceptor_construction()
        {
            //Test if we can create a new acceptor. and checkl if this one is existing!
            ccTalkNet.ccTalk_Bus bus = new unit_sim();

        }

        [TestMethod]
        public void ccTalkAcceptor_get_information()
        {
        }
        
    }
}
