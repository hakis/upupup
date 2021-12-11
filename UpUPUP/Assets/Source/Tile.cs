using System;
using UnityEngine;



public class Tile : MonoBehaviour
{
    void Start()
    {
        World.me.SetMapId(1, transform.position);
        this.name = "Tile";
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        if (World.me.player == null)
            return;

        World.me.player.Move(GetPackage());

        /*
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

                if (bPlayer && bTile)
                {
                    World.me.SetMapId(0, player.current.transform.position);
                    player.current.transform.position = vTile;
                    World.me.SetMapId(player.id, vTile);
                }
                if (bPlayer)
                {
                    player.transform.position = vPlayer;
                    player.current = this;
                }
            }
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

    public Package GetPackage()
    {
        return new Package()
        {
            Action = (int)Package.Actions.MOVE,
            Contains = new Packages.Move()
            {
                Player = TcpNetwork.me.Player,
                Time = TimeSpan.TicksPerSecond / 4,
                //Current = World.me.To1D(World.me.player.current.transform.position),
                Position = World.me.To1D(transform.position)
            }.Serialize()
        };
    }
}