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
    public enum unit_state {INIT, WORKING, FAILURE, ACKFAIL, TIMOUT };

    class unit_sim : ccTalkNet.ccTalk_Bus
    {
        //public Boolean should_fail = false;      
        public answer answer_request  = generate_poll_ack;
        public static Byte[] answer_pl = new Byte[2] { 1, 2};
        public static unit_state state = unit_state.INIT;

        public override Boolean open(String port) { return true; }
        public override ccTalkNet.ccTalk_Message send_ccTalk_Message(ccTalkNet.ccTalk_Message message)
        {
            switch (unit_sim.state)
            {
                case unit_state.ACKFAIL:
                    return new ccTalkNet.ccTalk_Message(answer_request(message.implode()));
                default:
                    return new ccTalkNet.ccTalk_Message(generate_answer(message.implode()));
            }                
        }
        public override Boolean ack_ccTalk_Message(ccTalkNet.ccTalk_Message message) { return !(state == unit_state.ACKFAIL); }
        public override Byte[] send_ccTalk_Bytes(Byte[] message) { return answer_request(message); }
        public override Boolean ack_ccTalk_Bytes(Byte[] message) {
            return !(state == unit_state.ACKFAIL);
        }

        public static Byte[] generate_poll_ack(Byte[] request)
        {
            Byte[] reply_message = new Byte[5] { 0x01, 0x00, 0x02, 0x00, 253 };
            reply_message[0] = request[2];
            reply_message[2] = request[0];
            return reply_message;
        }

        public static Byte[] generate_answer(Byte[] request)
        {
            //The answer depends on the unit state... 
            Byte[] reply_message = null; 
            switch (unit_sim.state)
            {
                case unit_state.INIT:
                    reply_message = generate_answer_sceleton(request, 3);
                    reply_message[4] = (Byte)'K';
                    reply_message[5] = (Byte)'R';
                    reply_message[6] = (Byte)'I';
                    //We know we test the init. This means we will be polled for our ID's
                    switch (request[3]){
                        case 246:
                            return reply_message;                            
                        case 245:
                            return reply_message;
                        case 244:
                            return reply_message;
                        case 192:
                            return reply_message;                   
                    }
                    return null;
                default:
                    reply_message  = generate_answer_sceleton(request, answer_pl.Length);
                    int traget = 4;
                    //Copy our payload
                    for (int i = 0; i < answer_pl.Length; i++)
                    {
                        reply_message[traget + i] = answer_pl[i];
                    }
                    reply_message[reply_message.Length - 1] = ccTalkNet.ccTalk_Message.simple_checksum(reply_message);
                    return reply_message;
            }            
        }

        public static Byte[] generate_answer_sceleton(Byte[] request, int pl_size)
        {
            //We take the src and dest from our request and manipulate it to be a reply. 
            Byte[] sceleton = new Byte[pl_size+5];
            sceleton[0] = request[2]; 
            sceleton[2] = request[0];
            sceleton[1] = (Byte)pl_size;
            return sceleton;
        }

    }
}
