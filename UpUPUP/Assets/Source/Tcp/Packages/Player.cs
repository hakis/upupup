using System;
using System.IO;

namespace Packages
{
    class Player
    {
        public int Id;

        public int Position;

        public int Total;

        public int[] Players;

        public int[] Positions;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Id);
                    writer.Write(Position);
                    writer.Write(Total);

                    foreach (int player in Players)
                        writer.Write(player);

                    foreach (int position in Positions)
                        writer.Write(position);
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
                    int total = player.Total = reader.ReadInt32();

                    player.Players = new int[total];
                    for (int i = 0; i < total; i++)
                        player.Players[i] = reader.ReadInt32();

                    player.Positions = new int[total];
                    for (int i = 0; i < total; i++)
                        player.Positions[i] = reader.ReadInt32();
                }
            }
            return player;
        }
    }
}