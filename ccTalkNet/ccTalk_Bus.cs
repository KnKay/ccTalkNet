using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ccTalkNet
{

    public enum ccTalk_Bus_State { CLOSED, OPEN, FAILURE, READTIMEOUT };

    public class ccTalk_Bus_EventArgs : EventArgs
    {
        public ccTalk_Bus_State state { get; set; }
    }

    /*
     * This class will be the "physical" implementation of ccTalk
     * We can only send and receive messages. 
     */
    public class ccTalk_Bus
    {
        public EventHandler<ccTalk_Bus_EventArgs> state_changed;
        public ccTalk_Bus_State state { get { return _state; } }

        private SerialPort _serial = new SerialPort();
        private int _baudrate = 9600;
        private ccTalk_Bus_State _state = ccTalkNet.ccTalk_Bus_State.CLOSED;        

        public virtual Boolean open(String port)
        {            
            if(!SerialPort.GetPortNames().Contains(port))
            {
                return false;
            }            
            switch (_state)
            {
                case ccTalkNet.ccTalk_Bus_State.CLOSED:
                    _serial.BaudRate = _baudrate;
                    _serial.Parity = Parity.None;
                    _serial.StopBits = StopBits.One;
                    //Catch Jankowsky Bullshit for mini hub
                    _serial.DtrEnable = false;                    
                    _serial.ReadTimeout = 500;
                    break;
                case ccTalkNet.ccTalk_Bus_State.OPEN:
                    return false;                    
            }
            _serial.PortName = port;
            _serial.Open();
            _state = ccTalk_Bus_State.OPEN;
            return true;
        }

        public virtual ccTalk_Message send_ccTalk_Message(ccTalk_Message message)
        {

            Byte[] reply = null;
            if (_write_to_bus(message.implode()) != null){
                reply = _read_from_bus();
                _flush_serial_input();
            }
            if (reply != null)
                return new ccTalk_Message(reply);
            return new ccTalk_Message();
        }
        
        public virtual Boolean ack_ccTalk_Message(ccTalk_Message message)
        {
            Byte[] reply = null;
            if (_write_to_bus(message.implode()) != null)
            {
                reply = _read_from_bus();
                _flush_serial_input();
            }
            else
            {
                _state = ccTalk_Bus_State.FAILURE;
                return false;
            }
            /*
             * Check if this is a confirm. 
             * This is if the dest is now source and vice versa. 
             * Header and size are 0
             */
            if (reply[0] == message.src
                & reply[2] == message.dest
                & reply[1] == 0
                & reply[3] == 0
                ) return true;
            return false;
        }

        public virtual Byte[] send_ccTalk_Bytes(Byte[] message)
        {
            Byte[] reply = null;
            if (_write_to_bus(message) != null)
            {
                reply = _read_from_bus();
                _flush_serial_input();
            }
            return reply;
        }        

        public virtual Boolean ack_ccTalk_Bytes(Byte[] message)
        {
            Byte[] reply = null;
            if (_write_to_bus(message) != null)
            {
                reply = _read_from_bus();
                _flush_serial_input();
            } else
            {
                _state = ccTalk_Bus_State.FAILURE;
                return false;
            }
            /*
             * Check if this is a confirm. 
             * This is if the dest is now source and vice versa. 
             * Header and size are 0
             */
            if (reply[0] == message[2]
                & reply[2] == message[0]
                & reply[1] == 0
                & reply[3] == 0
                ) return true;
            return false;
        }

        /*      Send and read some bytes directly. 
         *      Send will return the echo! 
         * 
         * ->  Choose Your Enemies Carefully  <-
         */
        public Byte[] write_direct(Byte[] bytes)
        {
            //send some data. Read the same amount back!
            return _write_to_bus(bytes);
        }

        public Byte[] read_direct(int size = 0)
        {
            return _read_from_bus(size);
        }

        //Protected functions to write and read messages
        private Byte[] _read_from_bus(int size = 0)
        {
            Byte[] buffer = null;
            try            {
                if (size == 0)
                {
                    buffer = new Byte[_serial.BytesToRead];
                    _serial.Read(buffer, 0, _serial.BytesToRead);
                }
                else
                {
                    buffer = new Byte[size];
                    _serial.Read(buffer, 0, size);
                    //Read the rest of what ever is on the port to avoid fractual residues
                }
            }
            catch (TimeoutException)
            {
                _state = ccTalk_Bus_State.READTIMEOUT;
                onStateChange();
            }
            return buffer;
        }

        private void _flush_serial_input()
        {
            Byte [] buffer = new Byte[_serial.BytesToRead];
            _serial.Read(buffer, 0, _serial.BytesToRead);
        }

        private Byte[] _write_to_bus(Byte[] bytes)
        {            
            //Write the message
            _serial.Write(bytes, 0, bytes.Length);
            Thread.Sleep(30);
            //read the echo                        
           return _read_from_bus(bytes.Length);                      
        }

        public void close()
        {            
            _serial.Close();
            _state = ccTalk_Bus_State.CLOSED;

        }

        //ToDo: Check VS suggested improvement
        protected virtual void onStateChange()
        {
            EventHandler<ccTalk_Bus_EventArgs> handler = state_changed;
            ccTalk_Bus_EventArgs new_state = new ccTalk_Bus_EventArgs();
            new_state.state = _state;
            if (handler != null)
            {
                handler(this, new_state);
            }
        }
    }
}
