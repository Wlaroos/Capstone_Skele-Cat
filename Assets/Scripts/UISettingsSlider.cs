using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsSlider : MonoBehaviour
{
    [SerializeField] TMP_Text _keyName;
    [SerializeField] private bool _isPercent;
    [SerializeField] private int _maxValue;
    
    private Button _decButton;
    private Button _incButton;
    private Slider _slider;
    private TMP_Text _sliderText;
    
    private bool _shiftPressed;
    
    private void Awake()
    {
        _decButton = transform.GetChild(0).GetComponent<Button>();
        _slider = transform.GetChild(1).GetComponent<Slider>();
        _incButton = transform.GetChild(2).GetComponent<Button>();
        _sliderText = _slider.GetComponentInChildren<TMP_Text>();

        _slider.maxValue = _maxValue;
        
        DefaultValues();
    }

    private void Start()
    {
        SettingsManager.Instance.ReturnDefaultValues += DefaultValues;
    }

    private void OnEnable()
    {
        _decButton.onClick.AddListener(DecrementValue);
        _incButton.onClick.AddListener(IncrementValue);
        _slider.onValueChanged.AddListener(SliderValueChange);
    }

    private void OnDisable()
    {
        _decButton.onClick.RemoveListener(DecrementValue);
        _incButton.onClick.RemoveListener(IncrementValue);
        _slider.onValueChanged.RemoveListener(SliderValueChange);
        SettingsManager.Instance.ReturnDefaultValues -= DefaultValues;
    }
    
    private void DecrementValue()
    {
        if(_shiftPressed) {_slider.value -= 5;}
        else{_slider.value -= 1;}
    }
    
    private void IncrementValue()
    {
        if(_shiftPressed) {_slider.value += 5;}
        else{_slider.value += 1;}
    }

    public void SliderValueChange(float value)
    {
        PlayerPrefs.SetFloat(_keyName.text, value);
        
        PlayerPrefs.Save();
        
        string methodName = _keyName.text.Replace(" ", "_");
        SettingsManager.Instance.Invoke(methodName, 0);

        UpdateText();
    }
    
    public void DefaultValues()
    {
        _slider.value = PlayerPrefs.GetFloat(_keyName.text);

        UpdateText();
    }

    private void UpdateText()
    {
        if (!_isPercent) _sliderText.text = _slider.value.ToString();
        else _sliderText.text = ((float)_slider.value / 10f).ToString("F1");
    }
    
    private void OnGUI()
    {
        Event e = Event.current;
        if (e.shift)
        {
            _shiftPressed = true;
        }
        else
        {
            _shiftPressed = false;
        }
    }
}

