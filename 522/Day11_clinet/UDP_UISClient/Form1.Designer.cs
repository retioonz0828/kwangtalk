namespace UDP_UISClient
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.ipaddress = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.tbdata = new System.Windows.Forms.TextBox();
            this.btn = new System.Windows.Forms.Button();
            this.d = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ipaddress
            // 
            this.ipaddress.Location = new System.Drawing.Point(212, 31);
            this.ipaddress.Multiline = true;
            this.ipaddress.Name = "ipaddress";
            this.ipaddress.Size = new System.Drawing.Size(153, 41);
            this.ipaddress.TabIndex = 0;
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(212, 78);
            this.port.Multiline = true;
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(153, 41);
            this.port.TabIndex = 1;
            // 
            // tbdata
            // 
            this.tbdata.Location = new System.Drawing.Point(22, 125);
            this.tbdata.Multiline = true;
            this.tbdata.Name = "tbdata";
            this.tbdata.Size = new System.Drawing.Size(343, 110);
            this.tbdata.TabIndex = 2;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(22, 263);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(343, 44);
            this.btn.TabIndex = 3;
            this.btn.Text = "전송하기";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // d
            // 
            this.d.AutoSize = true;
            this.d.Location = new System.Drawing.Point(97, 45);
            this.d.Name = "d";
            this.d.Size = new System.Drawing.Size(63, 12);
            this.d.TabIndex = 4;
            this.d.Text = "IPAddress";
            this.d.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "PORT";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.d);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.tbdata);
            this.Controls.Add(this.port);
            this.Controls.Add(this.ipaddress);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipaddress;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.TextBox tbdata;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Label d;
        private System.Windows.Forms.Label label2;
    }
}

