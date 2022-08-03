using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class HeroController
{
    protected HeroStats _stats;
    int hp;
    int mana;
    bool dead;
    bool shielded;

    public HeroController(HeroStats stats)
    {
        _stats = stats;
        hp = stats.maxHp;
        mana = 0;
        dead = false;
    }

    public void activateAbility()
    {
        if(Random.Range(0,101) <=_stats.critChance)
        {
            doAbility(true);
        }
        else
        {
            doAbility(false);
        }
    }
    public abstract void doAbility(bool crit);

    public void dealDamage(int dmg)
    {
        hp -= dmg;
        if(hp < 0)
        {
            hp = 0;
            dead = true;
        }
    }
}
