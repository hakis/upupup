using System;
using System.IO;

namespace Packages
{
    class Leave
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

        public static Leave Desserialize(byte[] data)
        {
            Leave leave = new Leave();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    leave.Player = reader.ReadInt32();
                }
            }
            return leave;
        }
    }
}