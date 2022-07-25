using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoreController : MonoBehaviour
{
    public TMP_Text warrior_text;
    public TMP_Text rogue_text;
    public TMP_Text archer_text;
    public TMP_Text mage_text;
    public TMP_Text priest_text;

    private int warrior_score;
    private int rogue_score;
    private int archer_score;
    private int mage_score;
    private int priest_score;

    public int winning_score;
    public int available_moves;
    // Start is called before the first frame update
    private void Awake()
    {
        BoardEvents.TileDestroyed += BoardEvents_TileDestroyed;
        warrior_text.text = $"0/{winning_score}";
        rogue_text.text = $"0/{winning_score}";
        archer_text.text = $"0/{winning_score}";
        mage_text.text = $"0/{winning_score}";
        priest_text.text = $"0/{winning_score}";
    }

    private void BoardEvents_TileDestroyed(Vector2 pos,int type)
    {
        switch ((TileType)type)
        {
            case TileType.NULL:
                Debug.LogError("Trying to update the score of a null tile");
                break;
            case TileType.SHIELD:
                ++warrior_score;
                UpdateScore(warrior_text, warrior_score);
                break;
            case TileType.DAGGER:
                rogue_score++;
                UpdateScore(rogue_text, rogue_score);
                break;
            case TileType.ARROW:
                archer_score++;
                UpdateScore(archer_text, archer_score);
                break;
            case TileType.WAND:
                mage_score++;
                UpdateScore(mage_text, mage_score);
                break;
            case TileType.CHALICE:
                priest_score++;
                UpdateScore(priest_text, priest_score);
                break;
            default:
                break;
        }
    }
    private void UpdateScore(TMP_Text text,int score)
    {
        if(score<=winning_score)
        {
            text.text = $"{score} /{winning_score}";
        }
        else
        {
            text.text = $"{winning_score} /{winning_score}";
        }
    }

    void CheckForVictory()
    {
        if(warrior_score>=winning_score && archer_score>=winning_score&& rogue_score>=winning_score&&mage_score>=winning_score&&priest_score>=winning_score)
        {
            Debug.Log("WINNER WINNER CHICKEN DINNER");
        }
    }
}
