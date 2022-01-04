using System.Collections;
using System;
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
    [SerializeField] Text namePlacehoder;
    [SerializeField] GameObject highscore;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] MainManager MainManager;

    [SerializeField] Transform entryTemplate;
    private List<Transform> HighscoreEntryTransformList;
    private Highscores highscores;

    private void Awake()
    {
        //exit if nothing is asging
        if (entryTemplate == null)
        {
            return;
        }

        //trun off the inital template
        entryTemplate.gameObject.SetActive(false);
    }

    public void LoadHighscores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                HighscoreEntryList = new List<HighscoreEntry>()
            };
        }

        //create new list
        HighscoreEntryTransformList = new List<Transform>();

        // add enteris to list and show them
        foreach (HighscoreEntry HighscoreEntry in highscores.HighscoreEntryList)
        {
            createHighscoreEnrtyTransform(HighscoreEntry,HighscoreEntryTransformList);
        }
    }

    private void createHighscoreEnrtyTransform(HighscoreEntry HighscoreEntry, List<Transform> transformList)
    {
        //step height
        float templateHeight = 25f;
        //copy the template and save as variable
        Transform entryTransform = Instantiate(entryTemplate, entryTemplate.parent);
        //save its rectTransform as variable
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        //move each template down
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        //trun it on
        entryTransform.gameObject.SetActive(true);

        //add 1 to rank so it want begin from zero
        int rank = transformList.Count + 1;
        string rankString;
        // change the rank number to pretty text
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        // set the rank to the text
        entryTransform.Find("rank").GetComponent<Text>().text = rankString;
        // set the name to text
        entryTransform.Find("name").GetComponent<Text>().text = HighscoreEntry.name;
        // get and set the score to text
        int score = HighscoreEntry.score;
        entryTransform.Find("score").GetComponent<Text>().text = score.ToString();
        
        if (rank % 2 == 1)
        {
            entryTransform.Find("bg").GetComponent<Image>().color = new Color32(210, 210, 210, 255);
        }
        // add a single entry to the list
        transformList.Add(entryTransform);
    }

    private class Highscores
    {
        public List<HighscoreEntry> HighscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void LoadMain()
    {
        if (playerName)
        {
            if (!String.IsNullOrEmpty(playerName.text))
            {
                MainManager.playerName = playerName.text;
            } else
            {
                namePlacehoder.text = "MUST ENTER NAME";
                return;
            } 
        }
        SceneManager.LoadScene("main");
    }

    public void Exit()
    {
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
            highscore.GetComponent<Text>().text = "BEST: " + name + " " + score.ToString(); 
        } else
        {
            highscore.SetActive(false);
        }
    }

    public void setMenuHighscoreText()
    {
        if (highscores.HighscoreEntryList.Count > 0)
        {
            highscore.SetActive(true);
            highscore.GetComponent<Text>().text = "BEST: " + highscores.HighscoreEntryList[0].name + " " + highscores.HighscoreEntryList[0].score;
        }
        else
        {
            highscore.SetActive(false);
        }

    }
}
