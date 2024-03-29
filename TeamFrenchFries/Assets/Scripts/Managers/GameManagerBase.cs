using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManagerBase : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    [Space, Header("Panels")]
    public Animator fadeBG;
    public Animator hudPanel;
    public GameObject deathPanel;
    public GameObject pausePanel;

    [Space, Header("Buttons")]
    public Button[] uIButtons;
    public GameObject controlsObj;
    public GameObject interactionObjs;
    public GameObject pauseButton;

    [Space, Header("Dimensions")]
    public float switchDelay = 1f;
    public GameObject normalDimension;
    public GameObject horrorDimension;

    [Space, Header("Audio")]
    public AudioSource buttonSFXAud;
    public AudioSource doorSFXAud;
    public AudioSource deathSFXAud;
    public AudioSource[] humanDimensionAud;
    public AudioSource[] spiritDimensionAud;
    public PlayerFootsteps plyFootsteps;
    #endregion

    #region Private Variables
    [SerializeField] private int _currLevel = 1;
    #endregion

    #region Unity Callbacks

    #region Events
    void OnEnable() => PlayerController.OnLevelEnded += OnLevelEndedEventReceived;

    void OnDisable() => PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;

    void OnDestroy() => PlayerController.OnLevelEnded -= OnLevelEndedEventReceived;
    #endregion

    #endregion

    #region My Functions

    #region Buttons

    #region HUD
    public void OnClick_SwitchDimension()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
            StartCoroutine(SwitchDimensionDelay());
    }

    public void OnClick_Pause()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
            PauseGame();
    }
    #endregion

    #region Menus
    public void OnClick_NewGame()
    {
        buttonSFXAud.Play();
        DisableCursor();
        gmData.ChangeState("Game");
        StartCoroutine(StartNewGameDelay());
    }

    public void OnClick_Resume()
    {
        buttonSFXAud.Play();
        DisableCursor();
        gmData.ChangeState("Game");
        pausePanel.SetActive(false);
        hudPanel.gameObject.SetActive(true);
    }

    public void OnClick_Restart()
    {
        buttonSFXAud.Play();
        StartCoroutine(RestartGameDelay());
    }

    public void OnClick_Menu()
    {
        buttonSFXAud.Play();
        StartCoroutine(MenuDelay());
    }

    public void OnClick_Quit()
    {
        buttonSFXAud.Play();
        StartCoroutine(QuitDelay());
    }

    void UIButtons()
    {
        for (int i = 0; i < uIButtons.Length; i++)
            uIButtons[i].interactable = false;
    }
    #endregion

    #endregion

    #region Audio
    public void HumanDimensionAudio(bool enabled)
    {
        if (enabled)
        {
            for (int i = 0; i < humanDimensionAud.Length; i++)
                humanDimensionAud[i].Play();

            Debug.Log("Enabled Human Audio");
        }
        else
        {
            for (int i = 0; i < humanDimensionAud.Length; i++)
                humanDimensionAud[i].Stop();

            Debug.Log("Disabled Human Audio");
        }
    }

    public void SpiritDimensionAudio(bool enabled)
    {
        if (enabled)
        {
            for (int i = 0; i < spiritDimensionAud.Length; i++)
                spiritDimensionAud[i].Play();

            Debug.Log("Enabled Spirit Audio");
        }
        else
        {
            for (int i = 0; i < spiritDimensionAud.Length; i++)
                spiritDimensionAud[i].Stop();

            Debug.Log("Disabled Spirit Audio");
        }
    }
    #endregion

    #region Settings
    public void OnToggleSetFullScreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;
    #endregion

    void CheckDimension()
    {
        if (normalDimension.activeInHierarchy)
        {
            SpiritDimensionAudio(false);
            HumanDimensionAudio(true);
            plyFootsteps.currDiemsion = PlayerFootsteps.DimensionState.Human;
        }

        if (horrorDimension.activeInHierarchy)
        {
            HumanDimensionAudio(false);
            SpiritDimensionAudio(true);
            plyFootsteps.currDiemsion = PlayerFootsteps.DimensionState.Spirit;
        }
    }

    protected void PauseGame()
    {
        hudPanel.gameObject.SetActive(false);
        buttonSFXAud.Play();
        EnableCursor();
        gmData.ChangeState("Paused");
        pausePanel.SetActive(true);
    }

    #region Cursor
    protected void EnableCursor()
    {
#if UNITY_STANDALONE
        gmData.LockCursor(false);
        gmData.VisibleCursor(true);
#endif
    }

    protected void DisableCursor()
    {
#if UNITY_STANDALONE
        gmData.LockCursor(true);
        gmData.VisibleCursor(false);
#endif
    }
    #endregion

    #endregion

    #region Events
    protected void OnLevelEndedEventReceived()
    {
        _currLevel++;
        StartCoroutine(StartNextLevelDelay());
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.started && gmData.currState == GameMangerData.GameState.Game)
            PauseGame();
    }

    public void OnDimensionSwitch(InputAction.CallbackContext context)
    {
        if (context.started && gmData.currState == GameMangerData.GameState.Game)
            StartCoroutine(SwitchDimensionDelay());
    }
    #endregion

    #region Coroutines
    IEnumerator StartNewGameDelay()
    {
        UIButtons();
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        gmData.StartNewGame();
    }

    protected IEnumerator StartGameDelay()
    {
        DisableCursor();
#if UNITY_STANDALONE
        controlsObj.SetActive(false);
        interactionObjs.SetActive(false);
        pauseButton.SetActive(false);
#else
        controlsObj.SetActive(true);
        interactionObjs.SetActive(true);
        pauseButton.SetActive(true);
#endif
        hudPanel.Play("FadeIn");
        fadeBG.Play("FadeIn");
        gmData.ChangeState("Intro");
        yield return new WaitForSeconds(0.5f);
        gmData.ChangeState("Game");
    }

    protected IEnumerator DeathScreenDelay()
    {
        fadeBG.Play("FadeOut");
        deathSFXAud.Play();
        gmData.ChangeState("Dead");
        yield return new WaitForSeconds(0.5f);
        EnableCursor();
        fadeBG.Play("FadeIn");
        hudPanel.gameObject.SetActive(false);
        deathPanel.SetActive(true);
    }

    IEnumerator RestartGameDelay()
    {
        DisableCursor();
        UIButtons();
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        gmData.NextLevel(_currLevel);
    }

    IEnumerator MenuDelay()
    {
        UIButtons();
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        gmData.Menu();
    }

    IEnumerator QuitDelay()
    {
        UIButtons();
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        gmData.QuitGame();
    }

    protected IEnumerator SwitchDimensionDelay()
    {
        fadeBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(switchDelay);
        normalDimension.SetActive(!normalDimension.activeSelf);
        horrorDimension.SetActive(!horrorDimension.activeSelf);
        CheckDimension();
        fadeBG.Play("FadeIn");
        gmData.ChangeState("Game");
    }

    IEnumerator StartNextLevelDelay()
    {
        hudPanel.Play("FadeOut");
        fadeBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(0.5f);
        gmData.NextLevel(_currLevel);
    }
    #endregion
}