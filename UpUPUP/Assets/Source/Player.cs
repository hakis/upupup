using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile current = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveTo(Tile tile)
    {
        if (current == null)
        {
            Debug.Log("current cant be null");
            return;
        }

        int[,] points = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        for (int i = 0; i < points.GetLength(0); i++)
        {
            Vector3 p1 = tile.transform.position;
            Vector3 p2 = transform.position;
            Vector3 ground = new Vector3(p1.x, 0.0f, p1.z) - new Vector3(p2.x, 0.0f, p2.z);
            if (ground.magnitude <= 1)
            {
                Vector3 v1 = transform.position + new Vector3(points[i, 0], 0, points[i, 1]) - (tile.transform.position + new Vector3(0, 1, 0));

                if (v1.magnitude <= 1)
                {
                    Vector3 to = tile.transform.position + new Vector3(0, 1.0f, 0);

                    {
                        int move = transform.position.y == to.y ? 0 : (transform.position.y < to.y ? 1 : -1);
                        Vector3 v3 = current.transform.position + new Vector3(0, move, 0);

                        if (!World.me.isTileFree(tile))
                            return;

                        current.transform.position += new Vector3(0, move, 0);
                    }

                    current = tile;
                    transform.position = to;

                    return;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.green;
        int[,] points = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        for (int i = 0; i < points.GetLength(0); i++)
        {
            Gizmos.DrawCube(transform.position + new Vector3(points[i, 0], 0, points[i, 1]), new Vector3(0.1f, 0.1f, 0.1f));
        }
        */
    }
}
