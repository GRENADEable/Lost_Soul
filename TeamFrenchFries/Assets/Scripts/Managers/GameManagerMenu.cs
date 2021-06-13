using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerMenu : GameManagerBase
{
    #region Public Variables
    [Space, Header("Buttons")]
    public Button[] uIButtons;

    [Space, Header("Resolutions")]
    public TMP_Dropdown resDropDown;
    #endregion

    #region Private Variables
    private Resolution[] _resolutions;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        gmData.ChangeState("Menu");
        fadeBG.Play("FadeIn");
        IntializeDropDownRes();
    }
    #endregion

    #region My Functions

    #region UI
    public void OnClick_NewGame() => StartCoroutine(StartNewGameDelay());
    public void OnClick_QuitGame() => StartCoroutine(ExitGameDelay());

    void UIButtons()
    {
        for (int i = 0; i < uIButtons.Length; i++)
            uIButtons[i].interactable = false;
    }
    #endregion

    #region Settings
    public void SetRes(int resIndex)
    {
        Resolution res = _resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    void IntializeDropDownRes()
    {
        _resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        resDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currResIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width
                && _resolutions[i].height == Screen.height)
                currResIndex = i;
        }

        resDropDown.AddOptions(options);
        resDropDown.value = currResIndex;
        resDropDown.RefreshShownValue();
    }
    #endregion

    #endregion

    #region Coroutines
    IEnumerator StartNewGameDelay()
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