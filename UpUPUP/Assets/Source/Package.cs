using System;
using System.IO;

public class Package
{
    public int Id { get; set; }
    public string Action { get; set; }
    public string Msg { get; set; }
    public long Time { get; set; }
    public float Duration { get; set; }
    public byte[] Serialize()
    {
        using (MemoryStream m = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(Id);
                writer.Write(Action == null ? "" : Action);
                writer.Write(Msg == null ? "" : Msg);
                writer.Write(Time);
                writer.Write(Duration);
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
                result.Id = reader.ReadInt32();
                result.Action = reader.ReadString();
                result.Msg = reader.ReadString();
                result.Time = reader.ReadInt64();
                result.Duration = reader.ReadSingle();
            }
        }
        return result;
    }
}