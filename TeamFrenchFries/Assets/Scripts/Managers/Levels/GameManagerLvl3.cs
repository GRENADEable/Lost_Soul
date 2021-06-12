using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLvl3 : GameManagerBase
{
    #region Public Variables
    public Collider2D endAreaCol;
    #endregion

    #region Private Variables
    [SerializeField] private bool _hitPlate1;
    [SerializeField] private bool _hitPlate2;
    [SerializeField] private bool _hitPlate3;
    [SerializeField] private bool _hitPlate4;
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

    #region My Functions
    #endregion

    #region Events
    void OnPressurePlatePressedEventReceived(int index)
    {
        if (index == 1)
            _hitPlate1 = true;

        if (index == 2 && _hitPlate1)
            _hitPlate2 = true;

        if (index == 3 && _hitPlate2)
            _hitPlate3 = true;

        if (index == 4 && _hitPlate3)
            _hitPlate4 = true;

        if (_hitPlate4)
            endAreaCol.enabled = true;
    }
    #endregion
}