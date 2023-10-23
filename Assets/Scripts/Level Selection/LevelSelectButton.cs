using TMPro;
using UnityEngine;
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

    public LevelSelectManager Manager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TMP_Text>();
    }
    
    public void Init()
    {
        _button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TMP_Text>();
        
        _buttonText.text = _levelNumber;

        this.name = (_levelNumber + " Button");
        transform.GetChild(0).name = (_levelNumber + " Background");
        transform.GetChild(0).GetChild(0).name = (_levelNumber + " Text");
        
        if (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + _levelNumber + ".unity") != -1)
        {
            //Debug.Log("Scene Found");
            _levelIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + _levelNumber + ".unity");
        }
        else
        {
            //Debug.Log("Scene NOT Found");
            _button.interactable = false;
            _levelIndex = -1;
        }
    }

    // Make it a double click? One to view, then one to actually go?
    public void GotoLevel()
    {
        if(_levelIndex != -1) {SceneManager.LoadScene(_levelIndex);}
    }

    public void Hover()
    {
        if (_button.IsInteractable())
        {
            Manager.ShowLevelData(_levelIndex - 2);
        }
    }
    
    public void UnHover()
    {
        if (_button.IsInteractable())
        {
            Manager.ShowDefaultData();
        }
    }
}
