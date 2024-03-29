using System;
using UnityEngine;
public static class BoardEvents 
{
    public static event Action<Vector2, TileType> TileCreated;
    public static void NotifyCreated(Vector2 pos, TileType type) => TileCreated?.Invoke(pos,type);

    public static event Action<Vector2, TileType> TileChanged;
    public static void NotifyChanged(Vector2 pos, TileType type) => TileChanged?.Invoke(pos,type);

    public static event Action<Vector2, TileType> TileDestroyed;
    public static void NotifyDestroyed(Vector2 pos, TileType type) => TileDestroyed?.Invoke(pos,type);

    public static event Action<Vector2, Vector2> TileMoved;
    public static void NotifyMoved(Vector2 origin, Vector2 destination) => TileMoved?.Invoke(origin,destination);

    public static event Action<Vector2, Vector2> TileSwapped;
    public static void NotifySwap(Vector2 origin, Vector2 destination) => TileSwapped?.Invoke(origin, destination);

    public static event Action<Vector2> TileSelected;
    public static void NotifySelected(Vector2 pos) =>TileSelected?.Invoke(pos);

    public static event Action<Vector2> TileUnselected;
    public static void NotifyUnselected(Vector2 pos) =>TileUnselected?.Invoke(pos);

    public static event Action<Vector2Int, SpecialTileCombination> SpecialTileDestroyed;
    public static void NotifySpecialTileCombination(Vector2Int pos,SpecialTileCombination type)=>SpecialTileDestroyed?.Invoke(pos,type);
}