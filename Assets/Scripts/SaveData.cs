using UnityEngine;
using System.Collections.Generic;

public class SaveData : MonoBehaviour
{
    private LevelData _levelData = new LevelData();

    public void SaveToJson()
    {
        string levelData = JsonUtility.ToJson(_levelData);
        string filePath = Application.persistentDataPath + "/LevelData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, levelData);
    }
}

[System.Serializable]
public class LevelData
{
    private List<Levels> _levels = new List<Levels>();
}

[System.Serializable]
public class Levels
{
    private int _levelIndex;
    private string _levelName;
    
    private bool _isCompleted;
    
    private float _clearTime;
    private int _jumpsUsed;
    private int _livesLeft;
}