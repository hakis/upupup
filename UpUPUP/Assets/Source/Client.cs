using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    public NetworkStream stream = default(NetworkStream);

    public byte[] bytes = new byte[1024];

    public Int32 port = 13000;

    public string ip = "127.0.0.1";

    void Start()
    {
        TcpClient server = new TcpClient();
        server.BeginConnect(ip, port, (arg) =>
        {
            Debug.Log("are we connected =  " + server.Connected);
            stream = server.GetStream();
            stream.BeginRead(bytes, 0, bytes.Length, Read, null);
        }, null);
    }

    public void Read(IAsyncResult arg)
    {
        int length = stream.EndRead(arg);
        if (length > 0)
        {
            string msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            Debug.Log(msg);
            World.me.networkmsg = msg;
            bytes = new byte[1024];
            stream.BeginRead(bytes, 0, bytes.Length, Read, null);
        }
        else if (length == 0)
        {
            Debug.Log("server down");
        }
    }
    public void Broadcast(string msg)
    {
        try
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
        catch (SocketException e)
        {
            bool error = e.NativeErrorCode.Equals(10035);
            Debug.Log("server down");
        }
    }
}
