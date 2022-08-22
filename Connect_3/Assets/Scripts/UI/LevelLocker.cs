using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLocker : MonoBehaviour
{
    public int level;
    public PlayerProgressionSO progression;
    Image image;
    public Color disabled;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if(progression.MaxLevelUnlocked<level)
        {
            image.color = disabled;
        }
    }
}
