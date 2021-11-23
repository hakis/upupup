using System;
using System.IO;

namespace Packages
{
    class Player
    {
        public int Id;

        public int Position;

        public int Current;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Id);
                    writer.Write(Position);
                    writer.Write(Current);
                }
                return m.ToArray();
            }
        }

        public static Player Desserialize(byte[] data)
        {
            Player player = new Player();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    player.Id = reader.ReadInt32();
                    player.Position = reader.ReadInt32();
                    player.Current = reader.ReadInt32();
                }
            }
            return player;
        }
    }
}