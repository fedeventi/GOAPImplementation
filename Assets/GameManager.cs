using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuUI;
    public static GameManager Instance;
    public static bool gameIsPaused = false;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        if(menuUI!=null)
            menuUI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
        if (menuUI != null)
            menuUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        if (menuUI != null)
            menuUI.SetActive(false);
    }

    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("FirstLevel");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
   
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }

    public void Lose()
    {
        SceneManager.LoadScene("Lose");
    }

}
