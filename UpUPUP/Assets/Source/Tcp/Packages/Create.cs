using System;
using System.IO;

namespace Packages
{
    public class Create
    {
        public int Width;

        public int Height;

        public int Depth;

        public int[] Map;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Width);
                    writer.Write(Height);
                    writer.Write(Depth);

                    foreach (int tile in Map)
                        writer.Write(tile);
                }
                return m.ToArray();
            }
        }

        public static Create Desserialize(byte[] data)
        {
            Create create = new Create();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    create.Width = reader.ReadInt32();
                    create.Height = reader.ReadInt32();
                    create.Depth = reader.ReadInt32();

                    int total = create.Width * create.Height * create.Depth;
                    create.Map = new int[total];
                    for (int i = 0; i < total; i++)
                        create.Map[i] = reader.ReadInt32();
                }
            }
            return create;
        }
    }
}