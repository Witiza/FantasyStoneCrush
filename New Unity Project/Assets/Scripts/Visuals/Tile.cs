using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum  TileType
{
    SHIELD,
    DAGGER,
    ARROW,
    WAND,
    CHALICE,
    BOMB,
    ROCKET,
    NULL
}
public class Tile : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[5];
    public TileType tile_type;

    private void Awake()
    {
        tile_type = (TileType)Random.Range(0, 5);
        gameObject.name = $"Tile{tile_type}";
        GetComponent<SpriteRenderer>().sprite = sprites[(int)tile_type];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
