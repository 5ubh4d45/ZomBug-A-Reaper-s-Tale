using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    [SerializeField] private CatBoss boss;
    [SerializeField] private Animator anim;
    


    // Start is called before the first frame update
    void Start()
    {
        //setting up components
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        if (boss == null)
        {
            boss = GetComponent<CatBoss>();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        anim.SetBool("CanWalk", boss.BossMovement.IsMoving);
    }

    public void PlayIdle()
    {
        anim.SetBool("CanWalk", false);
    }

    public void PlayWalk()
    {
        anim.SetBool("CanWalk", true);
    }

    public void PlayMeleeAttack()
    {
        anim.SetTrigger("MeleeAttack");
    }

    public void PlayJumpAttack()
    {
        anim.SetTrigger("JumpAttack");
    } 
}
