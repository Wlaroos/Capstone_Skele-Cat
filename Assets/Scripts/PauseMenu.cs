using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _confirmCanvas;
    
    [SerializeField] private Button _pauseReturnButton;
    [SerializeField] private Button _pauseRestartButton;
    [SerializeField] private Button _pauseExitButton;

    private void OnEnable()
    {
        _pauseReturnButton.onClick.AddListener(PauseReturnButton);
        _pauseRestartButton.onClick.AddListener(PauseRestartButton);
        _pauseExitButton.onClick.AddListener(PauseExitButton);
    }
    private void OnDisable()
    {
        _pauseReturnButton.onClick.RemoveListener(PauseReturnButton);
        _pauseRestartButton.onClick.RemoveListener(PauseRestartButton);
        _pauseExitButton.onClick.RemoveListener(PauseExitButton);
        Time.timeScale = 1;
    }

    private void Awake()
    {
        _pauseCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _settingsCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _confirmCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        // Toggle Pause Menu
        if (Input.GetButtonDown("PauseGame"))
        {
            if (_pauseCanvas.GetComponent<CanvasGroup>().interactable == false)
            {
                _pauseCanvas.GetComponent<CanvasGroup>().alpha = 1;
                _pauseCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
                _pauseCanvas.GetComponent<CanvasGroup>().interactable = true;
                Time.timeScale = 0;
            }
            else
            {
                _pauseCanvas.GetComponent<CanvasGroup>().alpha = 0;
                _pauseCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
                _pauseCanvas.GetComponent<CanvasGroup>().interactable = false;
                Time.timeScale = 1;
            }
        }
    }

    public void PauseReturnButton()
    {
        Time.timeScale = 1;
        _pauseCanvas.GetComponent<CanvasGroup>().alpha = 0;
        _pauseCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _pauseCanvas.GetComponent<CanvasGroup>().interactable = false;
    }
    
    public void PauseRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void PauseExitButton()
    {
        SceneManager.LoadScene(0);
    }
}
