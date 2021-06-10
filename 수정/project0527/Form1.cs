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
        private string userId;
        private string ip;

        private string myNickName;

        private NetworkStream stream;
        private TcpClient client;

        private List<byte> sendBuffer;

        private bool isConnect = false;

        public LoginPacket loginPacket;
        public LoginResponsePacket loginResponsePacket;

        public TextChatPacket textChatPacket;

        public Thread thread;

        private readonly int BUFFER_SIZE = 1024 * 10000;

        private Dictionary<string, string> users;

        private bool is_same;

        public Form1()
        {
            InitializeComponent();
        }

        async public Task Send()
        {
            //패킷 전송 함수
            //스트림에 sendBuffer 값을 전송함

            byte[] buffer = this.sendBuffer.ToArray();

            await this.stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            this.stream.Flush();

            this.sendBuffer.Clear();
        }

        async public Task SendLoginPacket()
        {
            //로그인 패킷을 전송하는 함수
            //내부적으로 Send 함수를 사용함.
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
            //쓰레드에서 동작하는 서버와의 통신 관련 함수. 서버와 연결 되어 있는 동안에는 반복문이 계속 실행 되며 서버로부터 값을 스트림에서 읽어온다.

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
                        //읽어올 것이 있으면 패킷으로 deserialize 해서 패킷 안에 들어 있는 패킷 타입에 따라 분기하여 각각의 패킷에 알맞은 행동을 하면 됨.
                        Packet packet = (Packet)Packet.DeSerialize(buffer);

                        switch (packet.type)
                        {
                            case PacketType.FILE_RESPONSE:
                                {
                                    //서버로부터 파일을 받은 경우
                                    FileResponsePacket fRPacket = (FileResponsePacket)Packet.DeSerialize(buffer);

                                    //서버로부터 어떤 유저가 접속하고 있는지 업데이트 받아서 클라이언트 윈폼에 업데이트함.
                                    this.users = fRPacket.users;

                                    if (this.users.Count > 0 && fRPacket.isOk)
                                    {
                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Value}{Environment.NewLine}");
                                            }
                                        }));
                                    }

                                    if (fRPacket.fileFormat != string.Empty && fRPacket.fileBody != null && fRPacket.isOk)
                                    {
                                        //패킷에 문제가 없으면 파일이 서버로부터 정상적으로 받아진 것으로 해당 파일을 saveFileDialog를 사용하여 사용자가 원하는 위치에 파일을 저장할 수 있게끔 해줌.
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

                                    //서버로부터 받은 유저 데이터를 통한 업데이트
                                    this.users = this.loginResponsePacket.users;

                                    if (this.users.Count > 0 && loginResponsePacket.isOk)
                                    {
                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Value}{Environment.NewLine}");
                                            }
                                        }));
                                    }

                                    is_same = false;
                                    foreach (KeyValuePair<string, string> user in this.users)
                                    {
                                        if (user.Value.Equals(this.myNickName)) //유저 리스트를 검사하여 먼저 들어온 사람 중 닉네임이 자신과 같으면서 ID가 다른 유저가 있다면 메시지 박스를 띄우고 닉네임을 고치도록함
                                        {
                                            if (user.Key.Equals(this.userId))
                                            {
                                                if (is_same)
                                                {
                                                    if (this.userId != string.Empty)
                                                    {
                                                        MessageBox.Show("중복된 닉네임이 있습니다.");

                                                        this.Invoke(new MethodInvoker(() =>
                                                        {
                                                            Form2 mdF2 = new Form2();
                                                            mdF2.ipTextBox.Text = this.ip;
                                                            DialogResult F2Result = mdF2.ShowDialog();
                                                            if (F2Result == DialogResult.OK)
                                                            {
                                                                this.myNickName = mdF2.nickNameTextBox.Text;
                                                                this.label1.Text = $"{this.myNickName}님의 채팅";
                                                            }
                                                            else if (F2Result == DialogResult.Cancel)
                                                            {
                                                                this.thread.Abort();
                                                                Application.Exit();
                                                            }
                                                        }));

                                                        NewLoginPacket lgPacket = new NewLoginPacket("", this.myNickName, this.userId);

                                                        this.sendBuffer = Packet.Serialize(lgPacket).ToList();
                                                        await this.Send().ConfigureAwait(false);

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
                                    //로그아웃 리스폰스 패킷을 받은 경우
                                    LogoutResponsePacket lgResPacket = (LogoutResponsePacket)Packet.DeSerialize(buffer);

                                    if (lgResPacket.isOK && lgResPacket.users != null)
                                    {
                                        //문제가 없으면 서버로 부터 받은 유저 데이터를 이용하여 업데이트해줌
                                        string logoutUserNickname = this.users[lgResPacket.logoutUserId];
                                        this.users = lgResPacket.users;

                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Value}{Environment.NewLine}");
                                            }
                                            textBox1.SelectionColor = Color.Black;
                                            textBox1.SelectionAlignment = HorizontalAlignment.Center;
                                            if (!lgResPacket.wrongOut) //ID가 중복되어 나가게 된 경우에는 이 메시지를 띄우지 않도록 함
                                                textBox1.AppendText($"{logoutUserNickname}님이 채팅방을 나갔습니다.{Environment.NewLine}");
                                            textBox1.SelectionAlignment = HorizontalAlignment.Left;
                                        }));
                                    }

                                    break;
                                }
                            case PacketType.TEXT_CHAT_RESPONSE:
                                {
                                    //텍스트 챗 리스폰스 패킷을 받은 경우
                                    TextChatResponsePacket tPacket = (TextChatResponsePacket)Packet.DeSerialize(buffer);

                                    //서버로부터 받은 유저값 업데이트
                                    this.users = tPacket.users;

                                    if (this.users.Count > 0 && tPacket.isOk)
                                    {
                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            tb39840.Clear();
                                            foreach (KeyValuePair<string, string> user in this.users)
                                            {
                                                tb39840.AppendText($"{user.Value}{Environment.NewLine}");
                                            }
                                        }));
                                    }

                                    if (tPacket.textBody != string.Empty && tPacket.isOk)
                                    {
                                        //패킷 데이터에 문제가 없으면 윈폼에 해당 데이터 업데이트
                                        //id문자열을 숫자로 변환, 10로 나눈 나머지값으로 색 부여
                                        string sendUser = this.users[tPacket.userId];
                                        int colorNum = 0;
                                        for (int i = 0; i < sendUser.Length; i++)
                                        {
                                            colorNum += (int)sendUser[i];
                                        }

                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            switch (colorNum % 10)
                                            {
                                                case 1:
                                                    textBox1.SelectionColor = Color.Red;
                                                    break;

                                                case 2:
                                                    textBox1.SelectionColor = Color.DarkOrange;
                                                    break;

                                                case 3:
                                                    textBox1.SelectionColor = Color.Blue;
                                                    break;

                                                case 4:
                                                    textBox1.SelectionColor = Color.Purple;
                                                    break;

                                                case 5:
                                                    textBox1.SelectionColor = Color.DarkGreen;
                                                    break;

                                                case 6:
                                                    textBox1.SelectionColor = Color.DarkBlue;
                                                    break;

                                                case 7:
                                                    textBox1.SelectionColor = Color.DarkRed;
                                                    break;

                                                case 8:
                                                    textBox1.SelectionColor = Color.Indigo;
                                                    break;

                                                case 9:
                                                    textBox1.SelectionColor = Color.Black;
                                                    break;

                                                default:
                                                    textBox1.SelectionColor = Color.Brown;
                                                    break;
                                            }
                                            //상대방이 보낸 메세지는 왼쪽으로, 내가 보낸 메세지는 오른쪽으로 출력
                                            if(tPacket.textBody[0] == '[' && tPacket.textBody[3] == ']') {
                                                textBox1.SelectionAlignment = HorizontalAlignment.Center;
                                                textBox1.SelectionColor = Color.Black;
                                                textBox1.AppendText($"{tPacket.textBody}{System.Environment.NewLine}");
                                                textBox1.SelectionAlignment = HorizontalAlignment.Left;
                                            }
                                            else if (sendUser == this.myNickName)
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
            //유저 아이디를 랜덤적으로 생성하는 함수
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
            //해당 클라이언트 윈폼이 불러지면 동작하는 함수.

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
                    //로그인 패킷을 전송하고, 서버로 부터 데이터를 송신받기 위해 쓰레드를 이용하여 대기함.
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
            //파일 전송 버튼을 누른 경우.
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

            //텍스트 챗 패킷 전송
            this.textChatPacket = new TextChatPacket(this.userId, textBody);
            this.sendBuffer = Packet.Serialize(this.textChatPacket).ToList();
            await this.Send().ConfigureAwait(false);
            this.Invoke(new MethodInvoker(() =>
            {
                textBox2.Text = string.Empty;
            }));
        }

        async private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //클라이언트가  채팅방을 나갈때 전부 정리하고 종료

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
            if (dr == DialogResult.OK)
            {
                string textBody = "[뽑기] " + f4.answer + "님이 당첨되셨습니다!";

                this.textChatPacket = new TextChatPacket(this.userId, textBody);
                this.sendBuffer = Packet.Serialize(this.textChatPacket).ToList();
                await this.Send().ConfigureAwait(false);
            }
        }

        async private void 공지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();

            DialogResult dr = f5.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if(f5.str != null)
                {
                    string textBody = "[공지] " + f5.str;

                    this.textChatPacket = new TextChatPacket(this.userId, textBody);
                    this.sendBuffer = Packet.Serialize(this.textChatPacket).ToList();
                    await this.Send().ConfigureAwait(false);
                }
            }
        }
    }
}