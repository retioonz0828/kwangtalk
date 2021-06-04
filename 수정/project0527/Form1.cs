using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using static PacketLibrary.Class1;

namespace project0527
{
    public partial class Form1 : Form
    {
        private string[] colors = new string[] { "Red", "Orange", "Blue", "Yellow", "Purple", "Green", "Pink" };
        private string userId;
        private string ip;

        private string myNickName;

        private NetworkStream stream;
        private TcpClient client;

        private List<byte> sendBuffer;
        private List<byte> readBuffer;

        private bool isConnect = false;

        public LoginPacket loginPacket;
        public LoginResponsePacket loginResponsePacket;

        public TextChatPacket textChatPacket;

        public Thread thread;

        private readonly int BUFFER_SIZE = 1024 * 100;

        private Dictionary<string, string> users;

        bool is_same;

        public Form1()
        {
            InitializeComponent();
        }

        async public Task Send()
        {
            byte[] buffer = this.sendBuffer.ToArray();

            await this.stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            this.stream.Flush();

            this.sendBuffer.Clear();
        }

        async public Task SendLoginPacket()
        {
            if (!this.isConnect)
            {
                return;
            }

            this.loginPacket = new LoginPacket(this.ip, this.myNickName, this.userId);

            List<byte> serialized = Packet.Serialize(this.loginPacket).ToList();
            this.sendBuffer = serialized;

            await this.Send().ConfigureAwait(false);
        }

        async public void RUN()
        {
            int nRead = 0;
            byte[] buffer;

            while (this.isConnect)
            {
                try
                {
                    nRead = 0;

                    buffer = new byte[this.BUFFER_SIZE];
                    nRead = await this.stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

                    if (nRead > 0)
                    {
                        Packet packet = (Packet)Packet.DeSerialize(buffer);

                        switch (packet.type)
                        {
                            case PacketType.FILE_RESPONSE:
                                {
                                    //when client receive file
                                    FileResponsePacket fRPacket = (FileResponsePacket)Packet.DeSerialize(buffer);

                                    this.users = fRPacket.users;

                                    if (this.users.Count > 0 && fRPacket.isOk)
                                    {
                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Key} : {user.Value}{Environment.NewLine}");
                                            }
                                        }));
                                    }

                                    if (fRPacket.fileFormat != string.Empty && fRPacket.fileBody != null && fRPacket.isOk)
                                    {
                                        this.Invoke(new MethodInvoker(async () =>
                                        {
                                            Form3 mdF3 = new Form3(fRPacket.sendUserNickName, fRPacket.fileName);
                                            DialogResult F3Result = mdF3.ShowDialog();
                                            if (F3Result == DialogResult.OK)
                                            {
                                                //파일을 받기로 했을 때의 동작

                                                saveFileDialog1.DefaultExt = fRPacket.fileFormat;
                                                DialogResult result = saveFileDialog1.ShowDialog();
                                                if (result == DialogResult.OK)
                                                {
                                                    using (var saveFileStream = saveFileDialog1.OpenFile())
                                                    {
                                                        await saveFileStream.WriteAsync(fRPacket.fileBody, 0, fRPacket.fileBody.Length).ConfigureAwait(false);
                                                    }
                                                }
                                            }
                                            mdF3.Close();
                                        }));
                                    }

                                    break;
                                }

                            case PacketType.LOGIN_RESPONSE:
                                {
                                    //로그인 패킷을 받은 경우
                                    this.loginResponsePacket = (LoginResponsePacket)Packet.DeSerialize(buffer);

                                    this.users = this.loginResponsePacket.users;

                                    if (this.users.Count > 0 && loginResponsePacket.isOk)
                                    {
                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Key} : {user.Value}{Environment.NewLine}");
                                            }
                                        }));
                                    }

                                    is_same = false;
                                    foreach (KeyValuePair<string, string> user in this.users)
                                    {
                                        if (user.Value.Equals(this.myNickName))
                                        {
                                            if (user.Key.Equals(this.userId))
                                            {
                                                if (is_same)
                                                {
                                                    if (this.userId != string.Empty)
                                                    {
                                                        MessageBox.Show("중복된 닉네임이 있습니다.");
                                                        LogoutPacket lgPacket = new LogoutPacket();
                                                        lgPacket.userId = this.userId;
                                                        lgPacket.wrongOut = true;

                                                        this.sendBuffer = Packet.Serialize(lgPacket).ToList();
                                                        await this.Send().ConfigureAwait(false);

                                                        this.stream.Close();
                                                        this.client.Close();
                                                        this.thread.Abort();
                                                        this.Close();
                                                    }
                                                }
                                                else
                                                    break;
                                            }
                                            else
                                            {
                                                is_same = true;
                                            }
                                        }
                                    }


                                    break;
                                }
                            case PacketType.LOGOUT_RESPONSE:
                                {
                                    LogoutResponsePacket lgResPacket = (LogoutResponsePacket)Packet.DeSerialize(buffer);

                                    if (lgResPacket.isOK && lgResPacket.users != null)
                                    {
                                        string logoutUserNickname = this.users[lgResPacket.logoutUserId];
                                        this.users = lgResPacket.users;

                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Key} : {user.Value}{Environment.NewLine}");
                                            }
                                            textBox1.SelectionColor = Color.Black;
                                            textBox1.SelectionAlignment = HorizontalAlignment.Center;
                                            if(!lgResPacket.wrongOut)
                                                textBox1.AppendText($"{logoutUserNickname}님이 채팅방을 나갔습니다.{Environment.NewLine}");
                                        }));
                                    }

                                    break;
                                }
                            case PacketType.TEXT_CHAT_RESPONSE:
                                {
                                    //text chat response packet received
                                    TextChatResponsePacket tPacket = (TextChatResponsePacket)Packet.DeSerialize(buffer);

                                    //user list update
                                    this.users = tPacket.users;

                                    if (this.users.Count > 0 && tPacket.isOk)
                                    {
                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Key} : {user.Value}{Environment.NewLine}");
                                            }
                                        }));
                                    }

                                    if (tPacket.textBody != string.Empty && tPacket.isOk)
                                    {
                                        string sendUser = this.users[tPacket.userId];
                                        int colorNum = 0;
                                        for (int i = 0; i < sendUser.Length; i++)
                                        {
                                            colorNum += (int)sendUser[i];
                                        }

                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            switch (colorNum % 21)
                                            {
                                                case 1:
                                                    textBox1.SelectionColor = Color.Red;
                                                    break;

                                                case 2:
                                                    textBox1.SelectionColor = Color.Orange;
                                                    break;

                                                case 3:
                                                    textBox1.SelectionColor = Color.Blue;
                                                    break;

                                                case 4:
                                                    textBox1.SelectionColor = Color.Purple;
                                                    break;

                                                case 5:
                                                    textBox1.SelectionColor = Color.Yellow;
                                                    break;

                                                case 6:
                                                    textBox1.SelectionColor = Color.Green;
                                                    break;

                                                case 7:
                                                    textBox1.SelectionColor = Color.Ivory;
                                                    break;

                                                case 8:
                                                    textBox1.SelectionColor = Color.Pink;
                                                    break;

                                                case 9:
                                                    textBox1.SelectionColor = Color.Gray;
                                                    break;

                                                case 10:
                                                    textBox1.SelectionColor = Color.Gold;
                                                    break;

                                                case 11:
                                                    textBox1.SelectionColor = Color.CadetBlue;
                                                    break;

                                                case 12:
                                                    textBox1.SelectionColor = Color.MediumVioletRed;
                                                    break;

                                                case 13:
                                                    textBox1.SelectionColor = Color.BurlyWood;
                                                    break;

                                                case 14:
                                                    textBox1.SelectionColor = Color.Lime;
                                                    break;

                                                case 15:
                                                    textBox1.SelectionColor = Color.Aqua;
                                                    break;

                                                case 16:
                                                    textBox1.SelectionColor = Color.Violet;
                                                    break;

                                                case 17:
                                                    textBox1.SelectionColor = Color.LightCoral;
                                                    break;

                                                case 18:
                                                    textBox1.SelectionColor = Color.Tan;
                                                    break;

                                                case 19:
                                                    textBox1.SelectionColor = Color.Khaki;
                                                    break;

                                                case 20:
                                                    textBox1.SelectionColor = Color.MediumAquamarine;
                                                    break;

                                                default:
                                                    textBox1.SelectionColor = Color.Brown;
                                                    break;
                                            }
                                            if (sendUser == this.myNickName)
                                            {
                                                textBox1.SelectionAlignment = HorizontalAlignment.Right;
                                                textBox1.AppendText($"{tPacket.textBody} : 나 [{DateTime.Now.ToShortTimeString()}]{System.Environment.NewLine}");
                                                textBox2.Text = string.Empty;
                                                textBox1.SelectionAlignment = HorizontalAlignment.Left;
                                            }
                                            else
                                            {
                                                textBox1.AppendText($"[{DateTime.Now.ToShortTimeString()}] {sendUser} : {tPacket.textBody}{System.Environment.NewLine}");
                                                textBox2.Text = string.Empty;
                                            }
                                        }));
                                    }

                                    break;
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " RUN");
                    this.thread.Abort();
                    Application.Exit();
                }
            }
        }

        private string generateUniqueID(int _characterLength = 11)
        {
            StringBuilder _builder = new StringBuilder();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(_characterLength)
                .ToList().ForEach(e => _builder.Append(e));
            return _builder.ToString();
        }

        async private void Form1_Load(object sender, EventArgs e)
        {
            Form2 mdF2 = new Form2();
            DialogResult F2Result = mdF2.ShowDialog();
            if (F2Result == DialogResult.OK)
            {
                //로그인에 성공하여 관련 정보를 서버에 전달
                this.ip = mdF2.ipTextBox.Text;
                this.myNickName = mdF2.nickNameTextBox.Text;
                this.userId = generateUniqueID();
                this.label1.Text = $"{this.myNickName}님의 채팅";

                this.client = new TcpClient();

                try
                {
                    this.client.Connect(this.ip, 18700);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Exit();
                    return;
                }

                this.isConnect = true;
                this.stream = this.client.GetStream();

                if (this.isConnect)
                {
                    await this.SendLoginPacket().ConfigureAwait(false);
                    this.thread = new Thread(() =>
                     {
                         RUN();
                     });
                    thread.Start();
                }
            }
            else if (F2Result == DialogResult.Cancel)
            {
                this.thread.Abort();
                Application.Exit();
            }
        }

        async private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                if (System.IO.File.Exists(filePath))
                {
                    //check file size
                    if (new FileInfo(filePath).Length >= BUFFER_SIZE)
                    {
                        MessageBox.Show("파일 사이즈가 너무 커서 전송할 수 없습니다.");
                        return;
                    }
                    //send file packet
                    string[] strs = filePath.Split('.');
                    string format = strs[strs.Length - 1];

                    FilePacket fPacket = new FilePacket();
                    FileStream fStream = System.IO.File.OpenRead(filePath);

                    fPacket.fileName = filePath;
                    fPacket.sendUserId = this.userId;
                    fPacket.fileFormat = format;

                    byte[] fileBuffer = new byte[fStream.Length];
                    await fStream.ReadAsync(fileBuffer, 0, fileBuffer.Length).ConfigureAwait(false);

                    fPacket.fileBody = fileBuffer;
                    fStream.Close();

                    this.sendBuffer = Packet.Serialize(fPacket).ToList();
                    await this.Send().ConfigureAwait(false);
                }
            }
        }

        async private void button1_Click(object sender, EventArgs e)
        {
            //메세지 전송 버튼 클릭 이벤트

            string textBody = textBox2.Text;
            if (textBody == string.Empty)
            {
                return;
            }

            this.textChatPacket = new TextChatPacket(this.userId, textBody);
            this.sendBuffer = Packet.Serialize(this.textChatPacket).ToList();
            await this.Send().ConfigureAwait(false);
            this.Invoke(new MethodInvoker(() =>
            {
                textBox2.Text = string.Empty;
            }));
        }

        private void tb39840_TextChanged(object sender, EventArgs e)
        {
        }

        async private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //클라이언트가  채팅방을 나갈때

            if (this.userId != string.Empty)
            {
                LogoutPacket lgPacket = new LogoutPacket();
                lgPacket.userId = this.userId;

                this.sendBuffer = Packet.Serialize(lgPacket).ToList();
                await this.Send().ConfigureAwait(false);

                this.stream.Close();
                this.client.Close();
                this.thread.Abort();
                this.Close();
            }
        }

        async private void 메뉴1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.items = new string[users.Count];
            int num = 0;
            foreach (KeyValuePair<string, string> user in this.users)
            {
                f4.items[num] = user.Value;
                num++;
            }
            DialogResult dr = f4.ShowDialog();
            if(dr == DialogResult.OK)
            {
                string textBody = "[뽑기] " + f4.answer + "님이 당첨되셨습니다!";

                this.textChatPacket = new TextChatPacket(this.userId, textBody);
                this.sendBuffer = Packet.Serialize(this.textChatPacket).ToList();
                await this.Send().ConfigureAwait(false);
            }
        }
    }
}