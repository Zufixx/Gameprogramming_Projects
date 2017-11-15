using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private PlayerController playerController;

    public float score;

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
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        score = playerController.GetMaxHeight();
        scoreText.text = "Score: " + Mathf.RoundToInt(score * 10);
	}

    public void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
