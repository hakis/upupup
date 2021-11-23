using System;
using System.IO;

namespace Packages
{
    class Join
    {
        public int Id;

        public int Position;

        public int Player;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Id);
                    writer.Write(Position);
                    writer.Write(Player);
                }
                return m.ToArray();
            }
        }

        public static Join Desserialize(byte[] data)
        {
            Join join = new Join();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    join.Id = reader.ReadInt32();
                    join.Position = reader.ReadInt32();
                    join.Player = reader.ReadInt32();
                }
            }
            return join;
        }
    }
}