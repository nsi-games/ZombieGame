using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerWeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;
    public Transform hipGunPos;
    public Transform aimGunPos;

    [Header("Armed")]
    public Transform weaponAimPoint;

    [Header("Disarmed")]
    public Transform jointToAttachTo;
    public Transform jointToLookAt;

    private List<Weapon> weapons;
    private bool isArmed = false;
    private bool isAiming = false;

    [HideInInspector] public Player player;
    [HideInInspector] public PlayerIKHandling IK;

    // Update is called once per frame
    void Start()
    {
        player = GetComponentInParent<Player>();
        IK = GetComponentInParent<PlayerIKHandling>();
        weapons = new List<Weapon>(GetComponentsInChildren<Weapon>(true));
        SelectWeapon(currentWeapon);
    }
    
    public void SelectWeapon(Weapon selectedWeapon)
    {
        currentWeapon = selectedWeapon;
        // Deactivate all other weapons in inventory
        foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        // Activate the selected weapon
        currentWeapon.gameObject.SetActive(true);
        IK.SetHandTargets(currentWeapon.rightHandIKTarget, currentWeapon.leftHandIKTarget);

        // Default values
        currentWeapon.weaponFollowPoint = hipGunPos;
        currentWeapon.weaponAimPoint = weaponAimPoint;
    }
    
    public void Aim()
    {
        if (!isAiming)
        {
            currentWeapon.weaponFollowPoint = aimGunPos;
            isAiming = true;
        }
    }

    public void Hip()
    {
        if (isAiming)
        {
            currentWeapon.weaponFollowPoint = hipGunPos;
            isAiming = false;
        }
    }
    
    public void Arm()
    {
        if (!isArmed)
        {
            currentWeapon.Arm();
            currentWeapon.transform.SetParent(transform);
            isArmed = true;
        }
    }

    public void Disarm()
    {
        if (isArmed)
        {
            currentWeapon.Disarm();
            currentWeapon.transform.SetParent(jointToAttachTo);
            currentWeapon.transform.position = jointToAttachTo.position;
            currentWeapon.transform.LookAt(jointToLookAt);
            isArmed = false;
        }
    }

    public void Fire()
    {
        RaycastHit hit;
        // Check if the weapon fired hit something
        if (currentWeapon.Fire(out hit))
        {
            ZombieHealth zombieHealth = hit.collider.GetComponent<ZombieHealth>();
            if(zombieHealth != null)
            {
                zombieHealth.TakeDamage(currentWeapon.damage, hit, player);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerWeaponManager))]
class PlayerWeaponManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("[Test] Apply Current Weapon"))
        {
            PlayerWeaponManager pWeaponManager = (PlayerWeaponManager)target;
            pWeaponManager.SelectWeapon(pWeaponManager.currentWeapon);
        }
    }
}
#endif
