using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    private TextMeshProUGUI LevelUI;
    private Text EnemiesTXT;
    private void Awake()
    {
        LevelUI = GameObject.Find("Level").GetComponent<TextMeshProUGUI>();
        EnemiesTXT = GameObject.Find("MolesAliveTXT").GetComponent<Text>();
    }
    private void Start()
    {
        EnemiesTXT.transform.parent.gameObject.SetActive(false);
        LevelUI.gameObject.SetActive(false);
        Player.GetInstance().OnStartBattle += BattleUI_OnStartBattle;

        Level.GetInstance().OnNewLevel += BattleUI_OnNewLevel;
    }

    private void BattleUI_OnStartBattle(object sender, EventArgs e)
    {
        EnemiesTXT.transform.parent.gameObject.SetActive(true);
        EnemiesTXT.text = "Alive: " + 1;
        LevelUI.gameObject.SetActive(true);       
        LevelUI.text = "EASY MODE";
        StartCoroutine("DisableLevel_UI");
    }

    private void BattleUI_OnNewLevel(object sender, EventArgs e)
    {
        if(Level.GetInstance().level == 2) 
        {
            LevelUI.gameObject.SetActive(true);
            LevelUI.text = "MEDIUM MODE";
            LevelUI.color = Color.yellow;
            StartCoroutine("DisableLevel_UI");
        }
        if(Level.GetInstance().level == 3) 
        {
            LevelUI.gameObject.SetActive(true);
            LevelUI.text = "HARD MODE";
            LevelUI.color = Color.red;
            StartCoroutine("DisableLevel_UI");
        }
    }

    IEnumerator DisableLevel_UI() 
    {
        yield return new WaitForSeconds(5f);
        LevelUI.gameObject.SetActive(false);
    }
}
