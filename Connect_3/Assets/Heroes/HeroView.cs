using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : MonoBehaviour 
{
    protected HeroController controller;

    public HeroStats Stats;

    public TileType heroType;

    public Image HealthBar;
    public Image ManaBar;
    public Booster ManaBooster;

    public void Start()
    {
        BoardEvents.TileDestroyed += BoardEventsTileDestroyed;
        SelectHeroController();
        controller.HeroDamaged += ControllerHeroDamaged;
        controller.HeroHealed += ControllerHeroHealed;
        controller.HeroManaGained += ControllerHeroManaGained;
        ManaBooster.BoosterEvent += ManaBoosterEvent;
        UpdateBar(HealthBar, Stats.maxHp, Stats.maxHp);
        UpdateBar(ManaBar, Stats.maxMana, Stats.maxMana);
    }

    private void ManaBoosterEvent()
    {
        controller.addMana(Stats.maxMana);
    }

    private void ControllerHeroManaGained()
    {
        UpdateBar(ManaBar, controller.mana, Stats.maxMana);
    }

    private void ControllerHeroHealed()
    {
        UpdateBar(HealthBar, controller.hp, Stats.maxHp);
    }

    private void ControllerHeroDamaged()
    {
        UpdateBar(HealthBar, controller.hp, Stats.maxHp);
    }

    public void OnDestroy()
    {
        BoardEvents.TileDestroyed -= BoardEventsTileDestroyed;
        controller.HeroDamaged -= ControllerHeroDamaged;
        controller.HeroHealed -= ControllerHeroHealed;
        controller.HeroManaGained -= ControllerHeroManaGained;
        ManaBooster.BoosterEvent -= ManaBoosterEvent;

    }
    public void AbilityButton()
    {
        Debug.Log("Hero ability used");
        if (controller.canUseAbility())
        {
            controller.activateAbility();
        }
    }

    public void BoardEventsTileDestroyed(Vector2 pos, int type)
    {
        if ((TileType)type == heroType)
        {
            controller.addMana(5);
        }
    }

    public void UpdateBar(Image bar, int value, int max_value )
    {
        bar.fillAmount = (float)value / (float)max_value;
    }

    public void SelectHeroController()
    {
        switch (heroType)
        {
            case TileType.SHIELD:
                controller = new WarriorController(Stats);
                break;
            case TileType.DAGGER:
                controller = new RogueController(Stats);
                break;
            case TileType.ARROW:
                controller = new ArcherController(Stats);
                break;
            case TileType.WAND:
                controller = new MageController(Stats);
                break;
            case TileType.CHALICE:
                controller = new PriestController(Stats);
                break;
            default:
                Debug.LogError("YOU CAN ONLY SELECT BASE TYPES FOR HEROES");
                break;
        }
    }
}
