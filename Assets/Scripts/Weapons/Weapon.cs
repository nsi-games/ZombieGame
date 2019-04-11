using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    RIFLE = 0,
    PISTOL = 1
}

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public Transform weaponFollowPoint;
    public Transform weaponAimPoint;
    public float lerpSmoothness = 0.5f;
    [Header("Settings")]
    public float timeBetweenShots = 0.15f;
    public float effectsDisplayTime = 0.2f;
    public float damage = 100f;
    public float range = 100f;
    public float recoil = 10f;
    public float inaccuracy = 1;
    public LayerMask shootableMask;
    public WeaponType weaponType;

    [Header("Audio")]
    [Range(0, 3)]
    public float minPitch = 1;
    [Range(0, 3)]
    public float maxPitch = 1;

    [Header("IK Settings")]
    public Transform rightHandIKTarget;
    public Transform leftHandIKTarget;

    [Header("Weapon Barrel End")]
    public Transform weaponBarrelEnd;

    private Light faceLight;
    private ParticleSystem weaponParticles;
    private LineRenderer weaponLine;
    private AudioSource weaponAudio;
    private Light weaponLight;
    
    private Ray shootRay = new Ray();
    private bool canFire = true;
    private bool isArmed = true;

    private Animator anim;
    
    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        faceLight = weaponBarrelEnd.GetComponentInChildren<Light>();
        weaponParticles = weaponBarrelEnd.GetComponent<ParticleSystem>();
        weaponLine = weaponBarrelEnd.GetComponent<LineRenderer>(); 
        weaponAudio = weaponBarrelEnd.GetComponent<AudioSource>();
        weaponLight = weaponBarrelEnd.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isArmed)
        {
            transform.position = Vector3.Lerp(transform.position, weaponFollowPoint.transform.position, Time.deltaTime * lerpSmoothness);
            if (canFire)
            {
                transform.LookAt(weaponAimPoint);
            }
        }

        anim.speed = 1 - timeBetweenShots;
        anim.SetFloat("Recoil", inaccuracy);
    }

    // Update is called once per frame
    public bool Fire(out RaycastHit hit)
    {
        if (canFire)
        {
            // Randomize audio pitch
            weaponAudio.pitch = Random.Range(minPitch, maxPitch);

            // Perform innacuracy
            Vector3 lookAtPos = weaponAimPoint.position + Random.insideUnitSphere * inaccuracy;
            weaponBarrelEnd.LookAt(lookAtPos);
            transform.LookAt(lookAtPos);

            canFire = false;
            StartCoroutine(IFireDelay(timeBetweenShots));
            StartCoroutine(IEffectsDelay(timeBetweenShots * effectsDisplayTime));
            return ShootProjectile(out hit);
        }
        // Default
        hit = new RaycastHit();
        return false;
    }

    IEnumerator IFireDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canFire = true;
    }

    IEnumerator IEffectsDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableEffects();
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        weaponLine.enabled = false;
        faceLight.enabled = false;
        weaponLight.enabled = false;
    }

    bool ShootProjectile(out RaycastHit hit)
    {
        anim.SetTrigger("Fire");

        // Play the gun shot audioclip.
        weaponAudio.Play();

        // Enable the lights.
        weaponLight.enabled = true;
        faceLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        weaponParticles.Stop();
        weaponParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        weaponLine.enabled = true;
        weaponLine.SetPosition(0, weaponLine.transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = weaponLine.transform.position;
        shootRay.direction = weaponLine.transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out hit, range, shootableMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            //EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            //
            //// If the EnemyHealth component exist...
            //if (enemyHealth != null)
            //{
            //    // ... the enemy should take damage.
            //    enemyHealth.TakeDamage(damagePerShot, shootHit.point, owner);
            //}

            // Set the second position of the line renderer to the point the raycast hit.
            weaponLine.SetPosition(1, hit.point);
            return true;
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            weaponLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            return false;
        }
    }

    public void Arm()
    {
        isArmed = true;
    }

    public void Disarm()
    {
        isArmed = false;
    }
}
