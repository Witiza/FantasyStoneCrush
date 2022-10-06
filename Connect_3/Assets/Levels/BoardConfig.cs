using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Array2DEditor;

public enum GameObjectives
{
    SCORE,
    BOXES
}
[CreateAssetMenu]
public class BoardConfig : ScriptableObject
{
    public float TileSize = 0.5f;
    public GameObjectives Objective;
    public int AvailableMoves=20;
    public int ScoreOrBoxesNeeded=20;
    [Header("Tile Numbers:\n\nRandom: 0\nBase Tiles: 1-4\nUnused Base Tile: 5\nBomb: 6\nVerticalRocket: 7\nHorizontalRocket: 8\nBox: 9")]
    public Array2DInt board = new Array2DInt();
    public GameObject tutorial;
    public string _tutorial;
    public int BoardWidth { get => board.GridSize.x; }
    public int BoardHeight { get => board.GridSize.y; }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BoardConfig))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        BoardConfig boardConfig = (BoardConfig)target;
        if(boardConfig.Objective == GameObjectives.BOXES)
        {
            if (GUILayout.Button("UpdateScore"))
            {
                int rocks = 0;
                for(int i = 0; i < boardConfig.BoardWidth; i++)
                {
                    for(int j = 0; j < boardConfig.BoardHeight; j++)
                    {
                        if (boardConfig.board[i,j] == 9)
                        {
                            rocks++;
                        }
                    }
                }
                boardConfig.ScoreOrBoxesNeeded = rocks;
            }
        }
    }
}
#endif


