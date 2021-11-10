using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static byte[] Vector3ToBytes(Vector3 v)
    {
        return new byte[3] { (byte)v.x, (byte)v.y, (byte)v.z };
    }

    public static Vector3 BytesToVector3(byte[] b)
    {
        return new Vector3(b[0], b[1], b[2]);
    }
}