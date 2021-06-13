using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroExit : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Callbacks
    void Start()
    {

    }

    void Update()
    {

    }
    #endregion

    #region My Functions
    public void OnOutroEnded()
    {
        gmData.LockCursor(false);
        gmData.VisibleCursor(true);
        gmData.Menu();
    }
    #endregion
}