using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public void Start()
    {
        Player.GetInstance().OnDied += Score_OnDied;
    }

    private void Score_OnDied(object sender, EventArgs e)
    {
        TrySetNewHighScore(Player.GetInstance().GetKills());
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }
    public  bool TrySetNewHighScore(int score)
    {
        int current_score = GetHighScore();
        if (score > current_score)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
}
