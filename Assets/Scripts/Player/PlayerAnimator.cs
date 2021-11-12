using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{   
    
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;
    
    
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    
    //adding all the animations as separate methods
    public void PlayIdle()
    {
        if (player.PlayerMovement.IsMoving) return;
        
        switch (player.PlayerMovement.IsFacingRight)
        {
            //BackRight
            case true when player.PlayerMovement.IsFacingUp:
                anim.Play("IdleBackRight");
                break;
            //BackLeft
            case false when player.PlayerMovement.IsFacingUp:
                anim.Play("IdleBackLeft");
                break;
        }

        switch (player.PlayerMovement.IsFacingRight)
        {
            //frontRight
            case true when player.PlayerMovement.IsFacingDown:
                anim.Play("IdleFrontRight");
                break;
            //frontLeft
            case false when player.PlayerMovement.IsFacingDown:
                anim.Play("IdleFrontLeft");
                break;
        }
    }

    public void PlayMoving()
    {
        switch (player.PlayerMovement.IsMovingRight)
        {
            //moving right
            case true:
                anim.Play("WalkRight");
                break;
            
            // not moving right but moving up facing right direction but not moving left
            case false when player.PlayerMovement.IsFacingRight && player.PlayerMovement.IsMovingUp && !player.PlayerMovement.IsMovingLeft:
                anim.Play("WalkRight");
                break;
            
            // not moving right but moving down facing right direction but not moving left
            case false when player.PlayerMovement.IsFacingRight && player.PlayerMovement.IsMovingDown && !player.PlayerMovement.IsMovingLeft:
                anim.Play("WalkRight");
                break;
        }
        
        switch (player.PlayerMovement.IsMovingLeft)
        {
            //moving left
            case true:
                anim.Play("WalkLeft");
                break;

            // not moving left but moving up facing left direction but not moving right
            case false when player.PlayerMovement.IsFacingLeft && player.PlayerMovement.IsMovingUp && !player.PlayerMovement.IsMovingRight:
                anim.Play("WalkLeft");
                break;
            
            // not moving left but moving down facing left direction but not moving right
            case false when player.PlayerMovement.IsFacingLeft && player.PlayerMovement.IsMovingDown && !player.PlayerMovement.IsMovingRight:
                anim.Play("WalkLeft");
                break;
        }
    }
}
