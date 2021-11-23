using System;
using System.IO;

namespace Packages
{
    class Connected
    {
        public int Player;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Player);
                }
                return m.ToArray();
            }
        }

        public static Connected Desserialize(byte[] data)
        {
            Connected connected = new Connected();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    connected.Player = reader.ReadInt32();
                }
            }
            return connected;
        }
    }
}