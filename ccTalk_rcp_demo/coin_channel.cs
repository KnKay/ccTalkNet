using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalk_rcp_demo
{
    class coin_channel 
    {
        public Boolean inhibited = false;
        public string coin_id { get { return coin.coin_id; } }
        public int channel { get { return coin.channel; } }
        public int sorter_path { get { return coin.sorter_path; } }

        private ccTalkNet.ccTalk_Coin coin;
        public coin_channel(ccTalkNet.ccTalk_Coin  a_coin, bool a_inhibited)
        {
            coin = a_coin;
            inhibited = a_inhibited;
        }



    }
}
