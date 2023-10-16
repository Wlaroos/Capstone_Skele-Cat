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

    private void Awake()
    {
        _horizontalLayout = GetComponent<HorizontalLayoutGroup>();
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        _buttonList.Clear();
        _vLayoutList.Clear();
        
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        for( int i = 1; i < sceneCount; i++ )
        {
            _scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility
                .GetScenePathByBuildIndex(i)));
        }
        
        Init();
    }

    private void Init()
    {
        Debug.Log(_scenes.Count + "--" + _levelsPerStage);
        Debug.Log(Mathf.Ceil((float)_scenes.Count / (float)_levelsPerStage));
        for (int i = 1; i < (Mathf.Ceil((float)_scenes.Count / (float)_levelsPerStage)) + 1; i++)
        {
            GameObject _stage = Instantiate(_verticalLayoutPrefab, transform.position, Quaternion.identity,
                transform);
            _stage.GetComponentInChildren<TMP_Text>().text = ("[ " + i + " ]");
            _vLayoutList.Add(_stage);

            if (_scenes.Count < _levelsPerStage)
            {
                for (int j = 1; j < _scenes.Count + 1; j++)
                {

                    GameObject _button = Instantiate(_buttonPrefab, transform.position, Quaternion.identity,
                        _stage.transform);
                    _button.GetComponent<LevelSelectButton>().LevelNumber = (i + "-" + j);
                    _button.GetComponent<LevelSelectButton>().Init();
                    _buttonList.Add(_button);

                }
            }
            else
            {
                for (int j = 1; j < _levelsPerStage + 1; j++)
                {

                    GameObject _button = Instantiate(_buttonPrefab, transform.position, Quaternion.identity,
                        _stage.transform);
                    _button.GetComponent<LevelSelectButton>().LevelNumber = (i + "-" + j);
                    _button.GetComponent<LevelSelectButton>().Init();
                    _buttonList.Add(_button);

                }
            }
        }
    }
}
