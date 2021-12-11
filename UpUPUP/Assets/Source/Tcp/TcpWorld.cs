using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpWorld : MonoBehaviour
{

    public List<Package> packages = new List<Package>();

    public void Incoming(Package package)
    {
        Debug.Log("World Incoming: " + package.Action);

        if (package.Action == (int)Package.Actions.JOIN)
        {
            Packages.Join join = Packages.Join.Desserialize(package.Contains);
            if (package.Fail == 0)
            {
                AddPlayer(join.Player, join.Position);
            }
            else
            {
                Debug.Log("can't join this world " + join.Id);
            }
        }

        if (package.Action == (int)Package.Actions.WORLD)
        {
            Packages.World world = Packages.World.Desserialize(package.Contains);
            AddBlocks(world);

            Packages.Player player = Packages.Player.Desserialize(world.Players);
            for (int i = 0; i < player.Total; i++)
            {
                AddPlayer(player.Players[i], player.Positions[i]);
            }
        }

        /*
        if (package.Action == (int)Package.Actions.PLAYER)
        {
            Packages.Player player = Packages.Player.Desserialize(package.Contains);

            if (player.Total > 0)
            {
                for (int i = 0; i < player.Total; i++)
                {
                    AddPlayer(player.Players[i], player.Positions[i]);
                }
            }
            else
            {
                AddPlayer(player.Id, player.Position);
            }
        }
        */

        if (package.Action == (int)Package.Actions.LEAVE)
        {
            Packages.Leave leave = Packages.Leave.Desserialize(package.Contains);

            Debug.Log("leave now " + leave.Player);

            GameObject find = GameObject.Find("Player" + leave.Player);
            Destroy(find);
        }

        if (package.Action == (int)Package.Actions.MOVE)
        {
            Packages.Move move = Packages.Move.Desserialize(package.Contains);
            Player[] players = Object.FindObjectsOfType<Player>();
            foreach (Player player in players)
            {
                player.Incomgin(package);
            }
        }
    }

    public void AddBlocks(Packages.World world)
    {
        Debug.Log("time to add blocks");
        for (int i = 0; i < world.Map.Length; i++)
        {
            if (world.Map[i] == 1)
            {
                GameObject add = Resources.Load("Prefabs/Tile1") as GameObject;

                GameObject tile = Instantiate(add);
                tile.transform.position = World.me.To3dVector(i);
            }
        }
    }

    public void AddPlayer(int id, int position)
    {
        Debug.Log($"Add Player {id} At {position}");

        if (HasPlayer(id) != null)
            return;

        GameObject add = Resources.Load("Prefabs/Player") as GameObject;
        add.GetComponent<Player>().Id = id;
        add.GetComponent<Player>().Position = position;

        GameObject toadd = Instantiate(add);
        toadd.transform.position = World.me.To3dVector(position);
    }

    public Player HasPlayer(int id)
    {
        Player[] players = Object.FindObjectsOfType<Player>();
        foreach (Player player in players)
            if (player.Id == id)
                return player;
        return null;
    }
}
