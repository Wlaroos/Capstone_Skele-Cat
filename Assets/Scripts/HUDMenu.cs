using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDMenu : MonoBehaviour
{
    [Header("Timer")]
    private TMP_Text _hudTimerText;
    [SerializeField] private int _maxTimerCount = 10; 
    private int _timerCount = 10;
    private float _timeRemaining = 1;
    
    private TMP_Text _levelName;

    private GameObject _gridLayout;
    [SerializeField] private Sprite _catSprite;
    [SerializeField] private Sprite _skullSprite;
    private List<Image> _hpImgList = new List<Image>();
    
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxBloodHealth;
    private int _currentHealth;
    private int _currentBloodHealth;
    public int CurrentHealth { get => _currentHealth;}
    public int CurrentBloodHealth { get => _currentBloodHealth;}
    
    private PlayerController _playerRef;
    private Canvas _hudCanvas;
    
    private void Awake()
    {
        _hudTimerText = transform.GetChild(2).GetComponent<TMP_Text>();
        _levelName = transform.GetChild(4).GetComponent<TMP_Text>();
        _gridLayout = transform.GetChild(3).GetChild(0).gameObject;
        _playerRef = FindObjectOfType<PlayerController>();
        _hudCanvas = GetComponent<Canvas>();
        
        var main = Camera.main;
        _hudCanvas.worldCamera = main;
        
        _timerCount = _maxTimerCount;
        _currentHealth = _maxHealth;
        _currentBloodHealth = _maxBloodHealth;
        
        _gridLayout.GetComponent<RectTransform>().sizeDelta = new Vector2 (245, (((_currentHealth / 3) - 1) * 80) + 105);
        _gridLayout.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2 (245, (((_currentHealth / 3) - 1) * 80) + 105);

        for (int i = 1; i <= _maxHealth; i++)
        {
            GameObject obj = new GameObject("Health");
            obj.transform.SetParent(_gridLayout.transform);
            Image img = obj.AddComponent<Image>();
            
            if (i <= _maxBloodHealth)
            {
                img.sprite = _catSprite;
            }
            else
            {
                img.sprite = _skullSprite;
            }
            
            obj.transform.localScale = Vector3.one;
            img.preserveAspect = true;
            
            _hpImgList.Add(img);
        }
    }

    private void Start()
    {
        _levelName.text = ("Level: " + SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        Timer();
    }
    
    private void Timer()
    {
        // Timer decrement
        if (_timerCount >= 1 && _playerRef.IsAlive == true)
        {
            _timeRemaining -= Time.deltaTime;
        }

        // Timer runs out, player explodes
        else if (_timerCount < 1 && _playerRef.IsAlive == true)
        {
            //_musicRef.PlaySound(_tickHighSFX);
            _playerRef.Explode();
        }
        // Reset the timer and update the timer text (timeRemaining counts for 1 second, timerCount is how many seconds)
        if (_timeRemaining <= 0)
        {
            _timeRemaining = 1;
            _timerCount--;
            _hudTimerText.text = ("" + ((Mathf.Round(_timeRemaining) + _timerCount - 1)));
            // Tick sound effect and camera shake if timer isn't on the last second
            if (_timerCount >= 1)
            { 
                //_musicRef.PlaySound(_tickSFX);
                CameraShake.Instance.CamShake();
            }
        }
    }

    public void PlayerRespawned()
    {
        _timerCount = _maxTimerCount;
        _hudTimerText.text = ("" + ((Mathf.Round(_timeRemaining) + _timerCount - 1)));
    }

    public void ChangeHealth(int amount)
    {
        _hpImgList[9 - _currentHealth].color = new Color(0.5f, 0, 0, 1);
        _currentHealth = Mathf.Clamp(_currentHealth += amount, 0, _maxHealth);
        _currentBloodHealth = Mathf.Clamp(_currentBloodHealth += amount, 0, _maxBloodHealth);
    }

}