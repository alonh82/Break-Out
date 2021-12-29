using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUI : MonoBehaviour
{
    [SerializeField] Text playerName;
    [SerializeField] GameObject highscore;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] MainManager MainManager;


    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void LoadMain()
    {
        if (playerName != null)
        {
            MainManager.playerName = playerName.text;
        }
        SceneManager.LoadScene("main");
    }

    public void Exit()
    {
        // ==> on exit save user data here <==
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }

    public void showGameOverMenu(bool active)
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(active);
        }
    }

    public void setCurrentScoreText(int score)
    {
        if (scoreText != null) 
        {
            scoreText.text = "SCORE: " + score.ToString();
        }
    }

    public void setMainHighscoreText(string name, int score)
    {
        if (score > 0)
        {
            highscore.SetActive(true);
            highscore.GetComponent<Text>().text = "HIGHSCORE - " + name + " " + score.ToString(); 
        } else
        {
            highscore.SetActive(false);
        }
    }
}
