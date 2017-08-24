using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalkNet
{

    public abstract class ccTalk_device
    {
        protected ccTalk_host _host = null;
        protected ccTalk_Bus _bus = null;
        private Byte _address;
        private int _host_address = 1;
        
        //Values we have in the core commands
        private string _manu_id = null;
        private string _equip_cat_id = null;
        private string _prod_code = null;
        private string _build_code = null;
        //We only want to get these values...
        public string manu_id { get { return _manu_id; } }
        public string equip_cat_id { get { return _equip_cat_id; } }
        public string prod_code { get { return _prod_code; } }
        public string build_code { get { return _build_code; } }



        protected ccTalk_device(ccTalk_host host, Byte address)
        {
            _host = host;
            _address = address;
            _host_address = host.address;
            _bus = host.bus;
            _init_std_reply();
        }

        protected ccTalk_device(ccTalk_Bus bus, Byte address)
        {
            _bus = bus;
            _address = address;
            _init_std_reply();
        }

        public abstract string get_device_info();
        public virtual bool is_available()
        {
            //To see if we are available we perform a poll...
            return poll();
        }

        
        public Boolean poll()
        {
            Byte[] poll_message = new Byte[5] { _address, 0x00, 0x01, 254, 255 };
            return _bus.ack_ccTalk_Bytes(poll_message);
        }

        //We read the ASC chars that are in the core commands!
        private void _init_std_reply()
        {
            ccTalk_Message request_message = new ccTalk_Message(new Byte[5] {_address, 0x00, 0x01, 246, 0x00});
            //Take the payload of our answer and stringify it!
            _manu_id = System.Text.Encoding.Default.GetString( _bus.send_ccTalk_Message(request_message).payload);
            request_message.header = 245;
            _equip_cat_id = System.Text.Encoding.Default.GetString(_bus.send_ccTalk_Message(request_message).payload);
            request_message.header = 244;
            _prod_code = System.Text.Encoding.Default.GetString(_bus.send_ccTalk_Message(request_message).payload);
            request_message.header = 192;
            _build_code = System.Text.Encoding.Default.GetString(_bus.send_ccTalk_Message(request_message).payload);
            return;
        }   
    }
}
