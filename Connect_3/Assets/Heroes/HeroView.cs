using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ParticleSystemJobs;

public class HeroView : MonoBehaviour 
{
    protected HeroController controller;

    public HeroModel Stats;

    public TileType heroType;

    public Image ManaBar;
    public Booster ManaBooster;
    public ParticleSystem particles;

    public void Start()
    {
        BoardEvents.TileDestroyed += BoardEventsTileDestroyed;
        SelectHeroController();
        controller.HeroManaGained += ControllerHeroManaGained;
        ManaBooster.BoosterEvent += ManaBoosterEvent;
        UpdateBar(ManaBar, 0, Stats.MaxMana);
    }

    public void AbilityButton()
    {
        Debug.Log("Hero ability used");
        if (controller.canUseAbility())
        {
            controller.activateAbility();
            UpdateBar(ManaBar, controller.mana, Stats.MaxMana);
        }
    }

    public void BoardEventsTileDestroyed(Vector2 pos, TileType type)
    {
        if (type == heroType)
        {
            controller.addMana(Stats.ManaGain);
        }
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
        }
    }

    private void ManaBoosterEvent(bool success)
    {
        if (success)
            controller.addMana(Stats.MaxMana);
    }

    private void ControllerHeroManaGained()
    {
        UpdateBar(ManaBar, controller.mana, Stats.MaxMana);
        particles.Emit(1);
    }

    public void OnDestroy()
    {
        BoardEvents.TileDestroyed -= BoardEventsTileDestroyed;
        controller.HeroManaGained -= ControllerHeroManaGained;
        ManaBooster.BoosterEvent -= ManaBoosterEvent;
    }

    public void UpdateBar(Image bar, int value, int max_value)
    {
        bar.fillAmount = (float)value / (float)max_value;
    }
}
