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

        public byte[] Map;

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
                    writer.Write(Map);
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
                    world.Height = reader.ReadInt32();
                    long total = reader.BaseStream.Length - reader.BaseStream.Position;
                    world.Map = reader.ReadBytes((int)total);
                }
            }
            return world;
        }
    }
}