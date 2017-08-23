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
        private int _address;        

        protected ccTalk_device(ccTalk_host host, int address)
        {
            _host = host;
            _address = address;
        }

        protected ccTalk_device(ccTalk_Bus bus, int address)
        {
            _bus = bus;
            _address = address;
        }

        public abstract string get_device_info();
        public abstract bool is_available();
        public abstract void poll();

    }
}
