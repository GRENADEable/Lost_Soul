using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    public float playerSpeed = 1f;

    #region Events
    public delegate void SendEvents();
    public static event SendEvents OnLevelEnded;
    public static event SendEvents OnLevel2KeyPlaced;

    public delegate void SendEventsInt(int index);
    public static event SendEventsInt OnPressurePlatePressed;
    #endregion

    #endregion

    #region Private Variables
    private Rigidbody2D _rg2D;
    private Vector2 _moveDirection;
    private Animator _playerAnim;
    private Collider2D _col2D;
    private int _currKey = 0;
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
            CheckInteraction();
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

    #region Triggers
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndArea"))
            OnLevelEnded?.Invoke();

        if (other.CompareTag("Key"))
            _col2D = other;

        if (other.CompareTag("Plate"))
            _col2D = other;

        if (other.CompareTag("Press_1") || other.CompareTag("Press_2") ||
            other.CompareTag("Press_3") || other.CompareTag("Press_4"))
            _col2D = other;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
            _col2D = null;

        if (other.CompareTag("Plate"))
            _col2D = null;

        if (other.CompareTag("Press_1") || other.CompareTag("Press_2") ||
            other.CompareTag("Press_3") || other.CompareTag("Press_4"))
            _col2D = null;
    }
    #endregion

    #endregion

    #region My Functions
    void PlayerInputs()
    {
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");

        _playerAnim.SetFloat("Horizontal", _moveDirection.x);
        _playerAnim.SetFloat("Vertical", _moveDirection.y);
        _moveDirection = _moveDirection.normalized;
        _playerAnim.SetFloat("Speed", _moveDirection.sqrMagnitude);
    }

    #region Interaction
    void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && _col2D != null && _col2D.CompareTag("Key"))
            PickKey();

        if (Input.GetKeyDown(KeyCode.E) && _col2D != null
            && _col2D.CompareTag("Plate") && _currKey == 1)
            InteractPlate();

        if (Input.GetKeyDown(KeyCode.E) && _col2D != null
            && _col2D.CompareTag("Press_1"))
            OnPressurePlatePressed?.Invoke(1);

        if (Input.GetKeyDown(KeyCode.E) && _col2D != null
            && _col2D.CompareTag("Press_2"))
            OnPressurePlatePressed?.Invoke(2);

        if (Input.GetKeyDown(KeyCode.E) && _col2D != null
           && _col2D.CompareTag("Press_3"))
            OnPressurePlatePressed?.Invoke(3);

        if (Input.GetKeyDown(KeyCode.E) && _col2D != null
           && _col2D.CompareTag("Press_4"))
            OnPressurePlatePressed?.Invoke(4);
    }

    void PickKey()
    {
        Destroy(_col2D.gameObject);
        _currKey++;
    }

    void InteractPlate()
    {
        OnLevel2KeyPlaced?.Invoke();
        _col2D = null;
        _currKey = 0;
    }
    #endregion

    #endregion
}