﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("Screen")]
    [SerializeField]
    private int resolutionX = 1024;
    [SerializeField]
    private int resolutionY = 896;
    [Header("Keys")]
    [SerializeField]
    private KeyCode resetKey;
    [SerializeField]
    private KeyCode quitKey;

    private void Awake()
    {
        CreateSingleton();
        SetStartResolution(resolutionX, resolutionY);
    }

    private void Update () {
        RestartKey();
        QuitKey();
    }

    private void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void SetStartResolution(int resX, int resY)
    {
        Screen.SetResolution(resX, resY, true);
    }

    private void RestartKey()
    {
        if (Input.GetKeyDown(resetKey))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitKey()
    {
        if (Input.GetKeyDown(quitKey))
            Application.Quit();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
