using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLvl2 : GameManagerBase
{
    #region Public Variables
    [Space, Header("End Area")]
    public Collider2D endCol2D;
    public SpriteRenderer endDoorImg;
    public Sprite openDoorImg;

    [Space, Header("Plate")]
    public SpriteRenderer plateImg;
    public Sprite plateOnImg;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        PlayerController.OnLevelEnded += OnLevelEndedEventReceived;
        PlayerController.OnLevel2KeyPlaced += OnLevel2KeyPlacedEventReceived;
        PlayerController.OnVoidDeath += OnVoidDeathEventReceived;
    }

    void OnDisable()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnLevel2KeyPlaced -= OnLevel2KeyPlacedEventReceived;
        PlayerController.OnVoidDeath -= OnVoidDeathEventReceived;
    }

    void OnDestroy()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
        PlayerController.OnLevel2KeyPlaced -= OnLevel2KeyPlacedEventReceived;
        PlayerController.OnVoidDeath -= OnVoidDeathEventReceived;
    }
    #endregion

    void Start()
    {
        StartCoroutine(StartGameDelay());
        HumanDimensionAudio(true);
    }

//#if UNITY_STANDALONE
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && gmData.currState != GameMangerData.GameState.Switch
//            && gmData.currState != GameMangerData.GameState.Paused)
//            StartCoroutine(SwitchDimensionDelay());

//        if (Input.GetKeyDown(KeyCode.Escape) && gmData.currState == GameMangerData.GameState.Game)
//            PauseGame();
//    }
//#endif

    #endregion

    #region My Functions
    void OnLevel2KeyPlacedEventReceived()
    {
        endDoorImg.sprite = openDoorImg;
        plateImg.sprite = plateOnImg;
        endCol2D.enabled = true;
        doorSFXAud.Play();
    }
    #endregion

    #region Events
    void OnVoidDeathEventReceived() => StartCoroutine(DeathScreenDelay());
    #endregion
}