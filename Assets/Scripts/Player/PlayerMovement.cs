using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.DialogueSystem;
using Game.Combat;

public class PlayerMovement : MonoBehaviour
{
    [Header("Required Components")]

    public Rigidbody2D rb;
    public Camera cam;

    private Player _player;
    private Vector2 _lastPos;

    #region Private Bools and Getters
    //bool for checking left or right
    private bool _isFacingRight;
    private bool _isFacingLeft;
    private bool _isFacingUp;
    private bool _isFacingDown;
    private bool _isMoving;
    private bool _isMovingRight;
    private bool _isMovingLeft;
    private bool _isMovingUp;
    private bool _isMovingDown;

    public bool IsMoving => _isMoving;
    public bool IsFacingRight => _isFacingRight;
    public bool IsFacingLeft => _isFacingLeft;
    public bool IsFacingUp => _isFacingUp;
    public bool IsFacingDown => _isFacingDown;
    public bool IsMovingRight => _isMovingRight;
    public bool IsMovingLeft => _isMovingLeft;
    public bool IsMovingUp => _isMovingUp;
    public bool IsMovingDown => _isMovingDown;
    #endregion
    
    private Vector2 _movementDir;
    private Vector2 _mousePos;
    private Vector2 _lookDir;



    void Start()
    {
        _player = GetComponent<Player>();
        cam = Camera.main;
        _lastPos = rb.transform.position;
    }


    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        //checks the directions the player moving
        CheckDirections();
        
        //play animations related to movements
        PlayMovementAnims();
        
        if (DialogueManager.Instance.IsOpen || WeaponWheel.Instance.IsOpened) return;
        MovePlayer();
        
    }


    private void ProcessInputs()
    {

        //Gethering input for Player MOvement X & Y axis
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _movementDir = new Vector2(moveX, moveY).normalized;

        //Gathering mousepointer position
        _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void MovePlayer()
    {
        
        //changing positions according to input
        rb.MovePosition(rb.position + _movementDir * _player.MoveSpeed * Time.fixedDeltaTime);

        //Finding Angle of rotation from player to mouse
        _lookDir = _mousePos - rb.position;
        

    }

    private void CheckDirections()
    {
        Vector2 currentPos = rb.position;
        // checks if player is left/right or up/down of the mouse position
        _isFacingRight = _lookDir.x > 0;
        _isFacingLeft = _lookDir.x < 0;
        _isFacingUp = _lookDir.y > 0;
        _isFacingDown = _lookDir.y < 0;
        
        // checks if player is moving left/right or up/down
        _isMovingRight = _movementDir.x > 0;
        _isMovingLeft = _movementDir.x < 0;
        _isMovingUp = _movementDir.y > 0;
        _isMovingDown = _movementDir.y < 0;
        
        //checks if player moving
        _isMoving = _lastPos != currentPos;

        _lastPos = currentPos;

    }

    private void PlayMovementAnims()
    {
        switch (_isMoving)
        {
            case false:
                _player.PlayerAnimator.PlayIdle();
                break;
            case true:
                _player.PlayerAnimator.PlayMoving();
                break;
        }
    }

}
