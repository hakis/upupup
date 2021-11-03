using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static int ids = 1;

    public int id;
    // Start is called before the first frame update
    void Start()
    {
        id = ids;
        name = $"Tile:{id}";

        ids++;

        World.me.SetMapId(id, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        Player player = World.me.player;
        Vector3 v1 = transform.position - player.current.transform.position;

        if (v1.magnitude < 1.5f && player.current != this)
        {
            Vector3 dir = new Vector3(v1.x, 0f, v1.z);
            if ((Mathf.Abs(dir.x) != Mathf.Abs(dir.z)))
            {
                Vector3 vPlayer = transform.position + new Vector3(0f, 1f, 0f);
                Vector3 vTile = player.current.transform.position;
                vTile.y = transform.position.y;

                bool bTile = World.me.GetMapId(vTile) == 0 ? true : false;
                bool bPlayer = World.me.GetMapId(vPlayer) == 0 ? true : false;

                /*
                Tile[] tiles = Object.FindObjectsOfType<Tile>();
                foreach (Tile tile in tiles)
                {
                    if (tile.transform.position == vPlayer)
                        bPlayer = false;

                    if (tile != player.current && tile.transform.position == vTile)
                        bTile = false;
                }
                */

                if (bPlayer && bTile)
                {
                    World.me.SetMapId(0, player.current.transform.position);
                    player.current.transform.position = vTile;
                    World.me.SetMapId(player.current.id, vTile);
                }
                if (bPlayer)
                {
                    player.transform.position = vPlayer;
                    player.current = this;
                }
            }
        }

        /*
        Vector3 v1 = transform.position - player.transform.position;

        if (v1.magnitude < 1.5f && player.current != this)
        {
            Vector3 movePlayer = transform.position + new Vector3(0f, 1f, 0f);
            Vector3 moveTile = player.current.transform.position + new Vector3(0f, 1f, 0f);
            Tile[] tiles = Object.FindObjectsOfType<Tile>();
            foreach (Tile tile in tiles)
            {
                if (tile.transform.position == movePlayer)
                    movePlayer = Vector3.zero;

                if (tile != player.current && tile.transform.position == moveTile)
                    moveTile = Vector3.zero;
            }

            if (movePlayer != Vector3.zero && moveTile != Vector3.zero && movePlayer.y > moveTile.y)
                player.current.transform.position = moveTile;

            if (movePlayer != Vector3.zero)
                player.MoveTo(this);
        }
        */

        /*
        Player player = World.me.player;
        if (!player || player.previous == this)
        {
            return;
        }


        World.me.client.Broadcast(new Package()
        {
            Id = player.id,
            Action = "PlayerMove",
            Msg = "Tile:" + id,
            Duration = 2f
        });
        */

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
                        //Debug.Log((v2 - v1).magnitude);
                        //Debug.Log(tile);
                        return false;
                    }
                }
            }
        }

        return true;
    }
}