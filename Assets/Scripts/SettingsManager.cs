using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    [SerializeField] private const int DefaultMusicVolume = 20;
    [SerializeField] private const int DefaultSfxVolume = 20;
    [SerializeField] private const int DefaultScreenShake = 10;
    
    public event Action ReturnDefaultValues;
    
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

        if (!PlayerPrefs.HasKey("Music Volume"))
        {
            DefaultValues();
        }
    }

    private void Update()
    {
        Debug.Log(PlayerPrefs.GetFloat("Music Volume") + "---" + PlayerPrefs.GetFloat("SFX Volume") + "---" + PlayerPrefs.GetFloat("Screen Shake"));
    }

    public void DefaultValues()
    {
        PlayerPrefs.SetFloat("Music Volume", DefaultMusicVolume);
        PlayerPrefs.SetFloat("SFX Volume", DefaultSfxVolume);
        PlayerPrefs.SetFloat("Screen Shake", DefaultScreenShake);
        
        PlayerPrefs.Save();
        
        ReturnDefaultValues?.Invoke();
    }
    
    public void Music_Volume()
    {
        //Debug.Log("MUSIC VOLUME: " + PlayerPrefs.GetFloat("Music Volume"));
    }
    
    public void SFX_Volume()
    {
        //Debug.Log("SFX VOLUME: " + PlayerPrefs.GetFloat("SFX Volume"));
    }
    
    public void Screen_Shake()
    {
        //Debug.Log("SCREEN SHAKE: " + PlayerPrefs.GetFloat("Screen Shake"));
    }
}
