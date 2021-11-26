using System;
using System.IO;

namespace Packages
{
    class Move
    {
        public int Player;
        public long Time;
        public int Position;
        public int Current;

        public long Max;

        public long Min;
        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Player);
                    writer.Write(Time);
                    writer.Write(Max);
                    writer.Write(Min);
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
                    move.Player = reader.ReadInt32();
                    move.Time = reader.ReadInt64();
                    move.Max = reader.ReadInt64();
                    move.Min = reader.ReadInt64();
                    move.Position = reader.ReadInt32();
                    move.Current = reader.ReadInt32();
                }
            }
            return move;
        }
    }
}