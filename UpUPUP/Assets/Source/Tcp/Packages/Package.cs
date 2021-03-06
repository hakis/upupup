using System;
using System.IO;

public class Package
{
    public enum Actions { MOVE, WORLD, CREATE, JOIN, LEAVE, START, CONNECTED, PLAYER, BLOCK }

    public int Action { get; set; }

    public byte Fail { get; set; }

    public byte[] Contains { get; set; }

    public byte[] Serialize()
    {
        using (MemoryStream m = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(Action);
                writer.Write(Fail);
                writer.Write(Contains);
            }
            return m.ToArray();
        }
    }

    public static Package Desserialize(byte[] data)
    {
        Package result = new Package();
        using (MemoryStream m = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(m))
            {
                result.Action = reader.ReadInt32();
                result.Fail = reader.ReadByte();
                long total = reader.BaseStream.Length - reader.BaseStream.Position;
                result.Contains = reader.ReadBytes((int)total);
            }
        }
        return result;
    }
}