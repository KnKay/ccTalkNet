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

    }
}
