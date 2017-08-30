using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ccTalkNet
{

    public class rcp_error
    {
        public Byte error_code;
        public rcp_error(Byte number)
        {
            error_code = number;
        }
        public static string get_error(int error_number)
        {
            string error;
            _errors.TryGetValue(error_number, out error);
            return error;
        } 
        private static Dictionary<int, string> _errors = new Dictionary<int, string>()
            {
                {255,"Packet too short" },
                {254,"Packet too long" },
                {253,"Too much data" },
                {252,"File verification error 1" },
                {251,"File verification error 2" },
                {240,"Unsupported packet header" },
                {239,"Unsupported file format" },
                {236,"Non-matching database version" },
                {231,"Sensor mismatch" },
                {210,"coin file not uploaded or incomplete" },
                {200,"Internal neural network error" },
                {100,"Illegal coin position" },
                {1,"Non RCP Failure! -> Host-Failure!" },
                {0,"Success" },
            };
    }

    public class ccTalk_RcpAcceptor : ccTalk_acceptor
    {
        public bool is_rcp  { get { return (database_version != 0); } } //If the database is 0 we have no rcp unit
        //RCP specific information of the unit
        public Byte database_version
        {
            get
            {
                return _bus.send_ccTalk_Message(
                     new ccTalk_Message(new Byte[] { _address, 0, _host_address, 243, 0 })
                    ).payload[0];
            }
        }
        public ccTalk_RcpAcceptor(ccTalk_Host host, Byte address) : base(host, address)
        {
            
        }
        public ccTalk_RcpAcceptor(ccTalk_Bus bus, Byte address) : base(bus, address)
        {
            
        }

        public void remove_coin_signature(Byte coin_position)
        {
            Byte[] request = new ccTalk_Message(new Byte[] { _address, 2, _host_address, 96, 249, coin_position, 0 }).implode();
            Byte[] reply = _bus.send_ccTalk_Bytes_with_read_wait(request, 500);
            Encoding.Default.GetString(reply);
        }
    
        public string request_extended_id(Byte coin_position)
        {
            Byte[] answer =  _bus.send_ccTalk_Message(
                     new ccTalk_Message(new Byte[] { _address, 2, _host_address, 96, 248 , coin_position, 0})
                    ).payload;            
            return Encoding.Default.GetString(answer);
        }

        public rcp_error upload_file(string file, Byte coin_position)
        {
            //Try to get the file Opened
            if (!File.Exists(file))
            {
                return new rcp_error(1);
            }

            Byte[] file_content = File.ReadAllBytes(file);

            //Begin upload
            if (!_bus.ack_ccTalk_Message(new ccTalk_Message(new Byte[] { _address, 1, _host_address, 96, 255, 0 })))
                return new rcp_error(1);
            //Upload data in package-...
            ccTalk_Message upload_message = new ccTalk_Message(new Byte[] { _address, 1, _host_address, 96, 0, 0 });
            int act = 0; //The last Byte we read
            int file_size = file_content.Length;
            while (file_size > act)
            {
                Byte[] payload;
                if ((file_size - act) > 200)
                {
                    payload = new Byte[201];
                    payload[0] = 254; //Set the extend header
                    Array.Copy(file_content, act, payload, 1, 200);
                    act += 200; 
                }
                else
                {
                    Byte remain = (Byte)(file_size - act);
                    payload = new Byte[remain + 1];
                    payload[0] = 254;
                    Array.Copy(file_content, act, payload, 1, remain);
                    act = file_size;
                }
                upload_message.payload = payload;
                Byte[] send = upload_message.implode();
                if (!_bus.ack_ccTalk_Bytes(send, 800))
                    return new rcp_error(1);
            }

            ccTalkNet.ccTalk_Message result = 
                _bus.send_ccTalk_Message(new ccTalk_Message
                (new Byte[] { _address, 2, _host_address, 96, 253, coin_position, 0 }), 600);
            if (result.data_bytes >0)
                return new rcp_error(result.payload[0]);           
            return new rcp_error(0);
        }

    }
}
