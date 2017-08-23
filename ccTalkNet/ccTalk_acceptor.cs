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
        public ccTalk_acceptor(ccTalk_host host, int address) : base(host, address)
        {
        }

        public ccTalk_acceptor(ccTalk_Bus bus, int address) : base(bus, address)
        {
        }


        public override string get_device_info()
        {
            throw new NotImplementedException();
        }

        public override bool is_available()
        {
            //We are available if we reply ack to a poll
            return _bus.ack_ccTalk_Bytes(new Byte[5] { 0x02, 0x00, 0x01, 254, 255 });
        }

        public override void poll()
        {
            throw new NotImplementedException();
        }
    }
}
