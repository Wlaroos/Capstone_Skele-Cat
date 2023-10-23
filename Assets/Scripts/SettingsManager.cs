using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }
    
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

    private void Music_Volume()
    {
        //Debug.Log("MUSIC VOLUME: " + PlayerPrefs.GetFloat("Music Volume"));
    }
    
    public void SFX_Volume()
    {
        //Debug.Log("SFX VOLUME: " + PlayerPrefs.GetFloat("SFX Volume"));
    }
}
