using UnityEngine;

[CreateAssetMenu]
public class HeroStats : ScriptableObject
{
    public int maxHp;
    public int maxMana;
    public int critChance;
    public int manaGainMultiplier;
}