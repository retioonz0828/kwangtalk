using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace UDP_client_2
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient();
            IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000);


            while(true)
            {
                Console.Write("입력 : ");
                string data =  Console.ReadLine();

                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
                byte[] byteData = stream.ToArray();

                client.Send(byteData, byteData.Length, des_ip);
            }
            client.Close();
        }
    }
}
