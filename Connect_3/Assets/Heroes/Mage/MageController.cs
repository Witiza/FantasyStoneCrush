using UnityEngine;

public class MageController : HeroController
{
    public MageController(HeroStats stats) : base(stats)
    {

    }

    public override void doAbility(bool crit)
    {
        int amount = crit == true ? _stats.mageTileAmount.x : _stats.mageTileAmount.y;
        for (int i = 0; i < amount; i++)
        {
            int x = Random.Range(0, _boardController.BoardWidth);
            int y = Random.Range(0, _boardController.BoardHeight);
            _boardController.ChangeTile(new Vector2Int(x, y), TileType.BOMB);
        }
        _boardController.ProcessBoard();
    }
}
