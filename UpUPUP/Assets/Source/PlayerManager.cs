using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public Player player;
    public List<Player> players;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player[] players = Object.FindObjectsOfType<Player>();
            foreach (Player player in players)
            {
                if (player != this.player)
                {
                    this.player = player;
                    return;
                }
            }
        }
    }
}
