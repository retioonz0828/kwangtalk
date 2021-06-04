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
    public partial class Form2 : Form
    {
        public TextBox ipTextBox;
        public TextBox nickNameTextBox;

        public Form2()
        {
            InitializeComponent();
            this.ipTextBox = textBox1;
            this.nickNameTextBox = textBox3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))// ip가 비어있으면 출력
            {
                MessageBox.Show("ip를 입력해주세요");
            }
            else if(string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("닉네임을 입력해주세요");

               
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}