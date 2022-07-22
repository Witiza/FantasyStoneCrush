using System;
using UnityEngine;
public static class BoardEvents 
{
    public static event Action<Vector2,int> TileCreated;
    public static void NotifyCreated(Vector2 pos, int type) => TileCreated?.Invoke(pos,type);

    public static event Action<Vector2> TileDestroyed;
    public static void NotifyDestroyed(Vector2 pos) => TileDestroyed?.Invoke(pos);

    public static event Action<Vector2, Vector2> TileMoved;
    public static void NotifyMoved(Vector2 origin, Vector2 destination) => TileMoved?.Invoke(origin,destination);

    public static event Action<Vector2> TileSelected;
    public static void NotifySelected(Vector2 pos) =>TileSelected?.Invoke(pos);

    public static event Action<Vector2> TileUnselected;
    public static void NotifyUnselected(Vector2 pos) =>TileUnselected?.Invoke(pos);
}
