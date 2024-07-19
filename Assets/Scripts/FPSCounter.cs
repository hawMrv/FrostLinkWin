using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    #region PUBLICS

    public TMP_Text _fpsCounter;
    public int _frameWindow = 60;

    #endregion

    #region PRIVATES

    private float[] _f100 = new float[100];
    private int _counter = 0;

    #endregion

    #region UNTIY_METHODS

    private void Start()
    {
        _f100 = new float[_frameWindow];

        for (int i = 0; i < _f100.Length; i++)
        {
            _f100[i] = 0f;
        }
    }

    private void Update()
    {
        _fpsCounter.text = GetFPS();
    }

    #endregion

    #region METHODS

    private string GetFPS()
    {
        _f100[_counter] = 1f / Time.deltaTime;
        _counter++;

        if (_counter >= _f100.Length) _counter = 0;

        float average = 0;

        for (int i = 0; i < _f100.Length; i++)
        {
            average += _f100[i];
        }

        average /= _f100.Length;

        return average.ToString("F0");
    }

    #endregion
}
