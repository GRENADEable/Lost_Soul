using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerMenu : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    [Space, Header("Fade Panel")]
    public Animator fadeBG;

    [Space, Header("Buttons")]
    public Button[] uIButtons;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Callbacks
    void Start()
    {
        gmData.ChangeState("Menu");
        fadeBG.Play("FadeIn");
    }
    #endregion

    #region My Functions
    public void OnClick_NewGame() => StartCoroutine(StartGameDelay());
    public void OnClick_QuitGame() => StartCoroutine(ExitGameDelay());

    void UIButtons()
    {
        for (int i = 0; i < uIButtons.Length; i++)
            uIButtons[i].interactable = false;
    }
    #endregion

    #region Coroutines
    IEnumerator StartGameDelay()
    {
        UIButtons();
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        gmData.StartNewGame();
    }

    IEnumerator ExitGameDelay()
    {
        UIButtons();
        fadeBG.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        gmData.QuitGame();
    }
    #endregion
}