using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelListSO", menuName = "LevelList")]
public class LevelList : ScriptableObject
{

    public List<LevelNames> _levelNameList = new List<LevelNames>();

}

[Serializable]
public class LevelNames
{
    public string _levelName;
    [TextArea(3,10)]
    public string _levelDescription;
}