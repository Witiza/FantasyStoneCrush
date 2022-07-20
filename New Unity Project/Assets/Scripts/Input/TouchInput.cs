using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    RIGHT,
    LEFT,
    UP,
    DOWN
}
public class TouchInput : MonoBehaviour,IGameplayInput
{
    public Vector2 initial_touch { get; set; }
    public float swap_distance { get; set; }

    public event Action<Vector3> StartTouch;
    public void NotifyTouch(Vector3 pos) =>StartTouch?.Invoke(pos);

    public event Action<Direction> Swap;
    public void NotifySwap(Direction dir) =>Swap?.Invoke(dir);    

    public event Action EndTouch;
    public void NotifyEndTouch() =>EndTouch?.Invoke();  

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    initial_touch = touch.position;
                    Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                    pos.z = 0;
                    NotifyTouch(pos);
                    break;
                case TouchPhase.Moved:
                    { 
                        Vector2 diff = touch.position - initial_touch;
                        if ((diff).magnitude > swap_distance)
                        {
                            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                            {
                                if (diff.y > 0)
                                {
                                    NotifySwap(Direction.UP);
                                }
                                else
                                {
                                    NotifySwap(Direction.DOWN);
                                }
                            }
                            else
                            {
                                if (diff.x > 0)
                                {
                                    NotifySwap(Direction.RIGHT);
                                }
                                else
                                {
                                    NotifySwap(Direction.LEFT);
                                }
                            }
                        }
                    }
                    break;
                case TouchPhase.Canceled: case TouchPhase.Ended:
                    NotifyEndTouch();
                    break;
                default:
                    break;
            }
        }
    }
}
