using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAimWeapon : MonoBehaviour
{
    public Transform aimTransform;
    [SerializeField] private Player player;

    public Transform AimTransform => aimTransform;

    private Camera cam;
    private float _targetAngle;
    private float _targetAngleLeft;
    private float _targetAngleRight;
    private float _finalAngle;

    private void Start()
    {
        if (aimTransform == null)
        {
            aimTransform = GetComponent<Transform>();
        }

        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
        cam = Camera.main;
    }

    void Update()
    {
        UpdateTargetRotation();

        AimWeapon();
    }

    public void UpdateTargetRotation()
    {
        //gets mouse pos and convert to world then sets the weapon angle towards mouse
        Vector3 moussePos = cam != null ? cam.ScreenToWorldPoint(Input.mousePosition) : Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (moussePos - transform.position).normalized;
        _targetAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        //clamping the angle



        if (player.PlayerMovement.IsFacingRight)
        {
            _targetAngleRight = Mathf.Clamp(_targetAngle, -130f, -50f);
            _finalAngle = _targetAngleRight;

            //flip the sprite
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (player.PlayerMovement.IsFacingLeft)
        {
            //breaking into positive and the negetive components (angles are confusing af)
            float plusAngle = Mathf.Clamp(_targetAngle, 50f, 90f);
            float minusAngle = Mathf.Clamp(_targetAngle, -270f, -220f);

            if (_targetAngle > 0)
            {
                _targetAngleLeft = plusAngle;
            }

            if (_targetAngle < 0)
            {
                _targetAngleLeft = minusAngle;
            }
            _finalAngle = _targetAngleLeft;

            //flip the sprite
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

    }

    private void AimWeapon()
    {
        aimTransform.eulerAngles = new Vector3(0, 0, _finalAngle);

    }
}
