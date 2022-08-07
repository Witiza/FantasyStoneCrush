using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class HeroController
{
    protected HeroStats _stats;
    public int hp;
    public int mana;
    public bool dead;
    public bool shielded = false;
    float manaMultiplier = 1;

    public  event Action HeroDamaged;
    public  void NotifyDamaged() => HeroDamaged?.Invoke();
    public event Action HeroHealed;
    public void NotifyHealed() => HeroHealed?.Invoke();
    public event Action HeroManaGained;
    public void NotifyManaGained() => HeroManaGained?.Invoke();

    public HeroController(HeroStats stats)
    {
        _stats = stats;
        hp = stats.maxHp;
        mana = 0;
        dead = false;
    }

    public void activateAbility()
    {
        if(UnityEngine.Random.Range(0,101) <=_stats.critChance)
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

    public void dealDamage(int dmg)
    {
        if (!shielded)
        {
            hp -= dmg;
            if (hp < 0)
            {
                hp = 0;
                dead = true;
            }
        }
        else
        {
            shielded = false;
        }
        NotifyDamaged();
    }

    public void healHP(int heal)
    {
        hp+=heal;   
        if(hp>_stats.maxHp)
        {
            hp = _stats.maxHp;
        }
        NotifyHealed();
    }

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
