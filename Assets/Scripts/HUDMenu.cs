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
    [SerializeField] private int _maxHealth;
    
    private int _currentHealth;
    public int CurrentHealth { get => _currentHealth;}
    

    private PlayerController _playerRef;
    
    private void Awake()
    {
        _hudTimerText = transform.GetChild(0).GetComponent<TMP_Text>();
        _levelName = transform.GetChild(2).GetComponent<TMP_Text>();
        _playerRef = FindObjectOfType<PlayerController>();
        
        _timerCount = _maxTimerCount;
        _currentHealth = _maxHealth;
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
                // shake.CamShake();
            }
        }
    }

    public void PlayerRespawned()
    {
        _timerCount = _maxTimerCount;
        _hudTimerText.text = ("" + ((Mathf.Round(_timeRemaining) + _timerCount - 1)));
        
        ChangeHealth(0);
    }

    public void ChangeHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth += amount, 0, _maxHealth);
        
        switch (_currentHealth)
        {
            case 4:
                transform.Find("HealthArea").Find("HP4").GetComponent<Image>().enabled = true;
                transform.Find("HealthArea").Find("HP3").GetComponent<Image>().enabled = true;
                transform.Find("HealthArea").Find("HP2").GetComponent<Image>().enabled = true;
                transform.Find("HealthArea").Find("HP1").GetComponent<Image>().enabled = true;
                break;
            case 3:
                transform.Find("HealthArea").Find("HP1").GetComponent<Image>().enabled = false;
                break;
            case 2:
                transform.Find("HealthArea").Find("HP2").GetComponent<Image>().enabled = false;
                break;
            case 1:
                transform.Find("HealthArea").Find("HP3").GetComponent<Image>().enabled = false;
                break;
            case <= 0:
                transform.Find("HealthArea").Find("HP4").GetComponent<Image>().enabled = false;
                break;
        }
    }

}