using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        BoardEvents.TileSelected += BoardEvents_TileSelected;
        BoardEvents.TileUnselected += BoardEvents_TileUnselected;
    }

    private void OnDestroy()
    {
        BoardEvents.TileSelected -= BoardEvents_TileSelected;
        BoardEvents.TileUnselected -= BoardEvents_TileUnselected;
    }


    private void BoardEvents_TileUnselected(Vector2 obj)
    {
        VisualTile tile = VisualHandler.GetTileAtPos(obj);
        tile.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void BoardEvents_TileSelected(Vector2 obj)
    {
        VisualTile tile = VisualHandler.GetTileAtPos(obj);
        tile.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
