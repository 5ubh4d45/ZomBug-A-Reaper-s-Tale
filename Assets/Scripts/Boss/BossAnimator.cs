using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BossAnimator : MonoBehaviour
{
    [SerializeField] private CatBoss boss;
    [SerializeField] private Animator anim;
    [SerializeField] private float deathAnimationTime;

    [Space]
    // reference to the sound holder use _soundHolder.(your sound string variable)
    // at the Fmod sound string like
    // RuntimeManager.PlayOneShot(soundHolder.DeathSound);
    [SerializeField] private EnemySoundHolder soundHolder;

    public float DeathAnimationTime => deathAnimationTime;
    

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
        if (soundHolder == null)
        {
            soundHolder = GetComponent<EnemySoundHolder>();
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
        RuntimeManager.PlayOneShot(soundHolder.MeleeAttackSound);

        anim.SetTrigger("MeleeAttack");
    }

    public void PlayJumpAttack()
    {
        anim.SetTrigger("JumpAttack");
    }

    public void PlayDeathAniamtion()
    {   
        RuntimeManager.PlayOneShot(soundHolder.DeathSound);

        anim.SetTrigger("Death");
    }
}
