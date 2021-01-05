using System;
using UnityEngine.UI;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    private HealthSystem health;
    private Text HealthTXT;

    private void Awake()
    {
        HealthTXT = GameObject.Find("HealthTxt").GetComponent<Text>();
    }
    private void Start()
    {
        health = Player.GetInstance().health;
        HealthTXT.text = ": " + health.GetHealth().ToString();

        health.OnHealthChange += GameUI_OnHealthChange;
    }

    private void GameUI_OnHealthChange(object sender, EventArgs e)
    {
        HealthTXT.text = ": " + health.GetHealth();
    }
}
