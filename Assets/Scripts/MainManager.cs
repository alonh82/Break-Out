//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Brick BrickPrefab;
    [SerializeField] private int LineCount = 6;

    [SerializeField] private Rigidbody Ball;
    [SerializeField] private MenuUI MenuUI;

    public static string playerName;

    private bool m_Started;
    private int m_Points;
    private bool m_GameOver;
    private string sceneName;
    private List<HighscoreEntry> HighscoreEntryList;
    private Highscores highscores;
    private int number = 5;

    private void Awake()
    {
        LoadHighscores();

        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "main")
        {
            m_GameOver = false;
            m_Started = false;
            MenuUI.showGameOverMenu(false);
            makeBricks();

            Debug.Log("Player: " + playerName);
        }

        if (sceneName == "menu")
        {
            MenuUI.LoadHighscores();
        } 
    }

    // Start is called before the first frame update
    void makeBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (sceneName == "main" && !m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        MenuUI.setCurrentScoreText(m_Points);
        //ScoreText.text = $"Score: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;

        AddHighscoreEntry(m_Points, playerName);
        MenuUI.showGameOverMenu(true);
    }


    private void LoadHighscores()
    {
        //PlayerPrefs.DeleteKey("highscoreTable");
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
    }

    private void AddHighscoreEntry(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Add new entry to Highscores
        highscores.HighscoreEntryList.Add(highscoreEntry);
        SortHighscores();
        SaveHighscores();
    }

    private void SaveHighscores()
    {
        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void SortHighscores()
    {
        //sort the list by score
        for (int i = 0; i < highscores.HighscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.HighscoreEntryList.Count; j++)
            {
                if (highscores.HighscoreEntryList[j].score > highscores.HighscoreEntryList[i].score)
                {
                    //swap places
                    HighscoreEntry tmp = highscores.HighscoreEntryList[i];
                    highscores.HighscoreEntryList[i] = highscores.HighscoreEntryList[j];
                    highscores.HighscoreEntryList[j] = tmp;
                }
            }
        }

        Debug.Log("Enteries: " + highscores.HighscoreEntryList.Count);
        if (highscores.HighscoreEntryList.Count > 10)
        {
            highscores.HighscoreEntryList.RemoveRange(9, highscores.HighscoreEntryList.Count - 10);
        }
    }

    private class Highscores
    {
        public List<HighscoreEntry> HighscoreEntryList;
    }

    //represents a single high score entry
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
