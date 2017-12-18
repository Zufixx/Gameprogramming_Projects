using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool gameOver = false;

    [SerializeField]
    private Text healthText;
    [SerializeField]
    private GameObject player;

    void Awake()
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
        GetComponent<OpeningScript>().Opening();
        player.SetActive(false);
    }

    public void AfterOpening()
    {
        player.SetActive(true);
    }

    void Update () {
		if(gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}

    public void GameOver()
    {
        gameOver = true;
        //Debug.Log("Game Over");
        healthText.text = "Game Over!";
        player.SetActive(false);
    }
}
