using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class HeroController
{
    protected HeroStats _stats;
    protected BoardController _boardController;
    public int hp;
    public int mana;
    public bool dead;
    public bool shielded = false;
    float manaMultiplier = 1;

    public event Action HeroManaGained;
    public void NotifyManaGained() => HeroManaGained?.Invoke();

    public HeroController(HeroStats stats)
    {
        _stats = stats;
        mana = 0;
        dead = false;
        _boardController = GameObject.FindGameObjectWithTag("Controller").GetComponent<BoardView>().Board;
    }

    public void activateAbility()
    {
        if(UnityEngine.Random.Range(0, 101) <= _stats.critChance)
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
        return mana >= _stats.maxMana;
    } 

    public abstract void doAbility(bool crit);

    public void addMana(int _mana)
    {
        mana += Mathf.RoundToInt(_mana*manaMultiplier);
        if(mana > _stats.maxMana)
        {
            mana = _stats.maxMana;
        }
        NotifyManaGained();
    }
}
