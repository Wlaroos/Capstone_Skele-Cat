using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDontDestroy : MonoBehaviour
{
    public static PrefabDontDestroy Instance { get; set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
}
