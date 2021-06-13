using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManagerLvl5 : GameManagerBase
{
    #region Public Variables
    public PlayableDirector endOutroTimeline;
    public AudioSource endBGAud;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gmData.currState == GameMangerData.GameState.Game && _canExit)
            PauseGame();
    }
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
        endOutroTimeline.Play();
    }
    #endregion
}