using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLvl1 : GameManagerBase
{
    #region Public Variables
    public delegate void SendEvents();
    public static event SendEvents OnDimensionChanged;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        PlayerController.OnLevelEnded += OnLevelEndedEventReceived;
        PlayerController.OnVoidDeath += OnVoidDeathEventReceived;
    }

    void OnDisable()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnVoidDeath -= OnVoidDeathEventReceived;
    }

    void OnDestroy()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnVoidDeath -= OnVoidDeathEventReceived;
    }
    #endregion

    void Start()
    {
        StartCoroutine(StartGameDelay());
        HumanDimensionAudio(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gmData.currState != GameMangerData.GameState.Switch
            && gmData.currState != GameMangerData.GameState.Paused)
            StartCoroutine(SwitchDimensionDelay());

        if (Input.GetKeyDown(KeyCode.Escape) && gmData.currState == GameMangerData.GameState.Game)
            PauseGame();
    }
    #endregion

    #region Events
    void OnVoidDeathEventReceived() => StartCoroutine(DeathScreenDelay());
    #endregion
}