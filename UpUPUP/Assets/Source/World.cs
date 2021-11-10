using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Instantiate(Resources.Load("Prefabs/World", typeof(GameObject)) as GameObject);
    }

    public int Id;

    public bool master;

    public static World me;

    public Player player;

    public Client client;

    public Package package;

    public int Height, Width, Depth;


    public int[] map;

    private void Awake()
    {
        if (!me)
        {
            me = this;
        }
    }

    public Package GetPackage()
    {
        return new Package()
        {
            Action = (int)Package.Actions.WORLD,
            Contains = new Packages.World()
            {
                Id = Id,
                Width = Width,
                Height = Height,
                Depth = Depth,
                Map = toBytes(),

            }.Serialize()
        };
    }

    public int to1D(Vector3 v)
    {
        int index = ((int)v.z * Width * Height) + ((int)v.y * Width) + (int)v.x;
        return index > map.Length ? 0 : index;
    }

    public int[] to3D(int index)
    {
        int z = index / (Width * Height);
        index -= (z * Width * Height);
        int y = index / Width;
        int x = index % Width;
        return new int[] { x, y, z };

    }

    public byte[] toBytes()
    {
        byte[] itmes = new byte[map.Length];
        for (int index = 0; index < map.Length; index++)
        {
            itmes[index] = (byte)map[index];
        }

        return itmes;
    }

    public int GetMapId(Vector3 pos)
    {
        return map[to1D(pos)];
    }

    public void SetMapId(int tile, Vector3 pos)
    {
        map[to1D(pos)] = tile;
    }

    void Start()
    {
        Player[] players = Object.FindObjectsOfType<Player>();
        player = null;
        foreach (Player find in players)
        {
            if (find.controlling)
                player = find;
        }

        map = new int[Height * Width * Depth];
        client = GetComponent<Client>();
    }

    public void Read(Package package)
    {
        switch (package.Action)
        {
            case (int)Package.Actions.WORLD:
                Packages.World world = Packages.World.Desserialize(package.Contains);
                break;
            case (int)Package.Actions.MOVE:
                MoveTo(package);
                break;
        }
    }

    void Update()
    {
        if (package != null)
        {
            Read(package);
            package = null;
        }
    }

    public void MoveTo(Package package)
    {
        Packages.Move move = Packages.Move.Desserialize(package.Contains);
        Player[] players = Object.FindObjectsOfType<Player>();
        foreach (Player player in players)
        {

            if (player.Id == move.Player)
            {
                Debug.Log(package.Action);
                byte[] from = move.Current;
                byte[] to = move.Position;

                player.transform.position = Helper.BytesToVector3(to) + new Vector3(0f, 1f, 0f);
                //player.MoveTo(from, to, move.Time);
            }
        }

        Debug.Log(move.Time);

        /*Player player = GameObject.Find("Player:" + package.Id).GetComponent<Player>();
        Tile tile = GameObject.Find(package.Msg).GetComponent<Tile>();
        if (player == null || tile == null)
        {
            return;
        }

        player.MoveTo(tile, package.Time);*/
    }

    public void Broadcast(Package package)
    {
        client.Broadcast(package);
    }

    public Tile findTile(Vector3 positon)
    {
        Tile[] tiles = Object.FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            Vector3 v1 = positon;
            Vector3 v2 = tile.transform.position;

            if ((v2 - v1).magnitude < 1)
            {
                return tile;
            }
        }

        return null;
    }

    void OnDrawGizmos()
    {
        for (int index = 0; index < map.Length; index++)
        {
            int[] p = to3D(index);
            Gizmos.color = map[index] == 0 ? Color.green : Color.red;
            Gizmos.DrawSphere(new Vector3(p[0], p[1], p[2]), 0.1f);
        }

    }
}
