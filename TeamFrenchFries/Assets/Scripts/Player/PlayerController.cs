using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    [Space, Header("Player")]
    public float playerSpeed = 1f;
    public Transform pickKeyPos;
    public Animator playerFootStepAnim;

    //[Space, Header("Joystick")]
    //public Joystick joy;

    #region Events
    public delegate void SendEvents();
    public static event SendEvents OnLevelEnded;
    public static event SendEvents OnLevel2KeyPlaced;
    public static event SendEvents OnVoidDeath;
    public static event SendEvents OnGameEnd;

    public delegate void SendEventsInt(int index);
    public static event SendEventsInt OnPressurePlatePressed;
    #endregion

    #endregion

    #region Private Variables
    private Rigidbody2D _rg2D;
    private Vector2 _moveDirection;
    private Animator _playerAnim;
    private Collider2D _col2D;
    private GameObject _pickedKey;
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
#if UNITY_STANDALONE
            //CheckInteraction();
#endif
            FootStepsCheck();
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
            OnLevelEnded?.Invoke(); // Event Sent to GameManagerLevels;

        if (other.CompareTag("Key"))
            _col2D = other;

        if (other.CompareTag("Plate"))
            _col2D = other;

        if (other.CompareTag("Press_1") || other.CompareTag("Press_2") ||
            other.CompareTag("Press_3") || other.CompareTag("Press_4"))
            _col2D = other;

        if (other.CompareTag("Death_Zone"))
            OnVoidDeath?.Invoke(); // Event sent to All GameManagerLvl Scripts;

        if (other.CompareTag("End_Zone"))
        {
            other.enabled = false;
            OnGameEnd?.Invoke(); // Event sent to All GameManagerLvl Scripts;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
            _col2D = null;

        if (other.CompareTag("Plate"))
            _col2D = null;

        if (other.CompareTag("Press_1"))
            OnPressurePlatePressed?.Invoke(1);

        if (other.CompareTag("Press_2"))
            OnPressurePlatePressed?.Invoke(2);

        if (other.CompareTag("Press_3"))
            OnPressurePlatePressed?.Invoke(3);

        if (other.CompareTag("Press_4"))
            OnPressurePlatePressed?.Invoke(4);
    }
    #endregion

    #endregion

    #region My Functions

    #region Footsteps

    void FootStepsCheck()
    {
        if (_moveDirection != Vector2.zero)
            playerFootStepAnim.SetBool("isMoving", true);
        else
            playerFootStepAnim.SetBool("isMoving", false);
    }
    #endregion

    #region Inputs
    public void OnClick_Pick()
    {
        if (_col2D != null && _col2D.CompareTag("Key"))
            PickKey();

        if (_col2D != null && _col2D.CompareTag("Plate") && _currKey == 1)
            InteractPlate();
    }

    public void OnClick_PlayerInputMobileX(float input) => _moveDirection.x = input;

    public void OnClick_PlayerInputMobileY(float input) => _moveDirection.y = input;

    void PlayerInputs()
    {
        //#if UNITY_STANDALONE
        //        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        //        _moveDirection.y = Input.GetAxisRaw("Vertical");
        //#else
        //        //_moveDirection.x = joy.Horizontal;
        //        //_moveDirection.y = joy.Vertical;
        //#endif

        _playerAnim.SetFloat("Horizontal", _moveDirection.x);
        _playerAnim.SetFloat("Vertical", _moveDirection.y);
        _moveDirection = _moveDirection.normalized;
        _playerAnim.SetFloat("Speed", _moveDirection.sqrMagnitude);
    }
    #endregion

    #region Interaction
    //void CheckInteraction()
    //{
    //    if (Input.GetKeyDown(KeyCode.E) && _col2D != null && _col2D.CompareTag("Key"))
    //        PickKey();

    //    if (Input.GetKeyDown(KeyCode.E) && _col2D != null
    //        && _col2D.CompareTag("Plate") && _currKey == 1)
    //        InteractPlate();
    //}

    void PickKey()
    {
        _pickedKey = _col2D.gameObject;
        _pickedKey.transform.position = pickKeyPos.position;
        _pickedKey.transform.parent = pickKeyPos;
        Destroy(_pickedKey.GetComponent<Rigidbody2D>());
        _pickedKey.GetComponent<Collider2D>().enabled = false;
        _currKey++;
        _playerAnim.SetBool("IsPicking", true);
    }

    void InteractPlate()
    {
        _playerAnim.SetBool("IsPicking", false);
        OnLevel2KeyPlaced?.Invoke();
        _pickedKey.transform.position = _col2D.transform.position;
        _pickedKey.transform.parent = _col2D.transform;
        _col2D.enabled = false;
        _currKey = 0;
    }
    #endregion

    #endregion

    #region Events
    public void OnMove(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();

    public void OnPick(InputAction.CallbackContext context)
    {
        if (context.started && _col2D != null && _col2D.CompareTag("Key"))
            PickKey();

        if (context.started && _col2D != null && _col2D.CompareTag("Plate") && _currKey == 1)
            InteractPlate();
    }
    #endregion
}