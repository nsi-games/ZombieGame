using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerShooting : MonoBehaviour
{
    public PlayerWeaponManager pWeaponManager;
    [Header("Aim Settings")]
    public Camera playerCamera;
    public float aimFOV = 30f;
    public float aimSmoothness = 0.3f;

    private float prevFOV = 60;

    private Animator anim;
    private float aimWeight = 0;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        prevFOV = playerCamera.fieldOfView;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        anim.SetLayerWeight(1, aimWeight);
        anim.SetFloat("WeaponType", (float)pWeaponManager.currentWeapon.weaponType);

        if (Input.GetMouseButton(0))
        {
            pWeaponManager.Fire();
        }
        if (Input.GetMouseButton(1))
        {
            aimWeight = Mathf.Lerp(aimWeight, 1, aimSmoothness);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, aimFOV, aimSmoothness);
            pWeaponManager.Aim();
        }
        else
        {
            aimWeight = Mathf.Lerp(aimWeight, 0, aimSmoothness);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, prevFOV, aimSmoothness);
            pWeaponManager.Hip();
        }
    }
}
