using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World me;

    private void Awake()
    {
        if (!me)
        {
            me = this;
        }
    }

    void Start()
    {
        player = Object.FindObjectOfType<Player>();
    }

    public Player player;

    // Update is called once per frame
    void Update()
    {
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
