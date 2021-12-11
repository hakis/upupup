using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : TcpWorld
{
    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Instantiate(Resources.Load("Prefabs/World", typeof(GameObject)) as GameObject);
    }*/

    public static World me;

    public Player player;

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
        Map = new int[Height * Width * Depth];
    }

    void Update()
    {
        if (packages.Count > 0)
        {
            Package package = packages[0];
            Incoming(package);
            packages.RemoveAt(0);
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
                Players = new Packages.Player().Serialize()
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