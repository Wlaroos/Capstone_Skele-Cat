using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    private static InputController Instance { get; set; }
    
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetButtonDown ("CycleLevelNeg") && SceneManager.GetActiveScene().buildIndex - 1 >= 0)
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex - 1);
        }
        
        if (Input.GetButtonDown("ExitGame") && SceneManager.GetActiveScene().buildIndex < 2)
        {
            Application.Quit();
        }
        
        if (Input.GetButtonDown("RestartLevel"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        // Will be button in settings as well
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveData.Instance.ClearJson();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}