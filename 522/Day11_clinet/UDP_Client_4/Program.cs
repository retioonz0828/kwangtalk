using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace UDP_Client_4
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient();
            IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"),19000);
            Console.Write("닉네임 : ");
            string name = Console.ReadLine();

            while(true)
            {
                Console.Write("채팅 입력 : ");
                string message = Console.ReadLine();
                string data = name + " : " + message;
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, data);
                byte[] sendData = stream.ToArray();
                client.Send(sendData, sendData.Length, des_ip);
                stream.Close();
            }
            client.Close();
        }
    }
}
