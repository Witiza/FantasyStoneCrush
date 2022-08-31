using UnityEngine;

public class RogueController : HeroController
{
    public RogueController(HeroModel stats) : base(stats)
    {

    }

    public override void doAbility(bool crit)
    {
        int amount = crit == true ? _stats.CriticalStrength : _stats.NormalStrength;
        int x = Random.Range(0, _boardController.BoardWidth);
        int y = Random.Range(0, _boardController.BoardHeight);
        for(int i = 0; i < amount; i++)
        {
            _boardController.TargetAndDestroyTile(new Vector2Int(x + i, y + i));
            _boardController.TargetAndDestroyTile(new Vector2Int(x + i, y - i));
            _boardController.TargetAndDestroyTile(new Vector2Int(x - i, y + i));
            _boardController.TargetAndDestroyTile(new Vector2Int(x - i, y - i));
        }
        _boardController.ProcessBoard();
    }
}
