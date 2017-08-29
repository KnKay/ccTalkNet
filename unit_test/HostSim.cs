using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test
{
    class HostSim :  ccTalkNet.ccTalk_Host
    {
       public HostSim(ccTalkNet.ccTalk_Bus bus):base(bus)
        {
            
        }

        public void poll_acceptors()
        {
            _poll_acceptors();
        }

        public bool is_running()
        {
            return _poll_timer.Enabled;
        }

        

    }
}