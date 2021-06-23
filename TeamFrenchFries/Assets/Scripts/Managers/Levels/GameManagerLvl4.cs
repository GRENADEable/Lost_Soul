using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerLvl4 : GameManagerBase
{
    #region Public Variables
    [Space, Header("Dimension Timer")]
    public float dimensionDelay = 2f;
    public TextMeshProUGUI dimensionTimerText;
    #endregion

    #region Private Variables
    private bool _isSwitched;
    private float _currTimer = 0f;
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
        doorSFXAud.Play();
    }
    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space) && gmData.currState == GameMangerData.GameState.Game && !_isSwitched)
            StartCoroutine(SwitchToHorrorDimensionDelay());

        if (Input.GetKeyDown(KeyCode.Escape) && gmData.currState == GameMangerData.GameState.Game)
            PauseGame();
#endif

        if (_isSwitched && gmData.currState == GameMangerData.GameState.Game)
            DimensionCounter();
    }

    #endregion

    #region My Functions
    public void OnClick_SwitchDimensionForLvl4()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
            StartCoroutine(SwitchToHorrorDimensionDelay());
    }

    void DimensionCounter()
    {
        _currTimer -= Time.deltaTime;
        dimensionTimerText.text = $"Time Left: {_currTimer:f0}";

        if (_currTimer <= 0)
            StartCoroutine(SwitchToNormalDimensionDelay());
    }
    #endregion

    #region Events
    void OnVoidDeathEventReceived() => StartCoroutine(DeathScreenDelay());
    #endregion

    #region Coroutines

    #region Dimension Switches
    IEnumerator SwitchToHorrorDimensionDelay()
    {
        fadeFastBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(switchDelay);
        HumanDimensionAudio(false);
        SpiritDimensionAudio(true);
        _currTimer = dimensionDelay;
        _isSwitched = true;
        normalDimension.SetActive(false);
        horrorDimension.SetActive(true);
        fadeFastBG.Play("FadeIn");
        gmData.ChangeState("Game");
    }

    IEnumerator SwitchToNormalDimensionDelay()
    {
        fadeFastBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(switchDelay);
        SpiritDimensionAudio(false);
        HumanDimensionAudio(true);
        _currTimer = dimensionDelay;
        dimensionTimerText.text = $"Time Left: {_currTimer:f0}";
        _isSwitched = false;
        normalDimension.SetActive(true);
        horrorDimension.SetActive(false);
        fadeFastBG.Play("FadeIn");
        gmData.ChangeState("Game");
    }
    #endregion

    #endregion
}