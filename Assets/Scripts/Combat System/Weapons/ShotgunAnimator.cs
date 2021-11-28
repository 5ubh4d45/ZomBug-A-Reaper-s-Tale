using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform firePoint;

    public Transform FirePoint => firePoint;

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.PlayerMovement.IsFacingRight)
        {
            anim.SetBool("FacingRight", true);
        }

        if (Player.Instance.PlayerMovement.IsFacingLeft)
        {
            anim.SetBool("FacingRight", false);
        }
    }

    public void PlayShotgunFire()
    {
        if (Player.Instance.PlayerMovement.IsFacingRight)
        {
            anim.SetTrigger("FireRight");
        }

        if (Player.Instance.PlayerMovement.IsFacingLeft)
        {
            anim.SetTrigger(("FireLeft"));
        }
    }



}
