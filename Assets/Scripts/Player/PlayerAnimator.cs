using System;
using System.Collections;
using System.Collections.Generic;
using Game.HealthSystem;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{   
    
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;

    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    private void Update()
    {
        anim.SetBool("HasGun", player.PlayerCombat.HasGun);
        
        if (player.PlayerMovement.IsFacingRight)
        {
            anim.SetBool("FacingRight", true);
        }
        if (player.PlayerMovement.IsFacingLeft)
        {
            anim.SetBool("FacingRight", false);
        }
    }


    //adding all the animations as separate methods
    public void PlayIdle()
    {
        if (player.PlayerMovement.IsMoving) return;
        
        anim.SetBool("IsMoving", false);
        
        switch (player.PlayerMovement.IsFacingRight)
        {
            //BackRight
            case true when player.PlayerMovement.IsFacingUp:
                // changed the backPose to front 

                anim.SetBool("FacingRight", true);
                break;
            //BackLeft
            case false when player.PlayerMovement.IsFacingUp:
                // changed the backPose to front 
                // anim.Play("IdleBackLeft");
                // anim.Play("IdleFrontLeft");
                
                anim.SetBool("FacingRight", false);
                break;
        }

        switch (player.PlayerMovement.IsFacingRight)
        {
            //frontRight
            case true when player.PlayerMovement.IsFacingDown:
                // anim.Play("IdleFrontRight");
                
                anim.SetBool("FacingRight", true);
                break;
            //frontLeft
            case false when player.PlayerMovement.IsFacingDown:
                // anim.Play("IdleFrontLeft");
                
                anim.SetBool("FacingRight", false);
                break;
        }
    }

    public void PlayMoving()
    {   
        anim.SetBool("IsMoving", true);
        switch (player.PlayerMovement.IsMovingRight)
        {
            //moving right
            case true:
                
                anim.SetBool("MovingRight", true);

                break;
            
            // not moving right but moving up facing right direction but not moving left
            case false when player.PlayerMovement.IsFacingRight && player.PlayerMovement.IsMovingUp && !player.PlayerMovement.IsMovingLeft:
                // anim.Play("ScytheWalkRight");
                
                anim.SetBool("MovingRight", true);
                break;
            
            // not moving right but moving down facing right direction but not moving left
            case false when player.PlayerMovement.IsFacingRight && player.PlayerMovement.IsMovingDown && !player.PlayerMovement.IsMovingLeft:
                // anim.Play("ScytheWalkRight");
                
                anim.SetBool("MovingRight", true);
                break;
        }
        
        switch (player.PlayerMovement.IsMovingLeft)
        {
            //moving left
            case true:
                // anim.Play("ScytheWalkLeft");
                
                anim.SetBool("MovingRight", false);
                break;

            // not moving left but moving up facing left direction but not moving right
            case false when player.PlayerMovement.IsFacingLeft && player.PlayerMovement.IsMovingUp && !player.PlayerMovement.IsMovingRight:
                // anim.Play("ScytheWalkLeft");
                
                anim.SetBool("MovingRight", false);
                break;
            
            // not moving left but moving down facing left direction but not moving right
            case false when player.PlayerMovement.IsFacingLeft && player.PlayerMovement.IsMovingDown && !player.PlayerMovement.IsMovingRight:
                // anim.Play("ScytheWalkLeft");
                
                anim.SetBool("MovingRight", false);
                break;
        }
    }

    public void PlayMeleeAttack1()
    {

        if (player.PlayerMovement.IsFacingRight)
        {   
            anim.SetTrigger("Melee1R");
        }

        if (player.PlayerMovement.IsFacingLeft)
        {   
            anim.SetTrigger("Melee1L");
        }

    }

    public void PlayMeleeAttack2()
    {
        if (player.PlayerMovement.IsFacingRight)
        {   
            anim.SetTrigger("Melee2R");
        }

        if (player.PlayerMovement.IsFacingLeft)
        {   
            anim.SetTrigger("Melee2L");
        }
    }


}
