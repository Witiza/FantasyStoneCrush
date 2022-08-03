using UnityEngine;

class ModelTile
{
    public Vector2Int BoardPosition;
    public TileType TileType;
    public ModelTile(Vector2Int boardPosition, TileType tileType)
    {
        BoardPosition = boardPosition;
        TileType = tileType;
    }
}
