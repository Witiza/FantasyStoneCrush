using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[5];
    public Vector2 board_pos;
    public Vector2 original_world_pos;

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
        board_pos = pos;
        SetWorldPosition();
    }

    public void SetWorldPosition()
    {
       transform.position = new Vector3(original_world_pos.x + board_pos.x * Board.tile_size, original_world_pos.y + board_pos.y* Board.tile_size, 0);
    }
}
