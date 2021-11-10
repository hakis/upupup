using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

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

            Package package = (Package)arg.AsyncState;
            Broadcast(package);

            stream.BeginRead(bytes, 0, bytes.Length, Read, World.me);
        }, World.me.GetPackage());
    }

    public void Broadcast(Package package)
    {
        try
        {
            byte[] bytes = package.Serialize();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
        catch (SocketException e)
        {
            bool error = e.NativeErrorCode.Equals(10035);
            Debug.Log("server down");
        }
    }

    public void Read(IAsyncResult arg)
    {
        World world = (World)arg.AsyncState;

        int length = stream.EndRead(arg);
        if (length > 0)
        {
            World.me.package = Package.Desserialize(bytes);
            bytes = new byte[1024];
            stream.BeginRead(bytes, 0, bytes.Length, Read, world);
        }
        else if (length == 0)
        {
            Debug.Log("server down");
        }
    }

}
