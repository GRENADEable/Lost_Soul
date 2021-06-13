using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    [Space, Header("Enums")]
    public DimensionState currDiemsion = DimensionState.Human;
    public enum DimensionState { Human, Spirit };

    [Space, Header("Foosteps")]
    public AudioSource footstepSFXAud;
    public AudioClip[] footstepsClips;
    #endregion

    #region Unity Callbacks

    #endregion

    #region My Functions
    public void FootStep1()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
        {
            if (currDiemsion == DimensionState.Human)
                footstepSFXAud.PlayOneShot(footstepsClips[0]); // Human world footstep
            else
                footstepSFXAud.PlayOneShot(footstepsClips[2]); // Spirit world footstep
        }
    }

    public void FootStep2()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
        {
            if (currDiemsion == DimensionState.Human)
                footstepSFXAud.PlayOneShot(footstepsClips[1]); // Human world footstep
            else
                footstepSFXAud.PlayOneShot(footstepsClips[3]); // Spirit world footstep
        }
    }
    #endregion

    #region Events

    #endregion
}