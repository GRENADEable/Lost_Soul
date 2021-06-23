using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerLvl4 : GameManagerBase
{
    #region Public Variables
    [Space, Header("Dimension Timer")]
    public float dimensionDelay = 1f;
    public TextMeshProUGUI dimensionTimerText;

    [Space, Header("Panels")]
    public GameObject deathPanel;

    [Space, Header("Audios")]
    public AudioSource deathAud;
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gmData.currState == GameMangerData.GameState.Game && !_isSwitched)
            StartCoroutine(SwitchToHorrorDimensionDelay());

        if (Input.GetKeyDown(KeyCode.Escape) && gmData.currState == GameMangerData.GameState.Game)
            PauseGame();

        if (_isSwitched && gmData.currState == GameMangerData.GameState.Game)
            DimensionCounter();
    }
    #endregion

    #region My Functions
    void DimensionCounter()
    {
        _currTimer -= Time.deltaTime;
        dimensionTimerText.text = $"Time Left: {_currTimer.ToString("f0")}";

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
        dimensionTimerText.gameObject.SetActive(true);
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
        dimensionTimerText.gameObject.SetActive(false);
        _currTimer = dimensionDelay;
        _isSwitched = false;
        normalDimension.SetActive(true);
        horrorDimension.SetActive(false);
        fadeFastBG.Play("FadeIn");
        gmData.ChangeState("Game");
    }
    #endregion

    IEnumerator DeathScreenDelay()
    {
        fadeBG.Play("FadeOut");
        deathAud.Play();
        gmData.ChangeState("Dead");
        yield return new WaitForSeconds(1f);
        EnableCursor();
        fadeBG.Play("FadeIn");
        hudPanel.gameObject.SetActive(false);
        deathPanel.SetActive(true);
    }
    #endregion
}