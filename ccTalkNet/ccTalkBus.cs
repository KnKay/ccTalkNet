using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalkNet
{
    /*
     * This class will be the "physical" implementation of ccTalk
     * We can only send and receive messages. 
     */

    class ccTalkBus
    {
        private SerialPort _serial;
        private int _baudrate = 9600;
        private Boolean _is_open = false;

        public Boolean open(String port)
        {
            //Configure start stop etc...
            if (!_is_open)
            {
                _serial.BaudRate = _baudrate;
                _serial.Parity = Parity.None;
                _serial.StopBits = StopBits.One;
                //Catch Jankowsky Bullshit for mini hub
                _serial.DtrEnable = false;                
            }
            _serial.PortName = port;
            _serial.Open();
            return true;
        }


        public ccTalk_Message send_ccTalk_Message(ccTalk_Message message)
        {
            return new ccTalk_Message();
        }
        
        public Boolean ack_ccTalk_Message(ccTalk_Message message)
        {
            return false;
        }


        public Byte[] send_ccTalk_Bytes(Byte[] message)
        {
            return new Byte[10];
        }        

        public Boolean ack_ccTalk_Bytes(Byte[] message)
        {
            return false;
        }

        //Protected functions to write and read messages
        protected Byte[] read_from_bus(int size = 0)
        {
            return new Byte[10];
        }

        protected Boolean write_to_bus(Byte[] message)
        {
            //Write the message

            //read the anser
            
            return false;
        }

    }
}
