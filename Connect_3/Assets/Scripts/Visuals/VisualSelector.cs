using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSelector
{
    public VisualSelector()
    {
        SubscribeEvents();
    }

    ~VisualSelector()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        BoardEvents.TileSelected += BoardEventsTileSelected;
        BoardEvents.TileUnselected += BoardEventsTileUnselected;
    }

    public  void UnsubscribeEvents()
    {
        BoardEvents.TileSelected -= BoardEventsTileSelected;
        BoardEvents.TileUnselected -= BoardEventsTileUnselected;
    }


    private void BoardEventsTileUnselected(Vector2 obj)
    {
        VisualTile tile = BoardView.GetTileAtPos(obj);
        tile.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void BoardEventsTileSelected(Vector2 obj)
    {
        VisualTile tile = BoardView.GetTileAtPos(obj);
        tile.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
