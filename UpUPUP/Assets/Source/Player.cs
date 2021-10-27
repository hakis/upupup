using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyFrames keyframes;

    private static int ids = 0;
    public int id = 0;

    public bool controlling = false;

    public Tile previous = null;
    // Start is called before the first frame update
    void Start()
    {
        id = ids;
        ids++;
        name = $"Player:{id}";

        gameObject.AddComponent(typeof(KeyFrames));

        keyframes = Object.FindObjectOfType<KeyFrames>();
        transform.position = previous.transform.position + new Vector3(0f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveTo(Tile tile)
    {
        if (previous == null || keyframes.all.Count > 0f)
        {
            Debug.Log("running keyframes");
            return;
        }

        Vector3[] points = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(0,2,0),
        };

        Vector3 direction = new Vector3();
        foreach (Vector3 point in points)
        {
            Vector3 to = tile.transform.position + point;
            Vector3 from = transform.position;
            Vector3 between = to - from;

            if (between.magnitude <= 1f)
            {
                int updown = point.y == 0f ? 1 : point.y == 1 ? 0 : -1;
                direction = new Vector3(0, updown, 0);

                Vector3 moveTo = tile.transform.position + new Vector3(0, 1, 0);
                if (World.me.findTile(moveTo) != null)
                {
                    return;
                }

                keyframes.Play(new KeyFrames.KeyFrame(KeyFrames.KeyFrame.Type.MOVE, transform.position, moveTo, 0.5f));

                //todo cleanup
                Vector3 v1 = transform.position;
                Vector3 v2 = moveTo;

                v1.y = 0f;
                v2.y = 0f;

                keyframes.Play(new KeyFrames.KeyFrame(
                    KeyFrames.KeyFrame.Type.ROTATE,
                    transform.forward,
                    v1 - v2, 0.2f));

                if (previous.move(new int[,] { { 0, updown, 0 } }))
                {
                    previous.transform.position += direction;
                }

                previous = tile;
                return;
            }
        }
    }
}
