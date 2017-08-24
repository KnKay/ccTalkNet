using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace ccTalkNet
{
    public enum ccTalk_host_states { CREATED, WORKING, FAILURE };
    public class ccTalk_host_states_EventArgs : EventArgs
    {
        public ccTalk_host_states state { get; set; }
    }


    /// <summary>
    ///
    /// The "host" will handle all operations between machine
    /// and all ccTalk devices. 
    ///
    /// This is just an abstraction layer. All bux communication 
    /// will be done by this. The devices etc. must not been interfaced 
    /// directly.     
    /// 
    /// </summary>
    public class ccTalk_host
    {
        private Dictionary<string, ccTalk_acceptor> _coin_in
            = new Dictionary<string, ccTalk_acceptor>();
        private Dictionary<string, ccTalk_hopper> _coin_out
            = new Dictionary<string, ccTalk_hopper>();
        protected ccTalk_Bus _bus = new ccTalk_Bus();
        private string _bus_port;
        private List<Byte[]> _message_que = new List<byte[]>();
        private System.Timers.Timer _timer = new System.Timers.Timer(100);
        private int _address = 1;

        public int address { get { return _address; } set { _address = value; } }
        public ccTalk_Bus bus {get{return _bus ;} }

        public ccTalk_host(string a_port)
        {
            _bus_port = a_port;
        }

        public bool open()
        {
            return _bus.open(_bus_port);
        }

        public void add_hopper(ccTalk_hopper a_hopper)
        {
            
        }

        public void add_validator(ccTalk_acceptor a_acceptor)
        {
            //read the name and the address of the acceptor as identifier

        }

    }
}
