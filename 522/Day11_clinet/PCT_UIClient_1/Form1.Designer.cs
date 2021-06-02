namespace PCT_UIClient_1
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnconnect = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbBoard = new System.Windows.Forms.TextBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.tbInputID = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbIPAddress = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "아이디 입력";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(329, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 24;
            this.label3.Text = "채팅내용";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = "IP Address";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(30, 182);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(264, 49);
            this.btnDisconnect.TabIndex = 21;
            this.btnDisconnect.Text = "연결해제";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnconnect
            // 
            this.btnconnect.Location = new System.Drawing.Point(30, 126);
            this.btnconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnconnect.Name = "btnconnect";
            this.btnconnect.Size = new System.Drawing.Size(264, 49);
            this.btnconnect.TabIndex = 20;
            this.btnconnect.Text = "연결하기";
            this.btnconnect.UseVisualStyleBackColor = true;
            this.btnconnect.Click += new System.EventHandler(this.btnco_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(30, 309);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(264, 64);
            this.btnRegister.TabIndex = 19;
            this.btnRegister.Text = "등록하기";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(30, 604);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(120, 60);
            this.btnSend.TabIndex = 18;
            this.btnSend.Text = "전송하기";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbBoard
            // 
            this.tbBoard.Location = new System.Drawing.Point(331, 71);
            this.tbBoard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbBoard.Multiline = true;
            this.tbBoard.Name = "tbBoard";
            this.tbBoard.Size = new System.Drawing.Size(318, 580);
            this.tbBoard.TabIndex = 17;
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(30, 380);
            this.tbMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(263, 215);
            this.tbMessage.TabIndex = 16;
            this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
            // 
            // tbInputID
            // 
            this.tbInputID.Location = new System.Drawing.Point(30, 256);
            this.tbInputID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbInputID.Multiline = true;
            this.tbInputID.Name = "tbInputID";
            this.tbInputID.Size = new System.Drawing.Size(263, 44);
            this.tbInputID.TabIndex = 15;
            this.tbInputID.TextChanged += new System.EventHandler(this.tbInputID_TextChanged);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(137, 71);
            this.tbPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPort.Multiline = true;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(156, 30);
            this.tbPort.TabIndex = 14;
            // 
            // tbIPAddress
            // 
            this.tbIPAddress.Location = new System.Drawing.Point(137, 34);
            this.tbIPAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbIPAddress.Multiline = true;
            this.tbIPAddress.Name = "tbIPAddress";
            this.tbIPAddress.Size = new System.Drawing.Size(156, 29);
            this.tbIPAddress.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 603);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 60);
            this.button1.TabIndex = 26;
            this.button1.Text = "파일전송";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 680);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnconnect);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbBoard);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.tbInputID);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbIPAddress);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnconnect;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbBoard;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.TextBox tbInputID;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbIPAddress;
        private System.Windows.Forms.Button button1;
    }
}

