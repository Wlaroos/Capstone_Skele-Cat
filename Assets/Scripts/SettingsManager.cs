using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    private const int DefaultMusicVolume = 20;
    private const int DefaultSfxVolume = 20;

    public Button _defaultValuesButton;
    
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
        
        DefaultValues();
    }
    
    private void OnEnable()
    {
        _defaultValuesButton = GameObject.Find("DefaultValuesButton").GetComponent<Button>();
        _defaultValuesButton.onClick.AddListener(DefaultValues);
    }
    private void OnDisable()
    {
        _defaultValuesButton.onClick.RemoveListener(DefaultValues);
    }

    public void DefaultValues()
    {
        PlayerPrefs.SetFloat("Music Volume", DefaultMusicVolume);
        PlayerPrefs.SetFloat("SFX Volume", DefaultSfxVolume);
    }
    
    public void Music_Volume()
    {
        //Debug.Log("MUSIC VOLUME: " + PlayerPrefs.GetFloat("Music Volume"));
    }
    
    public void SFX_Volume()
    {
        //Debug.Log("SFX VOLUME: " + PlayerPrefs.GetFloat("SFX Volume"));
    }
}
