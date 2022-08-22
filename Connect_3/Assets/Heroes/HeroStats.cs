using UnityEngine;

[CreateAssetMenu]
public class HeroStats : ScriptableObject
{
    public int maxMana;
    public int critChance;
    public int manaGainMultiplier;

    [Header("Only use the one that pertains to the class")]
    public Vector2Int archerArrowAmount;
    public Vector2Int mageTileAmount;
    public Vector2Int priestHealAmount;
    public Vector2Int rogueDamageAmount;
    public Vector2Int warriorShieldAmount;
}