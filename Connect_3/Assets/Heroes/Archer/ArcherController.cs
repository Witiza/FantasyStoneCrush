using UnityEngine;
public class ArcherController : HeroController
{
    public ArcherController(HeroStats stats) : base(stats)
    {

    }

    public override void doAbility(bool crit)
    {
        int amount = crit == true ? _stats.archerArrowAmount.x : _stats.archerArrowAmount.y;
        for(int i = 0; i < amount; i++)
        {
            int x = Random.Range(0,_boardController.BoardWidth);
            int y = Random.Range(0,_boardController.BoardHeight);
            _boardController.TargetAndDestroyTile(new Vector2Int(x, y));
        }
        _boardController.ProcessBoard();
    }
}
