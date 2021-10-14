using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class KeyFrame
    {
        public Vector3 from, to;
        public float t, d;
    }
    List<KeyFrame> keyFrames = new List<KeyFrame>();


    public Tile previous = null;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = previous.transform.position + new Vector3(0f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyFrame k in keyFrames)
        {
            if (k.t > 1)
            {
                k.t = 0.0f;
                transform.position = k.to;
                keyFrames.Clear();
                return;
            }
            k.t += Time.deltaTime / k.d;

            Vector3 v1 = transform.position;
            Vector3 v2 = k.to;
            v1.y = 0;
            v2.y = 0;

            Vector3 diraction = v1 - v2;
            Quaternion lookAt = Quaternion.LookRotation(diraction, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, k.t);
            transform.position = Vector3.Lerp(k.from, k.to, k.t);
        }
    }

    public void MoveTo(Tile tile)
    {
        if (previous == null || keyFrames.Count > 0)
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
                        direction = new Vector3(0f, transform.position.y == to.y ? 0f : (transform.position.y < to.y ? 1f : -1f), 0f);

                        if (World.me.findTile(to) != null)
                        {
                            return;
                        }
                    }

                    moved = true;
                    //transform.position = to;
                    KeyFrame k = new KeyFrame();
                    k.to = to;
                    k.from = transform.position;
                    k.d = 0.5f;
                    k.t = 0.0f;
                    keyFrames.Add(k);
                    break;
                }
            }
        }

        if (!moved)
            return;

        //check can the tile move in the diraction
        if (previous.move(new int[,] { { (int)direction.x, (int)direction.y, (int)direction.z } }))
        {
            Debug.Log(previous);
            Debug.Log(direction);
            previous.transform.position += direction;
        }

        previous = tile;
    }
}
