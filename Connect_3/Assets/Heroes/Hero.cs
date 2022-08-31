using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class HeroController
{
    protected HeroModel _stats;
    protected BoardController _boardController;
    public int hp;
    public int mana;
    public bool dead;
    public bool shielded = false;
    float manaMultiplier = 1;

    public event Action HeroManaGained;
    public void NotifyManaGained() => HeroManaGained?.Invoke();

    public HeroController(HeroModel stats)
    {
        _stats = stats;
        _stats.GenerateFinalStats();
        mana = 0;
        dead = false;
        _boardController = GameObject.FindGameObjectWithTag("Controller").GetComponent<BoardView>().Board;
    }

    public void activateAbility()
    {
        if(UnityEngine.Random.Range(0, 101) <= _stats.CritChance)
        {
            doAbility(true);
        }
        else
        {
            doAbility(false);
        }
        mana = 0;
    }

    public bool canUseAbility()
    {
        return mana >= _stats.MaxMana;
    } 

    public abstract void doAbility(bool crit);

    public void addMana(int _mana)
    {
        mana += Mathf.RoundToInt(_mana*manaMultiplier);
        if(mana > _stats.MaxMana)
        {
            mana = _stats.MaxMana;
        }
        NotifyManaGained();
    }
}
