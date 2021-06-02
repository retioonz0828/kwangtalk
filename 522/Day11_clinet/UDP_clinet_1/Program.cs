using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_clinet_1
{
    class Program
    {
        static void Main(string[] args)
        {
            //포트번호를 고정하지 않으면
            //임시 포트번호가 발급되면서 클라이언트로 동작
            UdpClient client = new UdpClient();
            //

            IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000);

            while(true)
            {
                Console.Write("입력 : ");
                string data = Console.ReadLine();
                if(data.Equals("quit"))
                {
                    break;
                }
                byte[] byteData = Encoding.UTF8.GetBytes(data);
                //(어떤데이터를 보낼것인지,    데이터의 길이가 어떻게 되는지,    누구한테 보낼것인지)
                client.Send(byteData, byteData.Length, des_ip);
            }
            client.Close();
        }
    }
}
