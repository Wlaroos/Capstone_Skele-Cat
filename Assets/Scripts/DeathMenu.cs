using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private GameObject _deathCanvasGO;
    private Canvas _deathCanvas;
    private CanvasGroup _deathCanvasGroup;
    
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;


    private void OnEnable()
    {
        _retryButton.onClick.AddListener(InputController.Instance.RestartLevel);
        _mainMenuButton.onClick.AddListener(InputController.Instance.MainMenu);
        _quitButton.onClick.AddListener(InputController.Instance.QuitGame);
    }

    private void OnDisable()
    {
        _retryButton.onClick.RemoveListener(InputController.Instance.RestartLevel);
        _mainMenuButton.onClick.RemoveListener(InputController.Instance.MainMenu);
        _quitButton.onClick.RemoveListener(InputController.Instance.QuitGame);
    }
    
    private void Awake()
    {
        _deathCanvas = _deathCanvasGO.GetComponent<Canvas>();
        _deathCanvasGroup = _deathCanvasGO.GetComponent<CanvasGroup>();

        var main = Camera.main;
        _deathCanvas.worldCamera = main;
    }

    public void ShowDeathMenu()
    {
        _deathCanvasGroup.alpha = 1;
        _deathCanvasGroup.blocksRaycasts = true;
        _deathCanvasGroup.interactable = true;
    }

}
