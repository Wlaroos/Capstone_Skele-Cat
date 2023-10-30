using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] string _levelNumber;
    public string LevelNumber { get => _levelNumber; set => _levelNumber = value; }

    [SerializeField] string _levelName;
    public string LevelName { get => _levelName;}

    [SerializeField] int _levelIndex;
    public int LevelIndex { get => _levelIndex; }

    private Button _button;
    private TMP_Text _buttonText;

    private bool _isUnlocked = false;

    public LevelSelectManager Manager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TMP_Text>();
        
        _buttonText.text = _levelNumber;
        
        this.name = (_levelNumber + " Button");
        transform.GetChild(0).name = (_levelNumber + " Background");
        transform.GetChild(0).GetChild(0).name = (_levelNumber + " Text");

        // Checks to See if the Level Index has a Corresponding Scene
        if (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + _levelNumber + ".unity") != -1)
        {
            //Debug.Log("Scene Found");
            _levelIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + _levelNumber + ".unity");
            
            // If Level Index is Within List of Levels
            if (_levelIndex - 2 < SaveData.Instance._levelData._levels.Count)
            {
                ColorBlock cb = _button.colors;
                
                if (_levelIndex - 2 != 0)
                {
                    // If Previous Level is Completed, Unlock This Level
                    if (SaveData.Instance._levelData._levels[_levelIndex - 3]._isCompleted)
                    {
                        _isUnlocked = true;
                        
                        // If This Level is Completed, Show Green
                        if (SaveData.Instance._levelData._levels[_levelIndex - 2]._isCompleted)
                        {
                            cb.normalColor = Color.green;
                            cb.highlightedColor = Color.cyan;
                            cb.pressedColor = new Color32(0, 130, 125, 255);
                            cb.selectedColor = new Color32(0, 130, 125, 255);
                            cb.disabledColor = new Color32(20, 0, 0, 150);
                            _button.colors = cb;
                        }
                        // If This Level is NOT Completed, Show Red
                        else
                        {
                            cb.normalColor = Color.red;
                            cb.highlightedColor = Color.cyan;
                            cb.pressedColor = new Color32(0, 130, 125, 255);
                            cb.selectedColor = new Color32(0, 130, 125, 255);
                            cb.disabledColor = new Color32(20, 0, 0, 150);
                            _button.colors = cb;
                        }
                    }
                    // If Previous Level is NOT Completed, LOCK This Level. Darker Color and Can't Goto Level
                    else
                    {
                        _isUnlocked = false;
                        cb.normalColor = new Color32(20, 0, 0, 150);
                        cb.highlightedColor = new Color32(0, 130, 125, 50);
                        cb.pressedColor = new Color32(20, 0, 0, 150);
                        cb.selectedColor = new Color32(20, 0, 0, 150);
                        cb.disabledColor = new Color32(20, 0, 0, 150);
                        _button.colors = cb;
                    }
                }
                // First Level is Always Unlocked
                else
                {
                    _isUnlocked = true;
                    cb.normalColor = Color.green;
                    cb.highlightedColor = Color.cyan;
                    cb.pressedColor = new Color32(0, 130, 125, 255);
                    cb.selectedColor = new Color32(0, 130, 125, 255);
                    cb.disabledColor = new Color32(20, 0, 0, 150);
                    _button.colors = cb;
                }
            }
        }
        else
        {
            //Debug.Log("Scene NOT Found");
            _button.interactable = false;
            _levelIndex = -1;
            
            ColorBlock cb = _button.colors;
            cb.disabledColor = new Color32(25, 25, 25, 150);
            _button.colors = cb;
        }
    }

    // Make it a double click? One to view, then one to actually go?
    public void GotoLevel()
    {
        if (_levelIndex != -1 && _isUnlocked)
        {
            SceneManager.LoadScene(_levelIndex);
        }
    }

    public void Hover()
    {
        if (_button.interactable)
        {
            Manager.ShowLevelData(_levelIndex - 2);
        }
    }
    
    public void UnHover()
    {
        if (_button.interactable)
        {
            Manager.ShowDefaultData();
        }

        // Makes sure it doesn't stay pressed
        if (!_isUnlocked)
        {
            EventSystem.current.SetSelectedGameObject(null); 
        }
    }
}
