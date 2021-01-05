using System;
using UnityEngine;

public class SignUI : MonoBehaviour
{
    public GameObject BattlePopup_UI; //Referencia da tabuleta do Battle
    public GameObject TutorialPopup_UI;//Referencia da tabuleta do Tutorial

    private Transform Battle_Tab;
    private Transform Tutorial_Tab;

    private void Start()
    {
        BattlePopup_UI.SetActive(false);
        TutorialPopup_UI.SetActive(false);

        Battle_Tab = GameObject.Find("Tabuleta_Battle").GetComponent<Transform>();
        Tutorial_Tab = GameObject.Find("Tabuleta_Tut").GetComponent<Transform>();

        Player.GetInstance().OnStartBattle += SignUI_OnStartBattle;
    }

    /*Assim que começa a batalha as tabuletas deixam de Funcionar*/
    private void SignUI_OnStartBattle(object sender, EventArgs e)
    {
        BattlePopup_UI.SetActive(false);
        enabled = false;
    }

    private void Update()
    {
        if(Vector3.Distance(Player.GetInstance().GetComponent<Transform>().position, Battle_Tab.gameObject.transform.position) < 2) 
        {
            BattlePopup_UI.SetActive(true);
        }
        else if(Vector3.Distance(Player.GetInstance().GetComponent<Transform>().position, Tutorial_Tab.gameObject.transform.position) < 2)
        {
            TutorialPopup_UI.SetActive(true);
        }
        else 
        {
            BattlePopup_UI.SetActive(false);
            TutorialPopup_UI.SetActive(false);
        }
    }
}
