using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id = -1;

    public int Position;

    public int Tile = 1;

    public KeyFrames keyframes;

    private Tile current = null;
    // Start is called before the first frame update
    void Start()
    {
        name = "Player" + Id;

        int[] v1 = World.me.To3D(Position);
        current = World.me.FindTile(new Vector3(v1[0], v1[1], v1[2]));

        //transform.position = current.transform.position + new Vector3(0f, 1f, 0f);

        if (TcpNetwork.me != null)
        {
            if (Id == TcpNetwork.me.Player)
            {
                World.me.player = this;
                Camera.main.GetComponent<Orbit>().focus = transform;
            }
        }
    }

    public void Update()
    {
        Vector3 add = Vector3.zero;

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            add = new Vector3(0, 0, 1);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            add = new Vector3(0, 0, -1);
        }


        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            add = new Vector3(1, 0, 0);
        }


        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            add = new Vector3(-1, 0, 0);
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            add = new Vector3(0, 1, 0);
        }

        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            Tile = 1;
        }

        if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            Tile = 2;
        }


        if (add != Vector3.zero)
        {
            int index = World.me.To1D(transform.position + add);
            int width = (int)transform.position.x + (int)add.x;
            int height = (int)transform.position.y + (int)add.y;
            int depth = (int)transform.position.z + (int)add.z;

            if (index > 0)
            {
                if (width >= 0 && width < World.me.Width)
                {
                    if (height >= 0 && height < World.me.Height)
                    {
                        if (depth >= 0 && depth < World.me.Depth)
                        {
                            TcpNetwork.me.Broadcast(new Package()
                            {
                                Action = (int)Package.Actions.BLOCK,
                                Contains = new Packages.Block()
                                {
                                    Tile = Tile,
                                    Index = index,
                                    X = width,
                                    Y = height,
                                    Z = depth,
                                    Fail = 0,
                                    AddRemove = 0
                                }.Serialize()
                            });
                        }
                    }
                }
            }
        }
    }

    public void Incomgin(Package package)
    {
        if (package.Action == (int)Package.Actions.MOVE)
        {
            Packages.Move move = Packages.Move.Desserialize(package.Contains);
            if (move.Player == Id)
            {
                Position = move.Position;

                keyframes.Play(new KeyFrames.KeyFrame(KeyFrames.KeyFrame.Type.MOVE,
                    transform.position, World.me.To3dVector(move.Position), move.Min, move.Max));
            }
        }
    }

    public void Move(Package package)
    {
        if (current == null || keyframes.all.Count > 0f)
        {
            Debug.Log("running keyframes");
            return;
        }

        TcpNetwork.me.Broadcast(package);
    }

    public void MoveTo(byte[] from, byte[] to, long tick)
    {
        /*
        if (current == null || keyframes.all.Count > 0f)
        {
            Debug.Log("running keyframes");
            return;
        }
        */

        /*
                keyframes.Play(new KeyFrames.KeyFrame(
                                KeyFrames.KeyFrame.Type.MOVE,
                                Helper.BytesToVector3(from),
                                Helper.BytesToVector3(to) + new Vector3(0f, 1f, 0f),
                                tick));
                                */
        /*

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

                 keyframes.Play(new KeyFrames.KeyFrame(
                     KeyFrames.KeyFrame.Type.MOVE,
                     transform.position,
                     moveTo, max));

                 //todo cleanup
                 Vector3 v1 = transform.position;
                 Vector3 v2 = moveTo;

                 v1.y = 0f;
                 v2.y = 0f;

                 keyframes.Play(new KeyFrames.KeyFrame(
                     KeyFrames.KeyFrame.Type.ROTATE,
                     transform.forward,
                     v1 - v2, max));

                 if (previous.move(new int[,] { { 0, updown, 0 } }))
                 {
                     previous.transform.position += direction;
                 }

                 previous = tile;
                 return;
             }
         }
         */
    }
}
