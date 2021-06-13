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

    void Start() => StartCoroutine(StartGameDelay());

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && gmData.currState == GameMangerData.GameState.Game && _canExit)
            PauseGame();
    }
    #endregion

    #region My Functions

    #endregion

    #region Events
    void OnGameEndEventReceived()
    {
        _canExit = false;
        endOutroTimeline.Play();
        //StartCoroutine(EndGameDelay());
    }
    #endregion

    #region Coroutines
    //IEnumerator EndGameDelay()
    //{
    //    _canExit = false;
    //    yield return new WaitForSeconds(1f);
    //}
    #endregion
}