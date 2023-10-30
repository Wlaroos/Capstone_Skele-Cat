using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _clearDataConfirmButton;
    [SerializeField] private Button _defaultValuesButton;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(NextLevel);
        _quitButton.onClick.AddListener(QuitGame);
        _clearDataConfirmButton.onClick.AddListener(InputController.Instance.ClearData);
        _defaultValuesButton.onClick.AddListener(SettingsManager.Instance.DefaultValues);
    }
    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(NextLevel);
        _quitButton.onClick.RemoveListener(QuitGame);
        _clearDataConfirmButton.onClick.RemoveListener(InputController.Instance.ClearData);
        _defaultValuesButton.onClick.RemoveListener(SettingsManager.Instance.DefaultValues);
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
