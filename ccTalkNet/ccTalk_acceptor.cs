using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalkNet
{
    /// <summary>
    ///
    /// Class to make handling of coin acceptor a bit more comfortable   
    /// 
    /// </summary>
    public class ccTalk_acceptor : ccTalk_device
    {
        private List<ccTalk_Coin> _coin_list = new List<ccTalk_Coin>();

        public ccTalk_acceptor(ccTalk_host host, Byte address) : base(host, address)
        {
        }

        public ccTalk_acceptor(ccTalk_Bus bus, Byte address) : base(bus, address)
        {
        }

        public override string get_device_info()
        {
            throw new NotImplementedException();
        }

        public String get_coin(Byte channel)
        {
            return _coin_list[channel-1].coin_id;
        }

        public Byte get_sorter_path(Byte channel)
        {
            return _coin_list[channel - 1].sorter_path;
        }

        public Byte get_sorter_path(String coin)
        {
            return _coin_list.Find(x => x.coin_id.Equals(coin)).sorter_path;
        }


        protected override void _init_std_reply()
        {
            base._init_std_reply();
            //After we have the general information we need to get all coin id's
            //This is at the moment implemented for the normal 16 channel mode only!                        
            ccTalk_Message read_information = new ccTalk_Message(new Byte[] { _address, 1, _host_address, 184,0, 0 });            
            for (Byte channel = 1; channel < 17; channel++)
            {
                //modify the message
                read_information.header = 184;
                read_information.payload = new Byte[1] { channel };
                //read the id out of our payload
                string new_coin = Encoding.Default.GetString(_bus.send_ccTalk_Message(read_information).payload);
                if (new_coin != "")
                {
                    read_information.header = 209;
                    _coin_list.Add(new ccTalk_Coin { channel = channel, coin_id = new_coin, sorter_path = _bus.send_ccTalk_Message(read_information).payload[0] });
                }                
            }
        }

    }
}
