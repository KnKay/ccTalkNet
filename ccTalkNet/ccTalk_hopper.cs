﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalkNet
{
    public class ccTalk_hopper : ccTalk_device
    {

        public ccTalk_hopper(ccTalk_host host, Byte address) : base(host, address)
        {
        }

        public ccTalk_hopper(ccTalk_Bus bus, Byte address) : base(bus, address)
        {
        }


        public override string get_device_info()
        {
            throw new NotImplementedException();
        }

        public override bool is_available()
        {
            throw new NotImplementedException();
        }
    }
}
