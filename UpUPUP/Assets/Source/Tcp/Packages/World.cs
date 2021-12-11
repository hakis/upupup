using System;
using System.IO;

namespace Packages
{
    public class World
    {
        public int Id;

        public int Width;

        public int Height;

        public int Depth;

        public int[] Map;

        public byte[] Players;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Id);
                    writer.Write(Width);
                    writer.Write(Height);
                    writer.Write(Depth);

                    foreach (int index in Map)
                        writer.Write(index);

                    writer.Write(Players);
                }
                return m.ToArray();
            }
        }

        public static World Desserialize(byte[] data)
        {
            World world = new World();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    world.Id = reader.ReadInt32();
                    world.Width = reader.ReadInt32();
                    world.Height = reader.ReadInt32();
                    world.Depth = reader.ReadInt32();

                    int total = world.Width * world.Height * world.Depth;
                    world.Map = new int[total];

                    for (int i = 0; i < total; i++)
                        world.Map[i] = reader.ReadInt32();

                    total = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                    world.Players = reader.ReadBytes(total);
                }
            }
            return world;
        }
    }
}