using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    #region Public Variables
    [Space, Header("Data")]
    public GameMangerData gmData;

    [Space, Header("Ghost")]
    public float followSpeed = 5f;
    public float stoppingDistance = 0.5f;
    public Transform followTarget;

    [Space, Header("Joystick")]
    public Joystick joy;
    #endregion

    #region Private Variables
    private float _distance;
    private Vector2 _moveDirection;
    private Animator _ghostAnim;
    #endregion

    #region Unity Callbacks
    void Start() => _ghostAnim = GetComponent<Animator>();

    void Update()
    {
        _distance = Vector3.Distance(transform.position, followTarget.position);

        if (gmData.currState == GameMangerData.GameState.Game)
        {
            FollowPlayer();
            PlayerInputs();
        }
        else
        {
            _ghostAnim.SetFloat("Horizontal", 0f);
            _ghostAnim.SetFloat("Vertical", 0f);
            _ghostAnim.SetFloat("Speed", 0f);
        }
    }
    #endregion

    #region My Functions
    void FollowPlayer()
    {
        if (_distance > stoppingDistance)
            transform.position = Vector2.MoveTowards(transform.position, followTarget.position, followSpeed * Time.fixedDeltaTime);
    }

    void PlayerInputs()
    {
#if UNITY_STANDALONE
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");
#else
        _moveDirection.x = joy.Horizontal;
        _moveDirection.y = joy.Vertical;
#endif

        _ghostAnim.SetFloat("Horizontal", _moveDirection.x);
        _ghostAnim.SetFloat("Vertical", _moveDirection.y);
        _moveDirection = _moveDirection.normalized;
        _ghostAnim.SetFloat("Speed", _moveDirection.sqrMagnitude);
    }
    #endregion
}