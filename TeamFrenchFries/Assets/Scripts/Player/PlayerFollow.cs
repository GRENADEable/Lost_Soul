using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    #region Public Variables
    public float followSpeed = 4f;
    public float stoppingDistance = 3f;
    public Transform followTarget;
    #endregion

    #region Private Variables
    private float _distance;
    #endregion

    #region Unity Callbacks

    void Update()
    {
        _distance = Vector3.Distance(transform.position, followTarget.position);
        FollowPlayer();
    }
    #endregion

    #region My Functions
    void FollowPlayer()
    {
        if (_distance > stoppingDistance)
            transform.position = Vector2.MoveTowards(transform.position, followTarget.position, followSpeed * Time.fixedDeltaTime);
    }
    #endregion
}