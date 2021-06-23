using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    [Space, Header("Panels")]
    public Animator fadeBG;
    public Animator fadeFastBG;
    public Animator hudPanel;
    public GameObject pausePanel;
    public GameObject joystickObj;

    [Space, Header("Dimensions")]
    public float switchDelay = 1f;
    public GameObject normalDimension;
    public GameObject horrorDimension;

    [Space, Header("Audio")]
    public AudioSource buttonSFXAud;
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
    public void OnClick_Resume()
    {
        buttonSFXAud.Play();
        DisableCursor();
        gmData.ChangeState("Game");
        pausePanel.SetActive(false);
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
    #endregion

    #region Coroutines
    protected IEnumerator StartGameDelay()
    {
        DisableCursor();
#if UNITY_STANDALONE
        joystickObj.SetActive(false);
#else
        joystickObj.SetActive(true);
#endif
        hudPanel.Play("FadeIn");
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

    protected IEnumerator SwitchDimensionDelay()
    {
        fadeFastBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(switchDelay);
        normalDimension.SetActive(!normalDimension.activeSelf);
        horrorDimension.SetActive(!horrorDimension.activeSelf);
        CheckDimension();
        fadeFastBG.Play("FadeIn");
        gmData.ChangeState("Game");
    }

    IEnumerator StartNextLevelDelay()
    {
        hudPanel.Play("FadeOut");
        fadeFastBG.Play("FadeOut");
        gmData.ChangeState("Switch");
        yield return new WaitForSeconds(1f);
        gmData.NextLevel(_currLevel);
    }
    #endregion
}