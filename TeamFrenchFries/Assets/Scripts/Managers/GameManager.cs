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
    }
    #endregion

    #region My Functions
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
        fadeBG.Play("FadeIn");
        gmData.ChangeState("Intro");
        yield return new WaitForSeconds(1f);
        gmData.ChangeState("Game");
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
        Application.LoadLevel($"Level_{_currLevel}");
    }
    #endregion
}