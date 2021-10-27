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

    public string networkmsg = "";

    private void Awake()
    {
        if (!me)
        {
            me = this;
        }
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

    public void Read(string msg)
    {
        Debug.Log(msg);
        string[] e1 = msg.Split(':');
        if (e1.Length > 2 && player != null)
        {
            MoveTo(e1[1], e1[2]);
        }
    }

    public void MoveTo(string pId, string tId)
    {
        Player player = GameObject.Find("Player:" + pId).GetComponent<Player>();
        Tile tile = GameObject.Find("Tile:" + tId).GetComponent<Tile>();

        if (player == null || tile == null)
        {
            return;
        }
        player.MoveTo(tile);
    }

    // Update is called once per frame
    void Update()
    {
        if (networkmsg != "")
        {
            Read(networkmsg);
            networkmsg = "";
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
}
