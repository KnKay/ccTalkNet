using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cli_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            ccTalkNet.ccTalkBus bus = new ccTalkNet.ccTalkBus();
            bus.open("COM10");
            /*
             * This has been to test the general sending. This is some how confusing the unit!!!
             * 
                        Byte[] test_bytes = new Byte[20] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a,
                                                           0x11, 0x12, 0x13, 0x44, 0x45, 0x16, 0x17, 0x18, 0x19, 0x1a };
                        Console.WriteLine(test_bytes[16]);
                        Console.WriteLine(bus.write_direct(test_bytes)[16]);
            */

        

            Byte[] test_bytes = new Byte[5] { 0x02, 0x00, 0x01, 254, 0x00 };
            test_bytes[4] = ccTalkNet.ccTalk_Message.simple_checksum(test_bytes);
            Byte[] ack = new Byte[5] { 0x01, 0x00, 0x02, 0x00, 253 };
            //We write 4 bytes and expect them back!             
            Byte[] echo = null;
            Console.WriteLine("Testing send of Byte and get an Answer!");
            for (int i = 0; i < 5; i++)
            {
                echo = bus.send_ccTalk_Bytes(test_bytes);
                Console.Write("Expected ");
                foreach (Byte value in ack)
                {
                    Console.Write(value);
                    Console.Write("\t");
                }
                Console.WriteLine(" ");
                Console.Write("Received ");
                foreach (Byte value in echo)
                {
                    Console.Write(value);
                    Console.Write("\t");
                }
                Console.WriteLine(" ");
                
            }
            Console.WriteLine("____________________________________________________");
            Console.WriteLine("Testing send of Byte and get an Ack!");
            for (int i = 0; i < 5; i++)
            {                                
                Console.WriteLine(bus.ack_ccTalk_Bytes(test_bytes));
            }
            Console.WriteLine("____________________________________________________");
            bus.close();
            Console.ReadKey();
        }
    }
}
