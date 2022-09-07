using UnityEngine;

public class MageController : HeroController
{
    public MageController(HeroModel stats) : base(stats)
    {

    }

    public override void doAbility(bool crit)
    {
        int amount = crit == true ? _stats.CriticalStrength : _stats.NormalStrength;
        for (int i = 0; i < amount; i++)
        {
            int x = Random.Range(0, _boardController.BoardWidth);
            int y = Random.Range(0, _boardController.BoardHeight);
            if (!_boardController.ChangeTile(new Vector2Int(x, y), TileType.BOMB))
                i--;
        }
        _boardController.ProcessBoard();
    }
}
