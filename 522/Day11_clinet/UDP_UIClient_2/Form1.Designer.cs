namespace UDP_UIClient_2
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
            this.tbIPAddress = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbInputID = new System.Windows.Forms.TextBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.tbBoard = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbIPAddress
            // 
            this.tbIPAddress.Location = new System.Drawing.Point(119, 39);
            this.tbIPAddress.Multiline = true;
            this.tbIPAddress.Name = "tbIPAddress";
            this.tbIPAddress.Size = new System.Drawing.Size(137, 24);
            this.tbIPAddress.TabIndex = 0;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(119, 69);
            this.tbPort.Multiline = true;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(137, 25);
            this.tbPort.TabIndex = 1;
            // 
            // tbInputID
            // 
            this.tbInputID.Location = new System.Drawing.Point(25, 217);
            this.tbInputID.Multiline = true;
            this.tbInputID.Name = "tbInputID";
            this.tbInputID.Size = new System.Drawing.Size(231, 36);
            this.tbInputID.TabIndex = 2;
            this.tbInputID.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(25, 316);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(231, 173);
            this.tbMessage.TabIndex = 3;
            // 
            // tbBoard
            // 
            this.tbBoard.Location = new System.Drawing.Point(289, 69);
            this.tbBoard.Multiline = true;
            this.tbBoard.Name = "tbBoard";
            this.tbBoard.Size = new System.Drawing.Size(279, 465);
            this.tbBoard.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(25, 495);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(231, 48);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "전송하기";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(25, 259);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(231, 51);
            this.btnRegister.TabIndex = 6;
            this.btnRegister.Text = "등록하기";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(25, 113);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(231, 39);
            this.btnJoin.TabIndex = 7;
            this.btnJoin.Text = "가입하기";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(25, 158);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(231, 39);
            this.btnOut.TabIndex = 8;
            this.btnOut.Text = "탈퇴하기";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "IP Address";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(287, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "채팅내용";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "아이디 입력";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 555);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbBoard);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.tbInputID);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbIPAddress);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbIPAddress;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbInputID;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.TextBox tbBoard;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

