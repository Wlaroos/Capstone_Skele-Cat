using UnityEngine;
using System.Collections.Generic;

public class SaveData : MonoBehaviour
{
    private LevelData _levelData = new LevelData();
}

[System.Serializable]
public class LevelData
{
    private int _levelIndex;
    private bool _completed;
    private bool _collectable;
    private float _clearTime;
    private int _jumpsUsed;
    private int _livesLeft;

    private List<Test> _test = new List<Test>();
}

[System.Serializable]
public class Test
{
    private string _name;
    private string _desc;
}