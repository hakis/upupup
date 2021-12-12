using System;
using System.IO;

namespace Packages
{
    class Block
    {
        public int Tile;

        public int Index;

        public int X;
        public int Y;
        public int Z;

        public byte AddRemove;

        public byte Fail;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Tile);
                    writer.Write(Index);
                    writer.Write(X);
                    writer.Write(Y);
                    writer.Write(Z);
                    writer.Write(AddRemove);
                    writer.Write(Fail);
                }
                return m.ToArray();
            }
        }

        public static Block Desserialize(byte[] data)
        {
            Block block = new Block();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    block.Tile = reader.ReadInt32();
                    block.Index = reader.ReadInt32();
                    block.X = reader.ReadInt32();
                    block.Y = reader.ReadInt32();
                    block.Z = reader.ReadInt32();
                    block.AddRemove = reader.ReadByte();
                    block.Fail = reader.ReadByte();
                }
            }
            return block;
        }
    }
}