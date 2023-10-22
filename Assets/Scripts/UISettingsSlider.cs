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
        
        _decButton.onClick.AddListener(DecrementValue);
        _incButton.onClick.AddListener(IncrementValue);
        _slider.onValueChanged.AddListener(SliderValueChange);
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
        PlayerPrefsManager.Instance.Invoke(methodName, 0);
        
        _sliderText.text = _slider.value.ToString();
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

