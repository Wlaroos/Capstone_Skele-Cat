using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    public LevelData _levelData = new LevelData();
    private List<String> _scenes = new List<String>();

    private string _filePath;
    
    public static SaveData Instance { get; set; }
    
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
        
        _filePath = Application.persistentDataPath + "/LevelData.json";
    }
    private void Start()
    {
        // Make i The Index of the Builds to Start At
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        for( int i = 2; i < sceneCount; i++ )
        {
            _scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility
                .GetScenePathByBuildIndex(i)));
        }

        // If file exists, use that. If not, initialize with default values.
        if (File.Exists(_filePath))
        {
            LoadFromJson();
        }
        else
        {
            Init();
        }
        
    }

    public void SaveToJson()
    {
        string levelData = JsonUtility.ToJson(_levelData);
        Debug.Log(_filePath);
        System.IO.File.WriteAllText(_filePath, levelData);
        Debug.Log("SAVED DATA");
    }

    public void LoadFromJson()
    {
        string levelData = System.IO.File.ReadAllText(_filePath);

        _levelData = JsonUtility.FromJson<LevelData>(levelData);
        Debug.Log("LOADED DATA");
    }

    public void ClearJson()
    {
        // check if file exists
        if ( !File.Exists( _filePath ) )
        {
            Debug.Log( "No save file exists");
        }
        else
        {
            Debug.Log("File exists, deleting...");
			
            File.Delete( _filePath );
            
            _levelData._levels.Clear();
            Init();
        }
    }

    private void Init()
    {
        for (int i = 2; i < _scenes.Count + 2; i++)
        {
            Levels lev = new Levels();
            lev._levelIndex = i;
            lev._levelNumber = NameFromIndex(i);
            lev._levelName = "LEVEL NAME";
            lev._isCompleted = false;
            lev._clearTime = -1;
            lev._jumpsUsed = -1;
            lev._livesLeft = -1;
            lev._rank = "N/A";
            _levelData._levels.Add(lev);
        }
    }
    
    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
}

[System.Serializable]
public class LevelData
{
    public List<Levels> _levels = new List<Levels>();
}

[System.Serializable]
public class Levels
{
    public int _levelIndex;
    public string _levelNumber;
    public string _levelName;
    
    public bool _isCompleted;
    
    public float _clearTime;
    public int _jumpsUsed;
    public int _livesLeft;

    public string _rank;
}
