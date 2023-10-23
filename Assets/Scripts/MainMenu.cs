using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartButton);
        _exitButton.onClick.AddListener(ExitButton);
    }
    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartButton);
        _exitButton.onClick.RemoveListener(ExitButton);
    }
    
    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ExitButton()
    {
        Application.Quit();
    }
}
