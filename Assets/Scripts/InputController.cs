using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown ("CycleLevelPos") && SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            NextLevel();
        }
        else if (Input.GetButtonDown ("CycleLevelNeg") && SceneManager.GetActiveScene().buildIndex - 1 >= 0)
        {
            PreviousLevel();
        }
        
        if (Input.GetButtonDown("ExitGame") && SceneManager.GetActiveScene().buildIndex < 2)
        {
            QuitGame();
        }
        
        if (Input.GetButtonDown("RestartLevel"))
        {
            RestartLevel();
        }
        
        // Will be button in settings as well
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveData.Instance.ClearJson();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void PreviousLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClearData()
    {
        SaveData.Instance.ClearJson();
    }
}