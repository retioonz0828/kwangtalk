using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCT_UIClient_1
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnco_Click(object sender, EventArgs e)
        {
            client = new TcpClient(tbIPAddress.Text, int.Parse(tbPort.Text));
            stream = client.GetStream();
        

            byte[] recvData = new byte[1024];
            int size = stream.Read(recvData, 0, recvData.Length);
            string welcomeMessage = Encoding.UTF8.GetString(recvData, 0, size);
            tbBoard.Text = welcomeMessage + Environment.NewLine;

            string clientName = tbInputID.Text;
            byte[] sendData = Encoding.UTF8.GetBytes(clientName);
            stream.Write(sendData, 0, sendData.Length);

            Task task = new Task(new Action(reciveTask));
            task.Start();
   
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            stream.Close();
            client.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //btnRegister.PerformClick();

            string message = tbMessage.Text;
            byte[] sendData = Encoding.UTF8.GetBytes(message);
            stream.Write(sendData, 0, sendData.Length);
            tbBoard.AppendText("나의 대화 : " + message + Environment.NewLine);
            tbMessage.Clear();// 전송한번 하면 메시지창 비워주기
        }

        public void reciveTask()
        {
            while(true)
            {
                byte[] recvData = new byte[1024];
                int size = stream.Read(recvData, 0, recvData.Length);
                string message = Encoding.UTF8.GetString(recvData, 0, size);
                if(tbBoard.InvokeRequired)
                {
                    tbBoard.Invoke(new Action(delegate
                    {
                        tbBoard.AppendText("서버의 대화 : " + message + Environment.NewLine);
                    }));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tbInputID_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string clientName = tbInputID.Text;

            string message = tbMessage.Text;
            byte[] sendData = Encoding.UTF8.GetBytes(clientName);
            stream.Write(sendData, 0, sendData.Length);
        }
    }
}
