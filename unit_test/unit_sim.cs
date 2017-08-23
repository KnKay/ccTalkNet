using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test
{

    /** 
     * To be sure we do things like we think we generate our own answers...
     * 
     */
    public delegate Byte[] answer (Byte[] bytes);

    class unit_sim : ccTalkNet.ccTalk_Bus
    {
        public Boolean should_fail = false;
        public Boolean should_timeout = false;        
        answer answer_request  = generate_poll_ack;
        public static Byte[] answer_pl = new Byte[2] { 1, 2};

        public override Boolean open(String port) { return true; }
        public override ccTalkNet.ccTalk_Message send_ccTalk_Message(ccTalkNet.ccTalk_Message message)
            { return new ccTalkNet.ccTalk_Message(answer_request(message.implode())); }
        public override Boolean ack_ccTalk_Message(ccTalkNet.ccTalk_Message message) { return !should_fail; }
        public override Byte[] send_ccTalk_Bytes(Byte[] message) { return answer_request(message); }
        public override Boolean ack_ccTalk_Bytes(Byte[] message) { return !should_fail; }

        public static Byte[] generate_poll_ack(Byte[] request)
        {
            Byte[] reply_message = new Byte[5] { 0x01, 0x00, 0x02, 0x00, 253 };
            reply_message[0] = request[2];
            reply_message[2] = request[0];
            return reply_message;
        }

        public static Byte[] generate_answer(Byte[] request)
        {
            Byte[] reply_message = new Byte[answer_pl.Length + 5];
            reply_message[0] = request[2];
            reply_message[2] = request[0];
            int traget = 4;
            //Copy our payload
            for (int i = 0; i<answer_pl.Length; i++)
            {
                reply_message[traget + i] = answer_pl[i];
            }
            reply_message[reply_message.Length - 1] = ccTalkNet.ccTalk_Message.simple_checksum(reply_message);
            return reply_message;
        }

    }
}
