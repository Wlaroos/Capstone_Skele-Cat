using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsSlider : MonoBehaviour
{
    [SerializeField] TMP_Text _keyName;
    [SerializeField] [Range(0,100)] private int _initialSliderValue = 20;
    
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
    }

    private void OnEnable()
    {
        _decButton.onClick.AddListener(DecrementValue);
        _incButton.onClick.AddListener(IncrementValue);
        _slider.onValueChanged.AddListener(SliderValueChange);
        SettingsManager.Instance._defaultValuesButton.onClick.AddListener(DefaultValues);
    }

    private void OnDisable()
    {
        _decButton.onClick.RemoveListener(DecrementValue);
        _incButton.onClick.RemoveListener(IncrementValue);
        _slider.onValueChanged.RemoveListener(SliderValueChange);
        SettingsManager.Instance._defaultValuesButton.onClick.RemoveListener(DefaultValues);
    }

    private void Start()
    {
        _slider.value = _initialSliderValue;
        _sliderText.text = _initialSliderValue.ToString();
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
        
        string methodName = _keyName.text.Replace(" ", "_");
        SettingsManager.Instance.Invoke(methodName, 0);
        
        _sliderText.text = _slider.value.ToString();
    }
    
    public void DefaultValues()
    {
        _slider.value = PlayerPrefs.GetFloat(_keyName.text);
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

