using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class SettingsSaver : MonoBehaviour
{
    #region PUBLICS

    public FieldGenerator _FieldGenerator;

    [Space(10)]

    public Slider _SldrFieldWidth;
    public Slider _SldrFieldHeight;
    public Slider _SldrPlateauRotation;
    public Slider _SldrPlateauSize;
    public Slider _SldrHoleChance;
    public Slider _SldrPlateauSpacing;

    #endregion

    #region PRIVATES

    private FrostLinkSettings _Settings = new FrostLinkSettings();

    private string _savePath = "";
    private string _fileName = "frostlink_settings.txt";

    #endregion

    #region UNITY_METHODS

    private void Start()
    {
        GetPathsAndSettingsFile();
    }

    #endregion

    #region METHODS

    private void GetPathsAndSettingsFile()
    {
        _savePath = Application.dataPath;
        _savePath += "/" + _fileName;

        if (!File.Exists(_savePath))
        {
            SaveDefaults();
            LoadSettings();
        }
        else
        {
            LoadSettings();
        }
    }

    private void WriteSettingsToFile(string data)
    {
        FileStream fileStream = new FileStream(_savePath, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream)) 
        { 
            writer.Write(data); 
        }
    }

    private void SaveDefaults()
    {
        _Settings = new FrostLinkSettings();
        string settings = JsonUtility.ToJson(_Settings);
        WriteSettingsToFile(settings);
    }

    public void SaveSettings()
    {
        _Settings = new FrostLinkSettings();
        _Settings._fieldWidth = _FieldGenerator.FieldWidth();
        _Settings._fieldHeight = _FieldGenerator.FieldHeight();
        _Settings._plateauRotation = _FieldGenerator.PlateauRotation();
        _Settings._plateauSize = _FieldGenerator.PlateauSize();
        _Settings._holeChance = _FieldGenerator.HoleChance();
        _Settings._plateauSpacing = _FieldGenerator.PlateauSpacing();

        string settings = JsonUtility.ToJson(_Settings);
        WriteSettingsToFile(settings);
    }

    public void LoadSettings()
    {
        JsonUtility.FromJsonOverwrite(ReadSettingsFromFile(), _Settings);

        _SldrFieldWidth.value = _Settings._fieldWidth;
        _SldrFieldHeight.value = _Settings._fieldHeight;
        _SldrPlateauRotation.value = _Settings._plateauRotation;
        _SldrPlateauSize.value = _Settings._plateauSize;
        _SldrHoleChance.value = _Settings._holeChance;
        _SldrPlateauSpacing.value = _Settings._plateauSpacing;
    }

    private string ReadSettingsFromFile()
    {
        using (StreamReader reader = new StreamReader(_savePath))
        {
            string json = reader.ReadToEnd();
            return json;
        }
    }

    #endregion
}

[Serializable]
public class FrostLinkSettings
{
    public int _fieldWidth = 5;
    public int _fieldHeight = 10;
    public int _plateauRotation = 0;
    public float _plateauSize = 1f;
    public int _holeChance = 0;
    public float _plateauSpacing = .3f;
}
