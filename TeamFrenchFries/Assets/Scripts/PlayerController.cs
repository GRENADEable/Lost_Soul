using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    public float playerSpeed = 1f;
    #endregion

    #region Private Variables
    private Rigidbody2D _rg2D;
    private Vector2 _moveDirection;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _rg2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
        {
            PlayerInputs();
        }

    }

    void FixedUpdate()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
            _rg2D.MovePosition(_rg2D.position + (_moveDirection * playerSpeed * Time.fixedDeltaTime));
    }
    #endregion

    #region My Functions
    void PlayerInputs()
    {
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");
        _moveDirection = _moveDirection.normalized;
    }
    #endregion
}