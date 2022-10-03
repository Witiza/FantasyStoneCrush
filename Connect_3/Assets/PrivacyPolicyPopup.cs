using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyPolicyPopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("PrivacyPolicy", 0) == 1)
        {
            Destroy(gameObject);
        }
    }
    public void AcceptTerms()
    {
        PlayerPrefs.SetInt("PrivacyPolicy", 1);
        Destroy(gameObject);
    }
    public void ViewTerms()
    {
        Application.OpenURL("https://witiza.github.io/PRIVACY-POLICY/");
    }
    public void DenyTerms()
    {
        Application.Quit();
    }
}
