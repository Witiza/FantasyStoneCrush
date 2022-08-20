using UnityEngine;

[CreateAssetMenu]
public class PlayerProgressionSO  : ScriptableObject
{
    public int CurrentLevel { get; set; }
    public int Coins { get; set; }

    public int TurnBoosterAmount { get; set; }
    public int ManaBoosterAmount { get; set; }
    public int TileBoosterAmount { get; set; }
}