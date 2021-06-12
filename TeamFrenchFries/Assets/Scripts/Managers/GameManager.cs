using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    [Space, Header("Fade Panel")]
    public Animator fadeBG;
    public Animator fadeFastBG;

    [Space, Header("Dimensions")]
    public float switchDelay = 1f;
    public GameObject normalDimension;
    public GameObject horrorDimension;

    [Space, Header("Pause Panel")]
    public GameObject pausePanel;
    #endregion

    #region Private Variables
    [SerializeField] private int _currLevel = 1;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable()
    {
        PlayerController.OnLevelEnded += OnLevelEndedEventReceived;
    }

    void OnDisable()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
    }

    void OnDestroy()
    {
        PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
    }
    #endregion

    void Start()
    {
        StartCoroutine(StartGameDelay());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gmData.currState != GameMangerData.GameState.Switch)
            StartCoroutine(SwitchDimensionDelay());

        if (Input.GetKeyDown(KeyCode.Tab) && gmData.currState == GameMangerData.GameState.Game)
            PauseGame();
    }
    #endregion

    #region My Functions

    #region Buttons
    public void OnClick_Resume()
    {
        DisableCursor();
        gmData.ChangeState("Game");
        pausePanel.SetActive(false);
    }

    public void OnClick_Restart() => StartCoroutine(RestartGameDelay());

    public void OnClick_Menu() => StartCoroutine(MenuDelay());

    public void OnClick_Quit() => StartCoroutine(QuitDelay());
    #endregion

    void PauseGame()
    {
        EnableCursor();
        gmData.ChangeState("Paused");
        pausePanel.SetActive(true);
    }

    #region Cursor
    void EnableCursor()
    {
        gmData.LockCursor(false);
        gmData.VisibleCursor(true);
    }

    void DisableCursor()
    {
        gmData.LockCursor(true);
        gmData.VisibleCursor(false);
    }
    #endregion

    #endregion

    #region Events
    void OnLevelEndedEventReceived()
    {
        _currLevel++;
        StartCoroutine(StartNextLevelDelay());
    }
    #endregion

    #region Coroutines
    IEnumerator StartGameDelay()
    {
        DisableCursor();
        fadeBG.Play("FadeIn");
        gmData.ChangeState("Intro");
        yield return new WaitForSeconds(1f);
        gmData.ChangeState("Game");
    }

    IEnumerator RestartGameDelay()
    {
        DisableCursor();
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        gmData.NextLevel(_currLevel);
    }

    IEnumerator MenuDelay()
    {
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        gmData.Menu();
    }

    IEnumerator QuitDelay()
    {
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        gmData.QuitGame();
    }

    IEnumerator SwitchDimensionDelay()
    {
        fadeFastBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(switchDelay);
        normalDimension.SetActive(!normalDimension.activeSelf);
        horrorDimension.SetActive(!horrorDimension.activeSelf);
        fadeFastBG.Play("FadeIn");
        gmData.ChangeState("Game");
    }

    IEnumerator StartNextLevelDelay()
    {
        fadeFastBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(1f);
        gmData.NextLevel(_currLevel);
    }
    #endregion
}