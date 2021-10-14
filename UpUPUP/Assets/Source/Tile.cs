using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        PlayerManager manager = Object.FindObjectOfType<PlayerManager>();
        Player player = manager.player;
        if (!player || player.previous == this)
        {
            return;
        }

        player.MoveTo(this);

        /*Audio audio = Object.FindObjectOfType<Audio>();
        audio.Play("Tile", () =>
        {
        });*/
    }

    public bool move(int[,] points)
    {
        Tile[] tiles = Object.FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile != this)
            {
                for (int i = 0; i < points.GetLength(0); i++)
                {
                    Vector3 v1 = transform.position + new Vector3(points[i, 0], points[i, 1], points[i, 2]);
                    Vector3 v2 = tile.transform.position;

                    if ((v2 - v1).magnitude <= 0.5f)
                    {
                        Debug.Log((v2 - v1).magnitude);
                        Debug.Log(tile);
                        return false;
                    }
                }
            }
        }

        return true;
    }
}