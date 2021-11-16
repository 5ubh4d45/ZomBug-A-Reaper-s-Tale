using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    public Transform aimTransform;
    private Camera cam;

    private void Start()
    {
        if (aimTransform == null)
        {
            aimTransform = GetComponent<Transform>();
        }
    }

    void Update()
    {
        UpdateTargetRotation();
    }

    public void UpdateTargetRotation()
    {
        //gets mouse pos and convert to world then sets the weapon angle towards mouse
        Vector3 moussePos = cam != null ? cam.ScreenToWorldPoint(Input.mousePosition) : Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (moussePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }
}
