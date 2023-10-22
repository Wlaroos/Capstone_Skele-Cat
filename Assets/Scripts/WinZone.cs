using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private float _levelClearTime = 0;
    private int _levelJumpsUsed = 0;
    private int _levelLivesLeft = 0;

    private bool _isComplete;
    
    public static WinZone Instance { get; set; }
    
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

    private void Update()
    {
        if(!_isComplete) _levelClearTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController playerController = other.GetComponent<PlayerController>();
        
        if (playerController != null && playerController.IsAlive)
        {
            Debug.Log("Level Complete");
            _isComplete = true;
            
            playerController.NextLevel();
            _levelLivesLeft = playerController.GetLivesLeft();
            
            SetLevelData();
            
            Invoke(nameof(NextScene), 1f);
            //AudioHelper.PlayClip2D(_winSFX01, 1f);
        }
    }
    
    public void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    // Called by the PlayerController script everytime they jump
    public void IncrementJumps()
    {
        _levelJumpsUsed++;
    }
    
    private void SetLevelData()
    {
        // Build index is minus 2 because the build index is offset from the index of the level list.
        // It's -2 because there are two scenes in front of all the level in the build order.
        int buildIndex = SceneManager.GetActiveScene().buildIndex - 2;
        
        // If level wasn't completed, mark as completed
        if (!SaveData.Instance._levelData._levels[buildIndex]._isCompleted)
        {
            SaveData.Instance._levelData._levels[buildIndex]._isCompleted = true;
        }

        // Currently only updates data if the clear time was better than the previous one.
        if (SaveData.Instance._levelData._levels[buildIndex]._clearTime == -1f)
        {
            SaveData.Instance._levelData._levels[buildIndex]._clearTime = _levelClearTime;
            SaveData.Instance._levelData._levels[buildIndex]._jumpsUsed = _levelJumpsUsed;
            SaveData.Instance._levelData._levels[buildIndex]._livesLeft = _levelLivesLeft;
        }
        else if (_levelClearTime < SaveData.Instance._levelData._levels[buildIndex]._clearTime)
        {
            SaveData.Instance._levelData._levels[buildIndex]._clearTime = _levelClearTime;
            SaveData.Instance._levelData._levels[buildIndex]._jumpsUsed = _levelJumpsUsed;
            SaveData.Instance._levelData._levels[buildIndex]._livesLeft = _levelLivesLeft;
        }

        SaveData.Instance.SaveToJson();
    }
}
