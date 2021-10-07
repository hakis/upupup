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

    public bool isTileFree(Tile check)
    {
        int[,] points = new int[,] { { 0, 1, 0 } };
        Tile[] tiles = Object.FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile != check)
            {
                for (int i = 0; i < points.GetLength(0); i++)
                {
                    Vector3 v1 = check.transform.position + new Vector3(points[i, 0], points[i, 1], points[i, 2]);
                    Vector3 v2 = tile.transform.position;

                    if ((v2 - v1).magnitude < 1)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
