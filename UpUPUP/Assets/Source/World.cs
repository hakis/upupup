using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int width, height, depth;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start the world");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.1f, 0.1f));

        Gizmos.color = Color.red;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.DrawCube(new Vector3(x,y,z), new Vector3(0.7f, 0.7f, 0.7f));
                }
            }
        }
    }
}
