using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("Score")]
    public float score;
    private int actualScore;
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private PlayerController playerController;

    [Header("Highscore")]
    [SerializeField]
    private string path = "Highscore.txt";
    [SerializeField]
    private Text highscoreText;
    [SerializeField]
    private Text highscoreScore;

    private int highScore;

    [SerializeField]
    private GameObject previousHighscorePrefab;

    [Header("Game Over")]
    [SerializeField]
    private Text gameOverText;
    public bool gameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameObject previousHighScore;
        highScore = ReadHighScore();
        Vector2 spawnPos = new Vector2(2.75f, highScore / 10f);
        previousHighScore = Instantiate(previousHighscorePrefab, spawnPos, Quaternion.identity);
        previousHighScore.GetComponent<TextMesh>().text = "Highscore:\n" + highScore.ToString();
    }

    // Update is called once per frame
    void Update ()
    {
        score = playerController.GetMaxHeight();
        actualScore = Mathf.RoundToInt(score * 10);
        scoreText.text = "Score: " + actualScore;

        if (gameOver)
        {
            if(Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void GameOver()
    {
        if(!gameOver)
        {
            gameOver = true;

            gameOverText.text = "Game Over";

            if (!File.Exists(path))
            {
                WriteHighScore("0");
                highScore = 0;
            }
            else
            {
                highScore = ReadHighScore();
            }

            if (actualScore > highScore)
            {
                highscoreText.text = "New highscore!";
                highscoreScore.text = actualScore.ToString() + "\nPress R to reload";

                WriteHighScore(actualScore.ToString());
            }
            else
            {
                highscoreText.text = "Score: " + actualScore.ToString();
                highscoreScore.text = "Highscore: " + highScore.ToString() +"\nPress R to reload";
            }
        }
    }

    private void WriteHighScore(string toWrite)
    {
        File.WriteAllText(path, toWrite);
    }

    private int ReadHighScore()
    {
        if (!File.Exists(path))
        {
            WriteHighScore("0");
            highScore = 0;
        }

        int tempHS;
        using (FileStream fs = File.Open(path, FileMode.Open))
        using (StreamReader reader = new StreamReader(fs))
        {
            tempHS = int.Parse(reader.ReadToEnd());
            reader.Close();
            fs.Close();
        }
        return tempHS;
    }
}
