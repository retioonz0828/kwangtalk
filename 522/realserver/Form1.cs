using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using static PacketLibrary.Class1;

namespace TCP_UIServer_1
{
    public partial class Form1 : Form
    {
        private readonly int PORT = 18700;
        private readonly int BUFFER_SIZE = 1024 * 100;
        private Dictionary<string, string> users = new Dictionary<string, string>();
        private Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

        public Form1()
        {
            InitializeComponent();
            AsyncServer();
        }

        async public Task AsyncServer()
        {
            //서버 초기화 함수. 서버를 시작하고 클라이언트가 들어오길 기다리며, 클라이언트가 들어오면 새로운 Task를 만들어 클라이언트당 Task 하나씩 부여함.
            TcpListener server = new TcpListener(this.PORT);
            server.Start();

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync().ConfigureAwait(false);

                Task.Factory.StartNew(AsyncTcpProcess, client);
            }
        }

        async public Task BroadcastText(string text, string exceptUserId)
        {
            //텍스트챗리스폰스 패킷을 접속하고 있는 유저 모두에게 보내는 함수.
            foreach (KeyValuePair<string, TcpClient> client in this.clients)
            {
                NetworkStream stream = client.Value.GetStream();
                TextChatResponsePacket tPacket = new TextChatResponsePacket();
                tPacket.isOk = true;
                tPacket.users = this.users;
                tPacket.textBody = text;
                tPacket.userId = exceptUserId;

                byte[] buffer = Packet.Serialize(tPacket);

                await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }
        }

        async public Task BroadcastLoginResponse(LoginResponsePacket lPacket)
        {
            //로그인 리스폰스 패킷을 접속한 유저 모두에게 보내는 함수.
            foreach (KeyValuePair<string, TcpClient> client in this.clients)
            {
                NetworkStream stream = client.Value.GetStream();
                byte[] buffer = Packet.Serialize(lPacket);

                await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }
        }

        async public Task BroadcastFileResponse(FileResponsePacket fRPacket, string exceptUserId)
        {
            //파일 리스폰스 패킷을 보낸 클라이언트를 제외하고 나머지 모두에게 보내는 함수.
            if (fRPacket != null && exceptUserId != string.Empty)
            {
                foreach (KeyValuePair<string, TcpClient> client in this.clients)
                {
                    if (client.Key == exceptUserId)
                    {
                        continue;
                    }

                    NetworkStream stream = client.Value.GetStream();
                    byte[] buffer = Packet.Serialize(fRPacket);

                    await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                }
            }
        }

        async public Task BroadcastLogoutResponsePacket(LogoutResponsePacket lgResPacket)
        {
            //로그아웃 리스폰스 패킷을 보내는 함수.
            foreach (KeyValuePair<string, TcpClient> client in this.clients)
            {
                NetworkStream stream = client.Value.GetStream();

                byte[] buffer = Packet.Serialize(lgResPacket);

                await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }
        }

        async public void AsyncTcpProcess(object o)
        {
            //클라이언트를 받아서 동작하는 함수. 클라이언트가 나가기 전까지 접속을 유지하며 클라이언트가 보낸 패킷을 받아 처리함.
            TcpClient client = (TcpClient)o;
            NetworkStream stream = client.GetStream();

            byte[] buffer;
            int nRead;

            while (client.Connected)
            {
                try
                {
                    buffer = new byte[this.BUFFER_SIZE];

                    nRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

                    Packet packet = (Packet)Packet.DeSerialize(buffer);

                    switch (packet.type)
                    {
                        case PacketType.LOGIN:
                            {
                                //unpack login packet
                                LoginPacket loginPacket = (LoginPacket)Packet.DeSerialize(buffer);
                                if (loginPacket.userId != string.Empty && loginPacket.nickName != string.Empty)
                                {
                                    this.users.Add(loginPacket.userId, loginPacket.nickName);
                                    this.clients.Add(loginPacket.userId, client);

                                    this.Invoke(new MethodInvoker(() =>
                                    {
                                        this.tbBoard.AppendText($"{loginPacket.nickName}님이 입장하셨습니다.{Environment.NewLine}");
                                    }));
                                }

                                //send loginResponse packet to all
                                LoginResponsePacket lPacket = new LoginResponsePacket();
                                lPacket.isOk = true;
                                lPacket.users = this.users;

                                await BroadcastLoginResponse(lPacket).ConfigureAwait(false);

                                break;
                            }
                        case PacketType.LOGOUT:
                            {
                                //로그아웃 패킷을 받은 경우라면 users를 업데이트 해주고 로그아웃 리스폰스 패킷을 클라이언트들에게 전송
                                LogoutPacket lgPacket = (LogoutPacket)Packet.DeSerialize(buffer);
                                if (lgPacket.userId != string.Empty)
                                {
                                    string userId = lgPacket.userId;

                                    if (this.users.ContainsKey(userId))
                                    {
                                        this.Invoke(new MethodInvoker(() =>
                                        {
                                            //update server board
                                            tbBoard.AppendText($"{this.users[userId]}님이 채팅방에서 나갔습니다.{Environment.NewLine}");
                                        }));

                                        this.users.Remove(userId);

                                        if (this.clients.ContainsKey(userId))
                                        {
                                            //remove target tcpClient
                                            TcpClient logoutUser = this.clients[userId];
                                            logoutUser.GetStream().Close();
                                            logoutUser.Close();

                                            this.clients.Remove(userId);

                                            LogoutResponsePacket lgResPacket = new LogoutResponsePacket();
                                            lgResPacket.wrongOut = lgPacket.wrongOut;
                                            lgResPacket.users = this.users;
                                            lgResPacket.isOK = true;
                                            lgResPacket.logoutUserId = userId;

                                            await BroadcastLogoutResponsePacket(lgResPacket).ConfigureAwait(false);
                                        }
                                    }
                                }
                                break;
                            }
                        case PacketType.TEXT_CHAT:
                            {
                                //spread chat body to clients
                                TextChatPacket textChatPacket = (TextChatPacket)Packet.DeSerialize(buffer);
                                if (textChatPacket.textBody != string.Empty)
                                {
                                    await BroadcastText(textChatPacket.textBody, textChatPacket.userId).ConfigureAwait(false);
                                }
                                break;
                            }
                        case PacketType.FILE:
                            {
                                //파일 패킷을 받은 경우 해당 파일을 파일 리스폰스 패킷으로 포장하여 보낸 클라이언트 말고 다른 클라이언트들에게 전송
                                FilePacket filePacket = (FilePacket)Packet.DeSerialize(buffer);
                                if (filePacket.fileBody.Length > 0)
                                {
                                    FileResponsePacket fRPacket = new FileResponsePacket();
                                    fRPacket.isOk = true;
                                    fRPacket.sendUserNickName = this.users[filePacket.sendUserId];
                                    fRPacket.fileName = filePacket.fileName;
                                    fRPacket.fileBody = filePacket.fileBody;
                                    fRPacket.fileFormat = filePacket.fileFormat;
                                    fRPacket.users = this.users;

                                    await BroadcastFileResponse(fRPacket, filePacket.sendUserId);
                                }

                                break;
                            }
                    }
                }
                catch
                {
                    return;
                }
            }
        }
    }
}