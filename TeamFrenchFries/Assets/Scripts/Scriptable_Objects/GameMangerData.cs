using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameManagerData", menuName = "Managers/GameManagerData")]
public class GameMangerData : ScriptableObject
{
    [Space, Header("Enums")]
    public GameState currState = GameState.Game;
    public enum GameState { Menu, Intro, Game, Switch, Paused, Exit };

    #region My Functions

    #region Scenes
    public void StartNewGame() => Application.LoadLevel("Skate_Store_Main");
    public void Menu() => Application.LoadLevel("Skate_Store_Menu");

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }
    #endregion

    #region Game States

    #region Cursor
    public void LockCursor(bool isLocked)
    {
        if (isLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    public void VisibleCursor(bool isVisible)
    {
        if (isVisible)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }
    #endregion

    public void TogglePause(bool isPaused)
    {
        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void ChangeState(string state)
    {
        if (state.Contains("Menu"))
            currState = GameState.Menu;

        if (state.Contains("Intro"))
            currState = GameState.Intro;

        if (state.Contains("Game"))
            currState = GameState.Game;

        if (state.Contains("Switch"))
            currState = GameState.Switch;

        if (state.Contains("Paused"))
            currState = GameState.Paused;

        if (state.Contains("Exit"))
            currState = GameState.Exit;

    }
    #endregion

    #endregion
}