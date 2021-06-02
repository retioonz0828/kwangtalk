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
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}