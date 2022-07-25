using System;
using UnityEngine;

public interface IGameplayInput
{
     public event Action<Vector2> StartTouch;
    public void NotifyTouch(Vector2 pos);

     public event Action<Direction> Swap;
    public void NotifySwap(Direction dir);
     public event Action EndTouch;
    public void NotifyEndTouch();

    Vector2 initial_touch { get; set; }
    float swap_distance { get;set; }
}
