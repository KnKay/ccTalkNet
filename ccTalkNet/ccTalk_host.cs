using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalkNet
{


    /**
     * The "host" will handle all operations between machine
     * and all ccTalk devices. 
     * 
     * This is just an abstraction layer. All bux communication 
     * will be done by this. The devices etc. must not been interfaced 
     * directly. 
     * 
     */
    public class ccTalk_host
    {
        private List<ccTalk_acceptor> _coin_in = new List<ccTalk_acceptor>();
        private List<ccTalk_hopper> _coin_out = new List<ccTalk_hopper>();
        private ccTalkBus _bus = new ccTalkBus();

    }

    
}
