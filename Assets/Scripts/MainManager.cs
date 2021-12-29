using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Brick BrickPrefab;
    [SerializeField] private int LineCount = 6;

    [SerializeField] private Rigidbody Ball;
    [SerializeField] private MenuUI MenuUI;

    public static string playerName;
    public static string highscorePlayer;
    public static int highscore;

    private bool m_Started;
    private int m_Points;
    private bool m_GameOver;
    private string sceneName;  

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "main")
        {
            m_GameOver = false;
            m_Started = false;
            //Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
            //MenuUI = GameObject.FindGameObjectWithTag("MenuUI").GetComponent<MenuUI>();
            MenuUI.showGameOverMenu(false);
            makeBricks();
        }

        MenuUI.setMainHighscoreText(highscorePlayer, highscore);
        // => load data from file here <=
        //if (Instance != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //Instance = this;
        //DontDestroyOnLoad(gameObject);
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
            //if (!m_Started)
            //{
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Started = true;
                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();

                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            //}
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
        if (m_Points > highscore)
        {
            highscore = m_Points;
            highscorePlayer = playerName;
        }

        MenuUI.showGameOverMenu(true);
        MenuUI.setMainHighscoreText(highscorePlayer, highscore);
    }
}
