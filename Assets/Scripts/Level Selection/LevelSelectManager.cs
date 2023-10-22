using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Level Initialization")]
    [SerializeField] private GameObject _verticalLayoutPrefab;
    [SerializeField] private GameObject _buttonPrefab;
    
    [SerializeField] [Range(1, 8)] private int _numOfStages;
    [SerializeField] [Range(1, 8)] private int _levelsPerStage;
    
    private HorizontalLayoutGroup _horizontalLayout;
    private VerticalLayoutGroup _verticalLayout;
    private TMP_Text _stageNumberText;

    private List<GameObject> _vLayoutList = new List<GameObject>();
    private List<GameObject> _buttonList = new List<GameObject>();
    private List<String> _scenes = new List<String>();

    [Header("Text Refs")]
    [SerializeField] private TMP_Text _levelNumberText;
    [SerializeField] private TMP_Text _levelNameText;
    [SerializeField] private TMP_Text _timeCompletedText;
    [SerializeField] private TMP_Text _livesLeftText;
    [SerializeField] private TMP_Text _jumpsUsedText;
    [SerializeField] private TMP_Text _rankText;

    private LevelList _levelList;

    private void Awake()
    {
        _horizontalLayout = GetComponent<HorizontalLayoutGroup>();
        _levelList = (LevelList)Resources.Load("LevelListSO");
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        _buttonList.Clear();
        _vLayoutList.Clear();
        
        // Make i The Index of the Builds to Start At
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        for( int i = 2; i < sceneCount; i++ )
        {
            _scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility
                .GetScenePathByBuildIndex(i)));
        }
        
        Init();
    }

    private void Init()
    {
        int num = 0;

        //for (int i = 1; i < (Mathf.Ceil((float)_scenes.Count / (float)_levelsPerStage)) + 1; i++)
        for (int i = 1; i < (_numOfStages) + 1; i++)
        {

            GameObject _stage = Instantiate(_verticalLayoutPrefab, transform.position, Quaternion.identity,
                transform);
            _stage.GetComponentInChildren<TMP_Text>().text = ("[ " + i + " ]");
            _vLayoutList.Add(_stage);

            for (int j = 0; j < _levelsPerStage; j++)
            {
                GameObject _button = Instantiate(_buttonPrefab, transform.position, Quaternion.identity, _stage.transform);

                if (num < _scenes.Count) _button.GetComponent<LevelSelectButton>().LevelNumber = (_scenes[num]);
                else _button.GetComponent<LevelSelectButton>().LevelNumber = ("N/A");

                _button.GetComponent<LevelSelectButton>().Init();
                _button.GetComponent<LevelSelectButton>().Manager = this;
                _buttonList.Add(_button);

                num++;
            }

        }
    }

    public void ShowLevelData(int index)
    {
        _levelNumberText.text = SaveData.Instance._levelData._levels[index]._levelNumber;
        _levelNameText.text = _levelList._levelNameList[index]._levelName;
        _timeCompletedText.text = SaveData.Instance._levelData._levels[index]._clearTime.ToString();
        _livesLeftText.text = SaveData.Instance._levelData._levels[index]._livesLeft.ToString();
        _jumpsUsedText.text = SaveData.Instance._levelData._levels[index]._jumpsUsed.ToString();
        _rankText.text = SaveData.Instance._levelData._levels[index]._rank;
    }
    
    public void ShowDefaultData()
    {
        _levelNumberText.text = "";
        _levelNameText.text = "";
        _timeCompletedText.text = "N/A";
        _livesLeftText.text = "N/A";
        _jumpsUsedText.text = "N/A";
        _rankText.text = "N/A";
    }
}
