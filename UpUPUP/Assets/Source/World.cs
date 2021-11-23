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

    public List<Player> players = new List<Player>();

    public Player player;

    public Package package;

    public int Height, Width, Depth;

    public int[] Map;

    private void Awake()
    {
        if (!me)
        {
            me = this;
        }
    }

    void Start()
    {
        /*Player[] players = Object.FindObjectsOfType<Player>();
        player = null;
        foreach (Player find in players)
        {
            if (find.controlling)
                player = find;
        }*/

        Map = new int[Height * Width * Depth];
    }

    public void Incoming(Package package)
    {
        if (package.Action == (int)Package.Actions.JOIN)
        {
            Packages.Join join = Packages.Join.Desserialize(package.Contains);

            if (package.Fail == 0)
            {
                GameObject add = Resources.Load("Prefabs/Player") as GameObject;
                add.GetComponent<Player>().Id = join.Player;
                add.GetComponent<Player>().Position = join.Position;

                Instantiate(add);
            }
            else
            {
                Debug.Log("can't join this world " + join.Id);
            }
        }

        if (package.Action == (int)Package.Actions.LEAVE)
        {
            Packages.Leave leave = Packages.Leave.Desserialize(package.Contains);

            Debug.Log("leave now " + leave.Player);

            GameObject find = GameObject.Find("Player" + leave.Player);
            Destroy(find);
        }

        if (package.Action == (int)Package.Actions.MOVE)
        {
            foreach (Player player in players)
            {
                player.Incomgin(package);
            }
        }
    }

    void Update()
    {
        if (package != null)
        {
            Incoming(package);
            package = null;
        }
    }

    public int To1D(Vector3 v)
    {
        int index = ((int)v.z * Width * Height) + ((int)v.y * Width) + (int)v.x;
        return index > Map.Length ? 0 : index;
    }

    public int[] To3D(int index)
    {
        int z = index / (Width * Height);
        index -= (z * Width * Height);
        int y = index / Width;
        int x = index % Width;
        return new int[] { x, y, z };
    }

    public Vector3 To3dVector(int index)
    {
        int[] position = To3D(index);
        return new Vector3(position[0], position[1], position[2]);
    }

    public int GetMapId(Vector3 pos)
    {
        return Map[To1D(pos)];
    }

    public void SetMapId(int tile, Vector3 pos)
    {
        Map[To1D(pos)] = tile;
    }

    public void MoveTo(Package package)
    {
        Packages.Move move = Packages.Move.Desserialize(package.Contains);
        Player[] players = Object.FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            //if (player.Id == move.Player)
            {
                int[] to = World.me.To3D(move.Position);
                //World.me.player.transform.position = new Vector3(to[0], to[1], to[2]) + new Vector3(0f, 1f, 0f);
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

    public Tile FindTile(Vector3 positon)
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

    public Package GetPackage()
    {
        return new Package()
        {
            Action = (int)Package.Actions.CREATE,
            Contains = new Packages.World()
            {
                Id = -1,
                Width = Width,
                Height = Height,
                Depth = Depth,
                Map = Map,
                Total = 0,
                Players = new int[] { }
            }.Serialize()
        };
    }

    void OnDrawGizmos()
    {
        for (int index = 0; index < Map.Length; index++)
        {
            int[] p = To3D(index);
            Gizmos.color = Map[index] == 0 ? Color.green : Color.red;
            Gizmos.DrawSphere(new Vector3(p[0], p[1], p[2]), 0.1f);
        }

    }
}