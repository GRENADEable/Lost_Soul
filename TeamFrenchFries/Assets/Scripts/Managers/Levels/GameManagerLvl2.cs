using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLvl2 : GameManagerBase
{
    #region Public Variables
    [Space, Header("End Area")]
    public SpriteRenderer endArea;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        PlayerController.OnLevelEnded += OnLevelEndedEventReceived;
        PlayerController.OnLevel2KeyPlaced += OnLevel2KeyPlacedEventReceived;
    }

    void OnDisable()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnLevel2KeyPlaced -= OnLevel2KeyPlacedEventReceived;
    }

    void OnDestroy()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnLevel2KeyPlaced -= OnLevel2KeyPlacedEventReceived;
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
    void OnLevel2KeyPlacedEventReceived() => endArea.color = Color.green;
    #endregion
}