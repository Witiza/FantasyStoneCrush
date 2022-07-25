using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VisualTile : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[5];
    public Vector2 BoardPos;
    Vector2 original_world_pos;

    private void Awake()
    {
        original_world_pos = GameObject.FindGameObjectWithTag("Board").transform.position;
    }
    public void InitializeTile(TileType type, Vector2 pos)
    {
        SetSprite(type);
        SetBoardPosition(pos);
    }
     public void SetSprite(TileType type)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[(int)type];
        gameObject.name = $"Tile {type}";
    }
    public void SetBoardPosition(Vector2 pos)
    {
        BoardPos = pos;
        SetWorldPosition();
    }

    public void SetWorldPosition()
    {
       transform.position = new Vector3(original_world_pos.x + BoardPos.x * Board._tileSize, original_world_pos.y + BoardPos.y* Board._tileSize, 0);
    }
}
