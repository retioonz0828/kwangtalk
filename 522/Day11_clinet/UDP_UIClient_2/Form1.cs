using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP_UIClient_2
{
    public partial class Form1 : Form
    {
        UdpClient udp;
        IPEndPoint des_ip;
        public string name="";
        public Form1()
        {
            InitializeComponent();
            btnOut.Enabled = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //TTL(TimeToLive) : 네트워크 장비(라우터)를 거칠때마다 TTL값이
        //1씩 감소해 TTL이 0이 되면 데이터가 소멸됨
        //TTL 이 높을 수록 물리적으로 멀리 떨어진 UDP소켓까지 데이터 전송이 가능함

        private void button1_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            string data = name + " : " + tbMessage.Text;
            formatter.Serialize(stream, data);
            byte[] sendData = stream.ToArray();
            udp.Send(sendData, sendData.Length, des_ip);
            stream.Close();
            tbMessage.Text = "";
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            
            udp = new UdpClient(int.Parse(tbPort.Text));
            udp.Ttl = 100;
            des_ip = new IPEndPoint(IPAddress.Parse(tbIPAddress.Text + ""), int.Parse(tbPort.Text));

            udp.JoinMulticastGroup(IPAddress.Parse(tbIPAddress.Text + ""));
            btnJoin.Enabled = false;
            btnOut.Enabled = true;
            Task task = new Task(new Action(reciveTask));
            task.Start();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            udp.DropMulticastGroup(IPAddress.Parse(tbIPAddress.Text + ""));
            udp.Close();
            btnOut.Enabled = false;
            btnJoin.Enabled = true;
        }

        public void reciveTask()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            while(true)
            {
                byte[] recvData = udp.Receive(ref des_ip);
                MemoryStream stream = new MemoryStream(recvData);
                string message = (string)formatter.Deserialize(stream);
                if (tbBoard.InvokeRequired)
                {
                    tbBoard.Invoke(new Action(delegate
                    {
                        tbBoard.AppendText(message + Environment.NewLine);
                    }));
                   
                }
                stream.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            name = tbInputID.Text;
            MessageBox.Show("등록되었습니다!!");
            btnRegister.Enabled = false;
        }
    }
}
