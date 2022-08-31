using UnityEngine;
public class WarriorController : HeroController
{
    public WarriorController(HeroModel stats) : base(stats)
    {

    }

    public override void doAbility(bool crit)
    {
        int amount = crit == true ? _stats.CriticalStrength : _stats.NormalStrength;
        int x = Random.Range(0, _boardController.BoardWidth);
        int y = Random.Range(0, _boardController.BoardHeight);
        _boardController.DestroyArea(amount, new Vector2(x, y));
        _boardController.ProcessBoard();
    }
}
