using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateSliderValue : MonoBehaviour
{
    #region PUBLICS

    public Slider _slider;
    public TMP_Text _sliderValue;

    #endregion

    #region PRIVATES



    #endregion

    #region UNTIY_METHODS

    private void Start()
    {
        UpdateThisSliderValue();
    }

    #endregion

    #region METHODS

    public void UpdateThisSliderValue()
    {
        if (_slider.wholeNumbers)
        {
            _sliderValue.text = _slider.value.ToString("F0");
        }
        else
        {
            _sliderValue.text = _slider.value.ToString("F1");
        }
    }

    #endregion
}