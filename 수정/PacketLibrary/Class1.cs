using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PacketLibrary
{
    [Serializable]
    public class Class1
    {
        public enum PacketType
        {
            INITIAL = 0,
            LOGIN,
            LOGIN_RESPONSE,
            FILE,
            FILE_RESPONSE,
            TEXT_CHAT,
            TEXT_CHAT_RESPONSE
        }

        [Serializable]
        public class Packet
        {
            public PacketType type;

            public Packet()
            {
                this.type = 0;
            }

            public static byte[] Serialize(object o)
            {
                MemoryStream ms = new MemoryStream(1024 * 100);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, o);
                return ms.ToArray();
            }

            public static Object DeSerialize(byte[] bt)
            {
                MemoryStream ms = new MemoryStream(1024 * 100);

                foreach (byte b in bt)
                {
                    ms.WriteByte(b);
                }

                ms.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                Object obj = bf.Deserialize(ms);
                ms.Close();
                return obj;
            }
        }

        [Serializable]
        public class LoginPacket : Packet
        {
            public string ip;
            public string nickName;
            public string userId;

            public LoginPacket(string ip, string nickName, string userId)
            {
                this.type = PacketType.LOGIN;
                this.ip = ip;
                this.nickName = nickName;
                this.userId = userId;
            }
        }

        [Serializable]
        public class LoginResponsePacket : Packet
        {
            public bool isOk;
            public Dictionary<string, string> users;

            public LoginResponsePacket()
            {
                this.type = PacketType.LOGIN_RESPONSE;
                this.isOk = false;
                this.users = new Dictionary<string, string>();
            }
        }

        [Serializable]
        public class TextChatPacket : Packet
        {
            public string textBody;
            public string userId;

            public TextChatPacket(string userId, string textBody)
            {
                this.userId = userId;
                this.textBody = textBody;
                this.type = PacketType.TEXT_CHAT;
            }
        }

        [Serializable]
        public class TextChatResponsePacket : Packet
        {
            public bool isOk;
            public Dictionary<string, string> users;
            public string textBody;
            public string userId;

            public TextChatResponsePacket()
            {
                this.type = PacketType.TEXT_CHAT_RESPONSE;
                this.isOk = false;
                this.users = new Dictionary<string, string>();
                this.textBody = "";
                this.userId = "";
            }
        }

        [Serializable]
        public class FilePacket : Packet
        {
            public string sendUserId;
            public byte[] fileBody;
            public string fileName;
            public string fileFormat;

            public FilePacket()
            {
                this.fileBody = null;
                this.type = PacketType.FILE;
                this.sendUserId = string.Empty;
                this.fileName = string.Empty;
                this.fileFormat = string.Empty;
            }
        }

        [Serializable]
        public class FileResponsePacket : Packet
        {
            public bool isOk;
            public byte[] fileBody;
            public Dictionary<string, string> users;
            public string sendUserNickName;
            public string fileName;
            public string fileFormat;

            public FileResponsePacket()
            {
                this.type = PacketType.FILE_RESPONSE;
                this.isOk = false;
                this.fileBody = null;
                this.users = new Dictionary<string, string>();
                this.sendUserNickName = string.Empty;
                this.fileName = string.Empty;
                this.fileFormat = string.Empty;
            }
        }
    }
}