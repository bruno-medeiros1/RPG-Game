using UnityEngine.UI;
using UnityEngine;
using System;

public class GameOverUI : MonoBehaviour
{
    /*Referencia ao nosso text Component do GameOverUI*/
    private Text scoretxt;
    private Text HighScoretxt;

    private void Awake()
    {
        scoretxt = transform.Find("Mole_KIlls_TXT").GetComponent<Text>();
        HighScoretxt = transform.Find("HighscoreTXT").GetComponent<Text>();
    }

    private void Start()
    {
        //nao mostra a UI do gameOver
        Hide();

        //Subscrevemos ao evento
        Player.GetInstance().OnDied += GameOver_OnPlayerDied;
    }

    private void GameOver_OnPlayerDied(object sender, EventArgs e)
    {
        scoretxt.text = "Kills: " + Player.GetInstance().GetKills().ToString();

        if (Player.GetInstance().GetKills() >= Score.GetHighScore())
        {
            /* NEW HIGHSCCORE*/
            HighScoretxt.text = "NEW HIGHSCORE: " + Player.GetInstance().GetKills();
        }
        else
        {
            HighScoretxt.text = "HIGHSCORE: " + Score.GetHighScore();
        }
        Show();
        SoundManager.GetInstance().Play("GameOver");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
