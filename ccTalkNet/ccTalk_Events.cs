using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ccTalkNet
{
    
    public class ccTalk_Coin : EventArgs  { public Byte channel; public Byte sorter_path; public string coin_id; };
    public class Error_event : EventArgs { public string error; public Boolean reject; };
    public class ccTalk_Coin_speficic : EventArgs { public ccTalk_Coin coin; public object device; };
    public class Error_event_specific : EventArgs { public Error_event error; public object device; };

    public abstract class ccTalk_Error
    {     
        public static Error_event get_error(Byte error_number)
        {
            Error_event error;
            _errors.TryGetValue(error_number, out error);
            return error;
        }

        private static Dictionary<Byte, Error_event> _errors = new Dictionary<Byte, Error_event>()
        {
            {0,   new Error_event { error = "OK, error resolved",           reject = false } },
            {1,   new Error_event { error = "Rejected coin" ,               reject = true } },
            {5,   new Error_event { error = "Validator timeout",            reject = true } },
            {6,   new Error_event { error = "Credit Sensor timeout",        reject = false } },
            {8,   new Error_event { error = "2nd close coin",               reject = false } },
            {10,  new Error_event { error = "Credit sensor not ready",      reject = false } },
            {11,  new Error_event { error = "Sorter not ready",             reject = false } },
            {14,  new Error_event { error = "Credit sensor blocked",        reject = false } },
            {15,  new Error_event { error = "Sorter opto blocked",          reject = false } },
            {17,  new Error_event { error = "Coin going backwards",         reject = false } },
            {20,  new Error_event { error = "C.O.S mechanism activated",    reject = false } },
            {21,  new Error_event { error = "DCE opto  timeout",            reject = false } },
            {22,  new Error_event { error = "DCE not seen",                 reject = false } },
            {24,  new Error_event { error = "Rejected coin",                reject = false } },
            {29,  new Error_event { error = "Accept gate open not closed",  reject = false } },
            {30,  new Error_event { error = "Accept gate closed not open",  reject = false } },
            {31,  new Error_event { error = "Manifold opto timeout",        reject = false } },
            {32,  new Error_event { error = "Manifold opto blocked",        reject = false } },
            {39,  new Error_event { error = "Coin incorrect sorted",        reject = false } },
            {40,  new Error_event { error = "External light attack",        reject = false } },
            {115, new Error_event { error = "Escrow cycle finished",        reject = false } },
            {117, new Error_event { error = "Double signal coin on exit",   reject = false } },
            {118, new Error_event { error = "Disk stalled",                 reject = false } },
            {119, new Error_event { error = "Diskmotor overcurrent",        reject = false } },
            {120, new Error_event { error = "External light",               reject = false } },
            {121, new Error_event { error = "Validation Sensor Blocked",    reject = false } },
            {128, new Error_event { error = "Inhibited coin 1",             reject = false } },
            {129, new Error_event { error = "Inhibited coin 2",             reject = false } },
            {130, new Error_event { error = "Inhibited coin 3",             reject = false } },
            {131, new Error_event { error = "Inhibited coin 4",             reject = false } },
            {132, new Error_event { error = "Inhibited coin 5",             reject = false } },
            {133, new Error_event { error = "Inhibited coin 6",             reject = false } },
            {134, new Error_event { error = "Inhibited coin 7",             reject = false } },
            {135, new Error_event { error = "Inhibited coin 8",             reject = false } },
            {136, new Error_event { error = "Inhibited coin 9",             reject = false } },
            {137, new Error_event { error = "Inhibited coin 10",            reject = false } },
            {139, new Error_event { error = "Inhibited coin 11",            reject = false } },
            {140, new Error_event { error = "Inhibited coin 12",            reject = false } },
            {141, new Error_event { error = "Inhibited coin 13",            reject = false } },
            {142, new Error_event { error = "Inhibited coin 14",            reject = false } },
            {143, new Error_event { error = "Inhibited coin 15",            reject = false } },
            {144, new Error_event { error = "Inhibited coin 16",            reject = false } },
            {145, new Error_event { error = "Inhibited coin 17",            reject = false } },
            {146, new Error_event { error = "Inhibited coin 18",            reject = false } },
            {147, new Error_event { error = "Inhibited coin 19",            reject = false } },
            {148, new Error_event { error = "Inhibited coin 20",            reject = false } },
            {149, new Error_event { error = "Inhibited coin 21",            reject = false } },
            {150, new Error_event { error = "Inhibited coin 22",            reject = false } },
            {151, new Error_event { error = "Inhibited coin 23",            reject = false } },
            {152, new Error_event { error = "Inhibited coin 24",            reject = false } },
            {153, new Error_event { error = "Inhibited coin 25",            reject = false } },
            {154, new Error_event { error = "Inhibited coin 26",            reject = false } },
            {155, new Error_event { error = "Inhibited coin 27",            reject = false } },
            {156, new Error_event { error = "Inhibited coin 28",            reject = false } },
            {157, new Error_event { error = "Inhibited coin 29",            reject = false } },
            {158, new Error_event { error = "Inhibited coin 30",            reject = false } },
            {159, new Error_event { error = "Inhibited coin 31",            reject = false } },
            {254, new Error_event { error = "Door open",                    reject = false } },
            {255, new Error_event { error = "Unspecified Alarm",            reject = false } }
        };

        
    }    
}
