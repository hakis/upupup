using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFrames : MonoBehaviour
{
    [System.Serializable]
    public class KeyFrame
    {
        public enum Type { MOVE, ROTATE };
        public Type type;

        public Vector3 from, to;
        public float t, d;

        public bool done;

        public KeyFrame(Type type, Vector3 from, Vector3 to, float duration)
        {
            this.type = type;
            this.from = from;
            this.to = to;
            this.d = duration;
            this.t = 0;
            done = false;
        }
    }

    public List<KeyFrame> all = new List<KeyFrame>();

    public void Play(KeyFrame keyframe)
    {
        all.Add(keyframe);
    }

    void Update()
    {
        if (all.Count == 0)
            return;

        int total = 0;
        foreach (KeyFrame k in all)
        {
            if (!k.done)
            {
                if (k.t > 1)
                {
                    switch (k.type)
                    {
                        case KeyFrame.Type.MOVE:
                            transform.position = k.to;
                            break;
                        case KeyFrame.Type.ROTATE:
                            transform.rotation = Quaternion.LookRotation(k.to, Vector3.up);
                            break;
                    }

                    k.done = true;
                }
                else
                {
                    k.t += Time.deltaTime / k.d;

                    switch (k.type)
                    {
                        case KeyFrame.Type.MOVE:
                            transform.position = Vector3.Lerp(k.from, k.to, k.t);
                            break;
                        //this is not done , from and to need to be the rotation
                        case KeyFrame.Type.ROTATE:
                            Quaternion from = Quaternion.LookRotation(k.from, Vector3.up);
                            Quaternion to = Quaternion.LookRotation(k.to, Vector3.up);
                            transform.rotation = Quaternion.Lerp(from, to, k.t);
                            break;
                    }
                }
            }
            else
            {
                total += 1;
            }
        }

        if (total == all.Count)
        {
            all.Clear();
        }
    }
}
