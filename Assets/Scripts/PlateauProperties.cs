/* PlateauProperties
 * 
 * Defines properties of Plateau, such as:
 * - is a player standing on
 * - is field A or B
 * - is hole
 * - is "broken Down"
 * 
 * @mrv 24
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateauProperties : MonoBehaviour
{
    #region PUBLICS

    public GameObject _MOTHER;
    public FieldGenerator _FieldGenerator;

    [Space(10)]

    public GameObject _HoleIndicator;
    public GameObject _PlayerIndicator;

    [Space(10)]

    public Material[] _materialPlayerIndicator = new Material[2];

    [Space(10)]

    public GameObject[] _PlateauModel = new GameObject[3];

    #endregion

    #region PRIVATES

    [SerializeField]
    private bool _isHole = false;
    [SerializeField]
    private int _playerType = 0;
    
    private float _playerIndicatorHeightOffset = 0.15f;
    private float _playerIndicatorRiseTime = 0.500f;
    private float _playerIndicatorDiveTime = 5.000f;
    private bool _indicating = false;
    private bool _indicatorRising = false;
    private bool _indicatorDiving = false;

    private float _plateauDropTime = 5.0f;

    private int[] _ID = new int[2];

    private bool _plateauSurfaced = false;

    #endregion

    #region UNITY_METHODS

    public void OnMouseDown()
    {
        //Debug.Log("Clicked: " + this.name, this);
        //Debug.Log("_PlayerIndicator.transform.up * _playerIndicatorHeightOffset is " + _PlayerIndicator.transform.up * _playerIndicatorHeightOffset, this);
        //PlayerSteppedOnPlateau();

        ReportPlayerStepToFieldGenerator();
    }

    private void Start()
    {
        _MOTHER = GameObject.FindGameObjectWithTag("MOTHER");
        _FieldGenerator = GameObject.FindGameObjectWithTag("MOTHER").GetComponent<FieldGenerator>();
    }

    #endregion

    #region METHODS

    public int[] ID()
    {
        return _ID;
    }

    public void ID(int x, int y)
    {
        _ID[0] = x;
        _ID[1] = y;
    }

    public bool IsHole()
    {
        return _isHole;
    }

    public void IsHole(bool isHole)
    {
        _isHole = isHole;

        if (_isHole)
        {
            _HoleIndicator.gameObject.SetActive(true);
        }
        else
        {
            _HoleIndicator.gameObject.SetActive(false);
        }
    }

    public void ShowHoleVisibility(bool show)
    {
        _HoleIndicator.gameObject.SetActive(show && _isHole);
    }

    public void EnablePlateauModel(int model)
    {
        for (int i = 0; i < _PlateauModel.Length; i++)
        {
            _PlateauModel[i].SetActive((i == model));
        }
    }

    public void PlayerType(int playerType)
    {
        _playerType = playerType;
        _PlayerIndicator.GetComponent<MeshRenderer>().material = _materialPlayerIndicator[_playerType];
    }

    private void ReportPlayerStepToFieldGenerator()
    {
        _FieldGenerator.MirrorPlayerStep(_playerType, _ID[0], _ID[1]);
    }

    public void PlayerSteppedOnPlateau()
    {
        TriggerPlayerIndicator();
    }

    private void TriggerPlayerIndicator()
    {
        if (!_indicating)
        {
            StartCoroutine(RisePlayerIndicator());
        }
        else
        {
            _indicatorRising = true;
            _indicatorDiving = false;
        }
    }

    private IEnumerator RisePlayerIndicator()
    {
        //Debug.Log("Starting to Indicate Player", this);

        _indicating = true;
        _indicatorRising = true;
        _indicatorDiving = false;

        while (_indicating)
        {
            if (_indicatorRising)
            {
                //Debug.Log("PlayerIndicator is rising...", this);

                _PlayerIndicator.transform.localPosition += (_PlayerIndicator.transform.up * _playerIndicatorHeightOffset) / (_playerIndicatorRiseTime / Time.deltaTime);

                if (_PlayerIndicator.transform.localPosition.y >= 0f)
                {
                    _PlayerIndicator.transform.localPosition = new Vector3(_PlayerIndicator.transform.localPosition.x, 0f, _PlayerIndicator.transform.localPosition.z);
                    _indicatorRising = false;
                    _indicatorDiving = true;
                }
            }
            else if (_indicatorDiving)
            {
                //Debug.Log("PlayerIndicator is diving...", this);

                _PlayerIndicator.transform.localPosition -= (_PlayerIndicator.transform.up * _playerIndicatorHeightOffset) / (_playerIndicatorDiveTime / Time.deltaTime);

                if (_PlayerIndicator.transform.localPosition.y <= (_playerIndicatorHeightOffset * -1f))
                {
                    _PlayerIndicator.transform.localPosition = new Vector3(_PlayerIndicator.transform.localPosition.x, _playerIndicatorHeightOffset * -1f, _PlayerIndicator.transform.localPosition.z);
                    _indicatorRising = false;
                    _indicatorDiving = false;
                    _indicating = false;
                }
            }       

            yield return null;
        }

        //Debug.Log("PlayerIndicator is done indicating!", this);
    }

    public void TriggerHole()
    {
        if (!_isHole) return;

        //Debug.Log("DROPPING PLATEAU... ", this);
        StartCoroutine(DropPlateauToDeath());
    }

    private IEnumerator DropPlateauToDeath()
    {
        Vector3 thisPos = this.transform.position;

        while (this.transform.position.y >= -.5f)
        {
            this.transform.position -= .5f * this.transform.up / (_plateauDropTime / Time.deltaTime);

            yield return null;
        }

        //this.gameObject.SetActive(false);
    }

    public void Surface()
    {
        if (_plateauSurfaced) return;
        StartCoroutine(SurfacePlateau());
    }

    private IEnumerator SurfacePlateau()
    {
        //Debug.Log("Plateau is surfacing.. @ " + _ID[0] + " " + _ID[1], this);

        while (this.transform.position.y <= 0f)
        {
            this.transform.position += this.transform.up * Time.deltaTime;

            yield return null;
        }

        _plateauSurfaced = true;
    }

    public void Submerge()
    {
        StartCoroutine(SubmergePlateau());
    }

    private IEnumerator SubmergePlateau()
    {
        while (this.transform.position.y >= -1)
        {
            this.transform.position -= (this.transform.up * _playerIndicatorHeightOffset) / (_playerIndicatorDiveTime / Time.deltaTime);

            yield return null;
        }

        this.gameObject.SetActive(false);
    }

    #endregion
}
