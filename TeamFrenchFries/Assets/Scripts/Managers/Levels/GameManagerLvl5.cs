using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManagerLvl5 : GameManagerBase
{
    #region Public Variables
    public PlayableDirector endOutroTimeline;
    #endregion

    #region Private Variables
    private bool _canExit = true;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable() => PlayerController.OnGameEnd += OnGameEndEventReceived;

    void OnDisable() => PlayerController.OnGameEnd -= OnGameEndEventReceived;

    void OnDestroy() => PlayerController.OnGameEnd -= OnGameEndEventReceived;
    #endregion

    void Start()
    {
        StartCoroutine(StartGameDelay());
        SpiritDimensionAudio(true);
    }

#if UNITY_STANDALONE
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gmData.currState == GameMangerData.GameState.Game && _canExit)
            PauseGame();
    }
#endif

    #endregion

    #region Events
    public void OnShhStarted()
    {
        SpiritDimensionAudio(false);
        gmData.ChangeState("End");
    }

    void OnGameEndEventReceived()
    {
        _canExit = false;
        pauseButton.SetActive(false);
        endOutroTimeline.Play();
    }
    #endregion
}