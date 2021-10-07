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
        Player player = Object.FindObjectOfType<Player>();
        if (!player || player.current == this)
        {
            return;
        }

        player.MoveTo(this);
    }
}