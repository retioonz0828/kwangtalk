using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace UDP_Client_3
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient();
            IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("224.0.1.1"), 14000);

            Console.Write("아이디 입력 : ");
            string name = Console.ReadLine();

            while(true)
            {
                Console.Write("데이터 입력 : ");
                string message = Console.ReadLine();
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                string data = name + " : " + message;
                formatter.Serialize(stream, data);
                byte[] sendData = stream.ToArray();
                client.Send(sendData, sendData.Length, des_ip);
                stream.Close();
                
            }
            client.Close();
        }
    }
}
