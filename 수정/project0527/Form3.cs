using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project0527
{
    public partial class Form3 : Form
    {
        public string userNickName;
        public string fileName;

        public Form3(string userNickName, string fileName)
        {
            InitializeComponent();
            this.userNickName = userNickName;
            this.fileName = fileName;

            this.label1.Text = $"{this.userNickName}님이  파일을 전송하셨습니다.";
            this.label2.Text = $"{this.fileName}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}