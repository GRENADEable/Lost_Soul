using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLvl3 : GameManagerBase
{
    #region Public Variables
    [Space, Header("End Area")]
    public Collider2D endAreaCol;
    public SpriteRenderer endArea;
    public SpriteRenderer[] pressurePlateImg;
    #endregion

    #region Private Variables
    private bool _hitPlate1;
    private bool _hitPlate2;
    private bool _hitPlate3;
    private bool _hitPlate4;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        PlayerController.OnLevelEnded += OnLevelEndedEventReceived;
        PlayerController.OnPressurePlatePressed += OnPressurePlatePressedEventReceived;
    }

    void OnDisable()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnPressurePlatePressed -= OnPressurePlatePressedEventReceived;
    }

    void OnDestroy()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnPressurePlatePressed -= OnPressurePlatePressedEventReceived;
    }
    #endregion

    void Start() => StartCoroutine(StartGameDelay());

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gmData.currState != GameMangerData.GameState.Switch
            && gmData.currState != GameMangerData.GameState.Paused)
            StartCoroutine(SwitchDimensionDelay());

        if (Input.GetKeyDown(KeyCode.Tab) && gmData.currState == GameMangerData.GameState.Game)
            PauseGame();
    }
    #endregion

    #region Events
    void OnPressurePlatePressedEventReceived(int index)
    {
        if (index == 1)
        {
            _hitPlate1 = true;
            pressurePlateImg[0].color = Color.green;
        }

        if (index == 2 && _hitPlate1)
        {
            _hitPlate2 = true;
            pressurePlateImg[1].color = Color.green;
        }

        if (index == 3 && _hitPlate2)
        {
            _hitPlate3 = true;
            pressurePlateImg[2].color = Color.green;
        }

        if (index == 4 && _hitPlate3)
        {
            _hitPlate4 = true;
            pressurePlateImg[3].color = Color.green;
        }

        if (_hitPlate4)
        {
            endArea.color = Color.green;
            endAreaCol.enabled = true;
        }
    }
    #endregion
}