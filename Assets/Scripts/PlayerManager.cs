using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region PUBLICS

    public FieldGenerator _FieldGenerator;

    [Space(10)]

    public Text _TXTPlayerLives;
    public Text _TXTGameState;

    #endregion

    #region PRIVATES

    private int _playerLives = 3;

    private bool _isGameOn = false;
    private bool _playerAAlive = false;

    #endregion

    #region UNITY_METHODS

    private void Start()
    {
        _TXTGameState.color = Color.red;
        _TXTGameState.text = "";
    }

    #endregion

    #region METHODS

    public void ResetGame()
    {
        _playerLives = 3;

        _TXTGameState.text = "IDLE";
        _TXTGameState.color = Color.white;
        _FieldGenerator.RemoveAllPlateaus();

        _isGameOn = false;
    }

    public void StopGame()
    {
        if (!_isGameOn) return;

        _isGameOn = false;

        _TXTGameState.text = "GAME OVER";
        _TXTGameState.color = Color.red;

        _FieldGenerator.SubmergeAllPlateaus();
    }

    public void GameWon()
    {
        _isGameOn = false;

        _TXTGameState.text = "YOU'RE WINNER";
        _TXTGameState.color = Color.green;
    }

    public void StartGame()
    {
        if (_isGameOn) return;

        _playerLives = 3;

        _TXTGameState.text = "GAME ON";
        _TXTGameState.color = Color.green;

        _FieldGenerator.GenerateField();
        _FieldGenerator.MirrorPlateauA(true);

        _isGameOn = true;  
    }

    public void PlayerSteppedOnHole(int type)
    {
        if (type == 1)
        {
            StopGame();
            return;
        }
        else if (type == 0)
        {
            _playerLives--;
            _TXTPlayerLives.text = "";

            for (int i = 0; i < _playerLives; i++) _TXTPlayerLives.text += "X";

            if (_playerLives == 0)
            {
                _FieldGenerator.SubmergePlateausA();
                _FieldGenerator.MirrorPlateauA(false);
            }
        }
    }

    #endregion
}
