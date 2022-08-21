using System;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public enum GameObjectives
{
    SCORE,
    BOXES,
    ENEMIES
}
[CreateAssetMenu]
public class BoardConfig : ScriptableObject
{
    public float TileSize = 0.5f;
    public GameObjectives Objective;
    public int AvailableMoves=20;
    public int ScoreOrBoxesNeeded=20;
    [Header("Tile Numbers:\n\nRandom: -1\nNull, dont use: 0\nBase Tiles: 1-5\nBomb: 6\nVerticalRocket: 7\nHorizontalRocket: 8\nBox: 9")]
    public Array2DInt board = new Array2DInt();
    public int BoardWidth { get =>board.GridSize.x; }
    public int BoardHeight { get => board.GridSize.y; }

}


