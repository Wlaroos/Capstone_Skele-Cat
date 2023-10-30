using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject _pauseCanvasGO;
    [SerializeField] private GameObject _settingsCanvasGO;
    [SerializeField] private GameObject _confirmCanvasGO;
    
    private Canvas _pauseCanvas;
    private Canvas _settingsCanvas;
    private Canvas _confirmCanvas;
    
    private CanvasGroup _pauseCanvasGroup;
    private CanvasGroup _settingsCanvasGroup;
    private CanvasGroup _confirmCanvasGroup;
    
    [SerializeField] private Button _pauseReturnButton;
    [SerializeField] private Button _pauseRestartButton;
    [SerializeField] private Button _pauseMainMenuButton;
    
    [SerializeField] private Button _clearDataConfirmButton;
    
    [SerializeField] private Button _defaultValuesButton;

    private bool _isVisible = false;

    private void OnEnable()
    {
        _pauseReturnButton.onClick.AddListener(PauseReturnButton);
        _pauseRestartButton.onClick.AddListener(InputController.Instance.RestartLevel);
        _pauseMainMenuButton.onClick.AddListener(InputController.Instance.MainMenu);
        _clearDataConfirmButton.onClick.AddListener(InputController.Instance.ClearData);
        _defaultValuesButton.onClick.AddListener(SettingsManager.Instance.DefaultValues);
    }
    private void OnDisable()
    {
        _pauseReturnButton.onClick.RemoveListener(PauseReturnButton);
        _pauseRestartButton.onClick.RemoveListener(InputController.Instance.RestartLevel);
        _pauseMainMenuButton.onClick.RemoveListener(InputController.Instance.MainMenu);
        _clearDataConfirmButton.onClick.RemoveListener(InputController.Instance.ClearData);
        _defaultValuesButton.onClick.RemoveListener(SettingsManager.Instance.DefaultValues);
        Time.timeScale = 1;
    }

    private void Awake()
    {
        _pauseCanvas = _pauseCanvasGO.GetComponent<Canvas>();
        _settingsCanvas = _settingsCanvasGO.GetComponent<Canvas>();
        _confirmCanvas = _confirmCanvasGO.GetComponent<Canvas>();
        
        _pauseCanvasGroup = _pauseCanvasGO.GetComponent<CanvasGroup>();
        _settingsCanvasGroup = _settingsCanvasGO.GetComponent<CanvasGroup>();
        _confirmCanvasGroup = _confirmCanvasGO.GetComponent<CanvasGroup>();

        var main = Camera.main;
        _pauseCanvas.worldCamera = main;
        _settingsCanvas.worldCamera = main;
        _confirmCanvas.worldCamera = main;
    }

    private void Update()
    {
        // Toggle Pause Menu
        if (Input.GetButtonDown("PauseGame"))
        {
            if (!_isVisible)
            {
                ShowPauseMenu();
            }
            else
            {
                HideAll();
            }
        }
    }

    public void PauseReturnButton()
    {
        HideAll();
    }
    
    private void ShowPauseMenu()
    {
        _isVisible = true;
        
        _pauseCanvasGroup.alpha = 1;
        _pauseCanvasGroup.blocksRaycasts = true;
        _pauseCanvasGroup.interactable = true;
        
        _settingsCanvasGroup.alpha = 0;
        _settingsCanvasGroup.blocksRaycasts = false;
        _settingsCanvasGroup.interactable = false;
        
        _confirmCanvasGroup.alpha = 0;
        _confirmCanvasGroup.blocksRaycasts = false;
        _confirmCanvasGroup.interactable = false;
        
        Time.timeScale = 0;
    }
    
    private void HideAll()
    {
        _isVisible = false;
        
        Time.timeScale = 1;
        
        _pauseCanvasGroup.alpha = 0;
        _pauseCanvasGroup.blocksRaycasts = false;
        _pauseCanvasGroup.interactable = false;
        
        _settingsCanvasGroup.alpha = 0;
        _settingsCanvasGroup.blocksRaycasts = false;
        _settingsCanvasGroup.interactable = false;
        
        _confirmCanvasGroup.alpha = 0;
        _confirmCanvasGroup.blocksRaycasts = false;
        _confirmCanvasGroup.interactable = false;
    }
}
