using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace cli_demo
{
    class Program
    {

        static void coin_checker(object sender, ccTalkNet.ccTalk_Coin_speficic e)
        {
            Console.WriteLine(e.coin.coin_id + " acceted in" + e.coin.sorter_path);            
        }

        static void error_checker(object sender, ccTalkNet.Error_event_specific e)
        {
            Console.WriteLine(e.error.error);
        }


        static void Main(string[] args)
        {
            ccTalkNet.ccTalk_Bus bus = new ccTalkNet.ccTalk_Bus();
            Console.WriteLine("Please enter Port to use");
            foreach (String portname in SerialPort.GetPortNames()){
                Console.WriteLine(portname);
            }
            string port = Console.ReadLine();
            // port = port.ToUpper();
            bus.open(port);
            get_information(bus);            
            ccTalkNet.ccTalk_Host host = new ccTalkNet.ccTalk_Host(bus);
            ccTalkNet.ccTalk_RcpAcceptor eagle = new ccTalkNet.ccTalk_RcpAcceptor(host, 2);
<<<<<<< HEAD
            host.add_validator(eagle);            
            
            
=======
            host.add_validator(eagle);
            Console.WriteLine("Setting inhibits");
>>>>>>> master
            eagle.master_inhibit = false;
            eagle.coin_inhibits = new byte[] { 0xff, 0xff };
            Console.WriteLine("Start Poll");
            host.error_handler += error_checker;
            host.coin_handler += coin_checker;
            DateTime _desired = DateTime.Now.AddSeconds(30);
            while (DateTime.Now < _desired)
            {
                Thread.Sleep(1);
            }
            Console.WriteLine("We are done! This seem to work...");
            
            bus.close();
            Console.ReadKey();
        }


        /*********************************************************
         * Test for getting some information. Just to prove we   *
         * can write and read the bus, get unit information...   *
         *                                                       *
         * *******************************************************/
        public static void get_information(ccTalkNet.ccTalk_Bus bus)
        {
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
            Console.WriteLine("____________________________________________________\n");
            Console.WriteLine("Testing send of Byte and get an Ack!");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(bus.ack_ccTalk_Bytes(test_bytes));
            }
            Console.WriteLine("____________________________________________________\n");
            Console.WriteLine("Testing send of ccTalkMessage and get an Ack!");
            ccTalkNet.ccTalk_Message poll = new ccTalkNet.ccTalk_Message();
            poll.dest = 2;
            poll.src = 1;
            poll.header = 254;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(bus.ack_ccTalk_Message(poll));
            }
            Console.WriteLine("____________________________________________________\n");
            Console.WriteLine("Testing send of ccTalkMessage and get an Ack as Message!");
            ccTalkNet.ccTalk_Message result = null;
            for (int i = 0; i < 5; i++)
            {
                result = bus.send_ccTalk_Message(poll);
                Console.WriteLine(result);
            }
            Console.WriteLine("____________________________________________________\n");
            Console.WriteLine("Reading information of unit!");
            ccTalkNet.ccTalk_acceptor eagle = new ccTalkNet.ccTalk_acceptor(bus, 2);
            Console.WriteLine("Type: " + eagle.equip_cat_id);
            Console.WriteLine("Unit: " + eagle.prod_code);
            Console.WriteLine("Manufacturer: " + eagle.manu_id);
            Console.WriteLine("Coins: ");
            for (Byte channel = 1; channel < 17; channel++)
            {
                Console.WriteLine("\t" + eagle.get_coin(channel) + " to path " + eagle.get_sorter_path(channel).ToString());
            }
            Console.WriteLine("____________________________________________________\n");
        }
    }
}
