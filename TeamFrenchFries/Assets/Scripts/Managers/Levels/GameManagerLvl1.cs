using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLvl1 : GameManagerBase
{
    #region Unity Callbacks

    #region Events
    void OnEnable() => PlayerController.OnLevelEnded += OnLevelEndedEventReceived;

    void OnDisable() => PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;

    void OnDestroy() => PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
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

        if (Input.GetKeyDown(KeyCode.Tab) && gmData.currState == GameMangerData.GameState.Game)
            PauseGame();
    }
    #endregion
}