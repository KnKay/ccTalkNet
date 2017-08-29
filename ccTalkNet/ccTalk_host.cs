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
    public class ccTalk_Host
    {
        private Dictionary<string, ccTalk_acceptor> _coin_in
            = new Dictionary<string, ccTalk_acceptor>();
        private Dictionary<string, ccTalk_hopper> _coin_out
            = new Dictionary<string, ccTalk_hopper>();        
        
        private List<Byte[]> _message_que = new List<byte[]>();
        private System.Timers.Timer _timer = new System.Timers.Timer(100);
        private Byte _address = 1;
        //Specific handler will specify the event and the unit!
        public event EventHandler<ccTalk_Coin_speficic> coin_handler;
        public event EventHandler<Error_event_specific> error_handler;
        protected System.Timers.Timer _poll_timer = new System.Timers.Timer { Interval = 100, Enabled = true };
        


        protected ccTalk_Bus _bus = new ccTalk_Bus();

        public Byte address { get { return _address; } set { _address = value; } }
        public ccTalk_Bus bus {get{return _bus ;} }

        public ccTalk_Host(string a_port)
        {            
            _bus.open(a_port);
            _poll_timer.Elapsed += _poll_timer_Elapsed;
        }

        public ccTalk_Host(ccTalk_Bus bus)
        {
            _bus = bus;
            _poll_timer.Elapsed += _poll_timer_Elapsed;
        }

        private void _poll_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _poll_acceptors();
        }

        public void close()
        {
            bus.close();
        }

        public void add_hopper(ccTalk_hopper a_hopper)
        {
            
        }

        public void add_validator(ccTalk_acceptor a_acceptor)
        {
            //read the name and the address of the acceptor as identifier
            a_acceptor.coin_handler += handle_coins;
            a_acceptor.error_handler += handle_error;
            _coin_in.Add(a_acceptor.prod_code, a_acceptor);
        }

        protected Boolean _poll_acceptors()
        {
            Boolean had_an_event = false;
            foreach (ccTalk_acceptor acceptor in _coin_in.Values)
            {
                if (acceptor.get_credit_error_codes())
                    had_an_event = true;
            }
            return had_an_event;
        }

        protected void handle_coins(object sender, ccTalk_Coin a_coin)
        {
            coin_handler?.Invoke(this, new ccTalk_Coin_speficic() { coin = a_coin, device = sender });
        }

        protected void handle_error(object sender, Error_event an_error)
        {
            error_handler?.Invoke(this, new Error_event_specific() { error = an_error, device = sender });
        }

    }
}
