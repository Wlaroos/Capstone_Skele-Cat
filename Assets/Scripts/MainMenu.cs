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

    private void OnEnable()
    {
        _startButton.onClick.AddListener(InputController.Instance.NextLevel);
        _quitButton.onClick.AddListener(InputController.Instance.QuitGame);
    }
    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(InputController.Instance.NextLevel);
        _quitButton.onClick.RemoveListener(InputController.Instance.QuitGame);
    }
}
