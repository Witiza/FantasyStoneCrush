using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface IGameplayInput
{
     public event Action<Vector2> StartTouch;
    public void NotifyTouch(Vector2 pos);

    public event Action<Direction> SwapTouch;
    public void NotifySwapTouch(Direction dir);
     public event Action EndTouch;
    public void NotifyEndTouch();
    //Weird shit
    public System.Collections.IEnumerator CoroutineTurnWait();

    Vector2 _initialTouch { get; set; }
    float SwapDistance { get;set; }
}
