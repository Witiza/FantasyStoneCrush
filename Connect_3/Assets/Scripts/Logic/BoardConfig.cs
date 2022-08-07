using System;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

[CreateAssetMenu]
public class BoardConfig : ScriptableObject
{
    public float TileSize;
    public Array2DInt board = new Array2DInt();
    public int BoardWidth { get =>board.GridSize.x; }
    public int BoardHeight { get => board.GridSize.y; }

}


