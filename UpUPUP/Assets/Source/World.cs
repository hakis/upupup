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

    public bool master;

    public static World me;

    public Player player;

    public Client client;

    public Package package = null;

    public int[,,] map = new int[100, 100, 100];

    private void Awake()
    {
        if (!me)
        {
            me = this;
        }
    }

    public int GetMapId(Vector3 pos)
    {
        return map[(int)pos.y, (int)pos.z, (int)pos.x];
    }

    public void SetMapId(int id, Vector3 pos)
    {
        map[(int)pos.y, (int)pos.z, (int)pos.x] = id;
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

        client = GetComponent<Client>();
    }

    public void Read(Package package)
    {
        Debug.Log(package.Action == "PlayerMove" ? "yes" : "no");
        switch (package.Action)
        {
            case "PlayerMove":
                MoveTo(package);
                break;
        }
    }

    public void MoveTo(Package package)
    {
        Player player = GameObject.Find("Player:" + package.Id).GetComponent<Player>();
        Tile tile = GameObject.Find(package.Msg).GetComponent<Tile>();
        if (player == null || tile == null)
        {
            return;
        }

        player.MoveTo(tile, package.Time);
    }

    // Update is called once per frame
    void Update()
    {
        if (package != null)
        {
            Read(package);
            package = null;
        }
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
        // Draw a yellow sphere at the transform's position
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int z = 0; z < map.GetLength(1); z++)
            {
                for (int x = 0; x < map.GetLength(2); x++)
                {
                    Gizmos.color = map[y, z, x] == 0 ? Color.green : Color.red;
                    Gizmos.DrawSphere(Vector3.zero, 1f);
                }
            }
        }
    }
}
