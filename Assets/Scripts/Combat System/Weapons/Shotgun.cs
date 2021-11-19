using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Player player;

    public Transform FirePoint => firePoint;

    void Start()
    {
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.PlayerMovement.IsFacingRight)
        {
            anim.SetBool("FacingRight", true);
        }

        if (player.PlayerMovement.IsFacingLeft)
        {
            anim.SetBool("FacingRight", false);
        }
    }

    public void PlayPistolFire()
    {
        if (player.PlayerMovement.IsFacingRight)
        {
            anim.SetTrigger("FireRight");
        }

        if (player.PlayerMovement.IsFacingLeft)
        {
            anim.SetTrigger(("FireLeft"));
        }
    }



}
