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
            foreach (KeyValuePair<string, TcpClient> client in this.clients)
            {
                NetworkStream stream = client.Value.GetStream();
                byte[] buffer = Packet.Serialize(lPacket);

                await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }
        }

        async public Task BroadcastFileResponse(FileResponsePacket fRPacket, string exceptUserId)
        {
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

        async public void AsyncTcpProcess(object o)
        {
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
                                }

                                //send loginResponse packet to all
                                LoginResponsePacket lPacket = new LoginResponsePacket();
                                lPacket.isOk = true;
                                lPacket.users = this.users;

                                await BroadcastLoginResponse(lPacket).ConfigureAwait(false);

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