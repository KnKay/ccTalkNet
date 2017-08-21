using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalkNet
{
    public delegate Byte checksum_delegate(Byte[] bytes);

    public class ccTalk_Message
    {
        private Byte _src = 0x00;
        private Byte _dest = 0x00;
        private Byte _header = 0x00;
        private Byte _checksum = 0x00;
        private Byte[] _payload = null;

        //Setter and getter, any change will force recalc of checksum!
        public Byte src { get { return _src; } set { _src = value; _checksum = _calc_check(); } }
        public Byte dest { get { return _dest; } set { _dest = value; _checksum = _calc_check(); } }
        public Byte header { get { return _header; } set { _header = value; _checksum = _calc_check(); } }
        public Byte checksum { get { return _calc_check(); } }
        public Byte[] payload { get { return _payload; } set { _payload = value; _checksum = _calc_check(); } }
        public Byte data_bytes { get
            {
                if (_payload != null)
                    return (Byte)_payload.Length;
                return 0;
            } }
        public Boolean had_valid_checksum = false; 

        public checksum_delegate calc_check_method = simple_checksum;


        public ccTalk_Message()
        {

        }

        public ccTalk_Message(checksum_delegate checksum_algo)
        {
            calc_check_method = checksum_algo;
        }

        public ccTalk_Message(Byte[] raw)
        {            
            _dest = raw[0];
            _src = raw[2];
            _header = raw[3];
            int pl_size = raw[1];
            if (pl_size > 0)
            {
                _payload = new Byte[pl_size];
                int databyte = 0;
                while (databyte < pl_size)
                {
                    _payload[databyte] = raw[4 + databyte];
                    databyte++;
                }
            }
            _checksum = _calc_check();
            had_valid_checksum = (_checksum == raw[raw.Length - 1]);
                      
        }

        public ccTalk_Message(Byte[] raw, checksum_delegate checksum_algo) : this(raw)
        {
            calc_check_method = checksum_algo;
        }

        public Byte[] implode ()
        {
            Byte[] serialized = new Byte[data_bytes + 5];
            serialized[0] = _dest;
            serialized[1] = data_bytes;
            serialized[2] = _src;
            serialized[3] = _header;
            int offset = 4;
            //not too nice... 
            if (serialized[1]  > 0)
            {
                foreach (Byte databyte in _payload)
                {
                    serialized[offset] = databyte;
                    offset++;
                }
            }
            serialized[serialized.Length - 1] = _checksum;
            return serialized;
        }

        private Byte _calc_check()
        {
            return calc_check_method(this.implode());            
        }
               
        //Static functions for checksum calc
        public static Byte simple_checksum(Byte[] bytes)
        {            
            Byte sum = 0;
            //force the checksum 0
            bytes[bytes.Length - 1] = 0x00;
            foreach (Byte databyte in bytes)
            {
                sum += databyte;
            }
            return (Byte)(256 - (int)sum);
        }
    }
}
