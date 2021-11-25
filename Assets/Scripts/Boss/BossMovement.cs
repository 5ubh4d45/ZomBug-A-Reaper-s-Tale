using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossMovement : MonoBehaviour
{
    #region InspectorComponents
    
    [Header("Movement Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CatBoss boss;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float backOffSpeed;
    [SerializeField] private float backOffImpulseForce;
    
    #endregion
    
    #region PrivateVariables
    //private bools
    private bool _isFacingRight = true;
    private bool _isMovingRight = true;
    private bool _isMoving;
    private bool _atMinMeleeDistance;

    //private floats
    private float _currentDistance;
    private float _distanceFromMinRanged;

    //private Vectors
    private Vector3 _lookDir;
    private Vector3 _moveDir;
    private Vector3 _lastPosition;

    #endregion

    #region Getters

    public Vector3 LookDir => _lookDir;
    public Vector3 MoveDir => _moveDir;
    public float CurrentDistance => _currentDistance;
    public bool IsFacingRight => _isFacingRight;
    public bool IsMovingRIght => _isMovingRight;
    public bool IsMoving => _isMoving;
    public bool AtMinMeleeDistance => _atMinMeleeDistance;

    #endregion
    
    // Start is called before the first frame update
    private void Start()
    {
        //setting up components
        if (boss == null)
        {
            boss = GetComponent<CatBoss>();
        }

        _lastPosition = transform.position;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {   
        //updates the required parameters
        CheckParameters();
        

    }
    

    private void CheckParameters()
    {
        Vector3 targetPos = boss.Target.position;
        Vector3 currentPos = transform.position;
        
        // _isMoving = !rb.IsSleeping();
        
        // updates Direction
        _moveDir = (currentPos - _lastPosition).normalized;
        _lookDir = (targetPos - (Vector3) rb.position).normalized;

        _isMoving = Mathf.Abs(_moveDir.magnitude) > 0f;

        _lastPosition = currentPos;
        
        //updates distance
        _currentDistance = Vector3.Distance(targetPos, (Vector3) rb.position);
        
        // distance required to backoff
        _distanceFromMinRanged = (boss.BossCombat.MinRangedDistance - _currentDistance);
        _distanceFromMinRanged = Mathf.Clamp(_distanceFromMinRanged, 0f, boss.BossCombat.MinRangedDistance);
        
        //sets isfacingright;
        _isFacingRight = _lookDir.x > 0f;

        _isMovingRight = _moveDir.x > 0f;

        _atMinMeleeDistance = _currentDistance <= boss.BossCombat.MinMeleeDistance;

    }

    private void Follow(bool canFollow, Vector3 targetDirection, float speed)
    {
        if (!canFollow)
            return;
        
        // boss.BossAnimator.PlayWalk();

        float speedFactor = _atMinMeleeDistance ? Mathf.Sqrt(_currentDistance / boss.BossCombat.MinMeleeDistance) : 1f;

        Vector3 velocity = targetDirection * (speed * speedFactor);

        rb.position += (Vector2) velocity * Time.fixedDeltaTime;
    }

    public void BackOff(bool canBackOff)
    {
        Vector3 reverseDir = new Vector3(_lookDir.x * -1, _lookDir.y * -1, _lookDir.z);

        Follow(canBackOff, reverseDir, backOffSpeed);
    }

    public IEnumerator BackoffImpulse()
    {
        Vector3 reverseDir = new Vector3(_lookDir.x * -1, _lookDir.y * -1, _lookDir.z);

        Vector3 reverseDirForce = reverseDir * backOffImpulseForce;

        yield return new WaitForSeconds(0.4f);
        
        rb.AddForce((Vector2)reverseDirForce, ForceMode2D.Impulse);



    }
    public void Chase(bool canChase)
    {
        Follow(canChase, _lookDir, movementSpeed);
    }
}
