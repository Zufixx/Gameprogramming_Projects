using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private PlayerController playerController;

    public float score;

	// Update is called once per frame
	void Update () {
        score = playerController.GetMaxHeight();
        scoreText.text = "Score: " + Mathf.RoundToInt(score * 10);
	}

    public void GameOver()
    {

    }
}
