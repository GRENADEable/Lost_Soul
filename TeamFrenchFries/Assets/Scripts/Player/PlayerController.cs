using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    public float playerSpeed = 1f;

    public delegate void SendEvents();
    public static event SendEvents OnLevelEnded;
    #endregion

    #region Private Variables
    private Rigidbody2D _rg2D;
    private Vector2 _moveDirection;
    private Animator _playerAnim;
    //private int _currKey = 0;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _rg2D = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
        {
            PlayerInputs();
        }
        else
        {
            _playerAnim.SetFloat("Horizontal", 0f);
            _playerAnim.SetFloat("Vertical", 0f);
            _playerAnim.SetFloat("Speed", 0f);
        }
    }

    void FixedUpdate()
    {
        if (gmData.currState == GameMangerData.GameState.Game)
            _rg2D.MovePosition(_rg2D.position + (_moveDirection * playerSpeed * Time.fixedDeltaTime));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndArea"))
            OnLevelEnded?.Invoke();
    }
    #endregion

    #region My Functions
    void PlayerInputs()
    {
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");

        _playerAnim.SetFloat("Horizontal", _moveDirection.x);
        _playerAnim.SetFloat("Vertical", _moveDirection.y);
        _playerAnim.SetFloat("Speed", _moveDirection.sqrMagnitude);

        _moveDirection = _moveDirection.normalized;
    }
    #endregion
}