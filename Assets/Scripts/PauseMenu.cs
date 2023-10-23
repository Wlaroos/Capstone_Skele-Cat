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
    [SerializeField] private Button _pauseExitButton;
    
    [SerializeField] private Button _clearDataConfirmButton;

    private bool _isVisible = false;

    private void OnEnable()
    {
        _pauseReturnButton.onClick.AddListener(PauseReturnButton);
        _pauseRestartButton.onClick.AddListener(PauseRestartButton);
        _pauseExitButton.onClick.AddListener(PauseExitButton);
        _clearDataConfirmButton.onClick.AddListener(ClearDataButton);
    }
    private void OnDisable()
    {
        _pauseReturnButton.onClick.RemoveListener(PauseReturnButton);
        _pauseRestartButton.onClick.RemoveListener(PauseRestartButton);
        _pauseExitButton.onClick.RemoveListener(PauseExitButton);
        _clearDataConfirmButton.onClick.RemoveListener(ClearDataButton);
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
    
    public void PauseRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void PauseExitButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ClearDataButton()
    {
        SaveData.Instance.ClearJson();
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
