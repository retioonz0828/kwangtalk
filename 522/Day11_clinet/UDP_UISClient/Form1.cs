using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP_UISClient
{
    public partial class Form1 : Form
    {

        UdpClient client;
        IPEndPoint des_ip;

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Click(object sender, EventArgs e)
        {
            client = new UdpClient();
            IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse(ipaddress.Text+""),int.Parse(port.Text));


            string data = tbdata.Text;
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            client.Send(byteData, byteData.Length, des_ip);

            client.Close();

        }
    }
}
