using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public Byte events = 0;
        public Byte[] last_event_poll = null;  //To be sure we can getr any information about our last events      
        public ccTalk_Message buffer_read;
        public bool has_lost_events = false;
        public event EventHandler<ccTalk_Coin> coin_handler;
        public event EventHandler<Error_event> error_handler;
        private Byte[] _act_inh = new Byte[]{0,0 };
        public Byte[] coin_inhibits
        {
            get
            {
                return _bus.send_ccTalk_Message(
                     new ccTalk_Message(new Byte[] { _address, 0, _host_address, 230, 0 })
                    ).payload;                
            }
            set
            {
                _manipulate_inhibits(value[0],value[1]);
            }
        } 
        
        public bool master_inhibit {
            get
            {  // 0 - master inhibit active
                Byte m_inhibit = _bus.send_ccTalk_Message(
                     new ccTalk_Message(new Byte[] { _address, 0, _host_address, 227, 0, 0 })
                    ).payload[0];
                if (m_inhibit == 0x00)
                    return true;
                return false;
            }
            set {
                Byte send_byte = 0xFF;
                if (value == true)
                    send_byte = 0x00;
                ccTalk_Message modify_message = new ccTalk_Message(new Byte[] { _address, 1, _host_address, 228, send_byte, 0 });
                _bus.ack_ccTalk_Message(modify_message);
                return;
            }
        }

        public ccTalk_acceptor(ccTalk_Host host, Byte address) : base(host, address)
        {
            buffer_read = new ccTalk_Message(new Byte[] { address, 0, host.address, 229, 0 });
        }

        public ccTalk_acceptor(ccTalk_Bus bus, Byte address) : base(bus, address)
        {
           buffer_read = new ccTalk_Message(new Byte[] { address, 0, 1, 229, 0 });
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
            //We set back the unit back!                                            
            ccTalk_Message read_information = new ccTalk_Message(new Byte[] { _address, 1, _host_address, 1,0, 0 });
            _bus.send_ccTalk_Bytes(new Byte[5] { 2, 0, 1, 1, 252 });
            Thread.Sleep(1000);
            read_information.header = 184;
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
        
        public Boolean get_credit_error_codes()
        {
                      
            //We get the payload and check if we have any new messages.
            last_event_poll = _bus.send_ccTalk_Message(buffer_read).payload;
            //ToDo: Correct the 255 jump            
            Byte event_count = handle_events(last_event_poll[0]);                                    
            if (event_count > 0)
            {
                for (int an_event = 0; an_event < 2* event_count; an_event += 2)
                {
                    if(last_event_poll[an_event+1] == 0)
                    {
                        Error_event e = ccTalk_Error.get_error(last_event_poll[an_event + 2]);
                        error_handler?.Invoke(this, e);                        
                    }
                    else
                    {                        
                        ccTalk_Coin coin = _coin_list[last_event_poll[an_event + 1] - 1];
                        coin.sorter_path = last_event_poll[an_event + 2]; 
                        coin_handler?.Invoke(this, coin);                      
                    }
                }
            }                            
            events = last_event_poll[0]; //We handeld all events. Now we are on level....
            return (event_count > 0);            
        }     

        private Byte handle_events(Byte unit_events)
        {
            Byte events_to_handle = (Byte)(unit_events - events);
            //Check if we lost something 
            if (events_to_handle > 5)
            {
                has_lost_events = true;
                events_to_handle = 5;
            }
            else
            {
                has_lost_events = false;
            }
            return events_to_handle;
        }

        private bool _manipulate_inhibits(Byte upper_mask, Byte lower_mask)
        {
            ccTalk_Message modify_message = 
                new ccTalk_Message(new Byte[] { _address, 2, _host_address, 231, upper_mask, lower_mask, 0 });            
            return _bus.ack_ccTalk_Message(modify_message);
        }
           
    }
}
