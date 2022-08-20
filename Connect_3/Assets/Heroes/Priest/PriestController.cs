﻿using UnityEngine;
public class PriestController : HeroController
{
    public PriestController(HeroStats stats) : base(stats)
    {

    }

    public override void doAbility(bool crit)
    {
        int amount = crit == true ? _stats.priestHealAmount.x : _stats.priestHealAmount.y;
        int x = Random.Range(0, _boardController.BoardWidth);
        int y = Random.Range(0, _boardController.BoardHeight);
        for (int i = 0; i < amount; i++)
        {
            _boardController.TargetAndDestroyTile(new Vector2Int(x , y + i));
            _boardController.TargetAndDestroyTile(new Vector2Int(x, y - i));
            _boardController.TargetAndDestroyTile(new Vector2Int(x - i, y));
            _boardController.TargetAndDestroyTile(new Vector2Int(x + i, y));
        }
        _boardController.ProcessBoard();
    }
}