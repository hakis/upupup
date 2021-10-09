using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile previous = null;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = previous.transform.position + new Vector3(0f,1f,0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveTo(Tile tile)
    {
        if (previous == null)
        {
            Debug.Log("current cant be null");
            return;
        }

        //check can player move to the tiel?
        int[,] points = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        Vector3 direction = new Vector3();
        bool moved = false;
        for (int i = 0; i < points.GetLength(0); i++)
        {
            Vector3 p1 = tile.transform.position;
            Vector3 p2 = transform.position;
            Vector3 ground = new Vector3(p1.x, 0f, p1.z) - new Vector3(p2.x, 0f, p2.z);

            if (ground.magnitude <= 1f)
            {
                Vector3 v1 = transform.position + new Vector3(points[i, 0], 0, points[i, 1]) - (tile.transform.position + new Vector3(0, 1, 0));

                if (v1.magnitude <= 1f)
                {
                    Vector3 to = tile.transform.position + new Vector3(0, 1.0f, 0);

                    {
                        direction = new Vector3(0f,transform.position.y == to.y ? 0f : (transform.position.y < to.y ? 1f : -1f), 0f);

                        if (World.me.findTile(to) != null) {
                            return;
                        }
                    }

                    moved = true;
                    transform.position = to;
                    break;
                }
            }
        }

        if (!moved)
            return;

        //check can the tile move in the diraction
        if(previous.move(new int[,] { { (int)direction.x, (int)direction.y, (int)direction.z} }))
        {
            previous.transform.position += direction;
        }

        previous = tile;
    }
}
