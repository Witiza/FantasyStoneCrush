using System;
using UnityEngine;

[CreateAssetMenu]
public class BoardConfig:ScriptableObject
{
    public float TileSize;
    public int BoardWidth = 9;
    public int BoardHeight = 9;
    public int[,] board = null;
}
