using System;
using System.IO;

namespace Packages
{
    class Move
    {
        public int Player;
        public long Time;
        public byte[] Position;
        public byte[] Current;
        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Player);
                    writer.Write(Time);
                    writer.Write(Position);
                    writer.Write(Current);
                }
                return m.ToArray();
            }
        }

        public static Move Desserialize(byte[] data)
        {
            Move move = new Move();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    move.Player = reader.ReadByte();
                    move.Time = reader.ReadInt64();
                    move.Position = reader.ReadBytes(3);
                    move.Current = reader.ReadBytes(3);
                }
            }
            return move;
        }
    }
}