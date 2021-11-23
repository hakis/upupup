using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpWorld : MonoBehaviour
{
    public List<Player> players = new List<Player>();

    public Package package;

    public void Incoming(Package package)
    {
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

        if (package.Action == (int)Package.Actions.PLAYER)
        {
            Packages.Player player = Packages.Player.Desserialize(package.Contains);
            AddPlayer(player.Id, player.Position);
        }

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
            foreach (Player player in players)
            {
                player.Incomgin(package);
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
        players.Add(toadd.GetComponent<Player>());
    }

    public Player HasPlayer(int id)
    {
        foreach (Player player in players)
            if (player.Id == id)
                return player;
        return null;
    }
}
