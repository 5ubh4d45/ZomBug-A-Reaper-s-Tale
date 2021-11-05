using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Camera cam;

    private void Start()
    {
        if (aimTransform == null){
            aimTransform = GetComponent<Transform>();
        }
        cam = Camera.main;
    }

    void Update()
    {
        //gets mouse pos and convert to world then sets the weapon angle towards mouse
        Vector3 moussePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (moussePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }
}
