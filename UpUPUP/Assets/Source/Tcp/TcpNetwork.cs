using System;
using System.Net.Sockets;
using UnityEngine;

public class TcpNetwork : MonoBehaviour
{
    public NetworkStream stream = default(NetworkStream);

    public byte[] bytes = new byte[1024];

    public Int32 port = 13000;

    public string ip = "127.0.0.1";

    public static TcpNetwork me;

    public int Player = -1;

    private void Awake()
    {
        if (!me)
        {
            me = this;
        }
    }

    void Start()
    {
        TcpClient server = new TcpClient();
        server.BeginConnect(ip, port, (arg) =>
        {
            Debug.Log("are we connected =  " + server.Connected);
            stream = server.GetStream();

            stream.BeginRead(bytes, 0, bytes.Length, Read, null);
        }, World.me.GetPackage());
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            Broadcast(new Package()
            {
                Action = (int)Package.Actions.CREATE,
                Contains = new Packages.Create()
                {
                    Width = World.me.Width,
                    Height = World.me.Height,
                    Depth = World.me.Height,
                    Map = World.me.Map
                }.Serialize()
            });
        }

        if (Input.GetKeyUp(KeyCode.F2))
        {
            Broadcast(new Package()
            {
                Action = (int)Package.Actions.JOIN,
                Contains = new Packages.Join()
                {
                    Id = 0,
                    Position = 0,
                    Player = TcpNetwork.me.Player
                }.Serialize()
            });
        }


        if (Input.GetKeyUp(KeyCode.F3))
        {
            Broadcast(new Package()
            {
                Action = (int)Package.Actions.LEAVE,
                Contains = new Packages.Leave()
                {
                    Player = TcpNetwork.me.Player
                }.Serialize()
            });
        }

        if (Input.GetKeyUp(KeyCode.F4))
        {
            Broadcast(new Package()
            {
                Action = (int)Package.Actions.MOVE,
                Contains = new Packages.Move()
                {
                    Player = TcpNetwork.me.Player,
                    Time = 2,
                    Position = 1,
                    Current = 1
                }.Serialize()
            });
        }

        if (Input.GetKeyUp(KeyCode.F5))
        {
            Broadcast(new Package()
            {
                Action = -1,
                Contains = new byte[] { 0 }
            });
        }
    }

    public void Incoming(Package package)
    {
        if (package.Action == (int)Package.Actions.CONNECTED)
        {
            Packages.Connected connected = Packages.Connected.Desserialize(package.Contains);
            TcpNetwork.me.Player = connected.Player;
        }
    }

    public void Read(IAsyncResult arg)
    {
        int length = stream.EndRead(arg);
        if (length > 0)
        {
            Package package = Package.Desserialize(bytes);

            TcpNetwork.me.Incoming(package);
            World.me.package = package;

            bytes = new byte[1024];
            stream.BeginRead(bytes, 0, bytes.Length, Read, null);
        }
        else if (length == 0)
        {
            Debug.Log("server down");
        }
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
}
