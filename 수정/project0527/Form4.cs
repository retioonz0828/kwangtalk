using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project0527
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public string[] items;
        public string answer;

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            DateTime dt = DateTime.Now;
            Random rand = new Random(dt.Millisecond);
            int num = rand.Next(1, items.Length);

            for (int i = num; i < num + 50; i++)
            {
                Thread.Sleep(50);
                label1.Text = items[i % items.Length];
                label1.Refresh();
            }
            label2.Text = "님 당첨!";
            answer = label1.Text;


            button1.Enabled = true;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
