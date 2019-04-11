using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
[RequireComponent(typeof(Animator))]
public class ZombieHealth : MonoBehaviour
{
    public GameObject[] hitEffects;
    public float deathDuration = 0.5f;
    public float maxHealth = 100f;

    private float currentHealth = 100f;

    private bool isDead = false;

    private Zombie zombie;
    private Animator anim;

    void Awake()
    {
        zombie = GetComponent<Zombie>();
        anim = GetComponent<Animator>();    
    }

    public void TakeDamage(float damage, RaycastHit hit, Player hitPlayer)
    {
        if (isDead) return;

        // Decrease health with damage dealt
        currentHealth -= damage;
        // Spawn hit point (more math required)
        GameObject hitEffect = Instantiate(hitEffects[Random.Range(0, hitEffects.Length)]);
        hitEffect.transform.position = hit.point;
        hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);

        if(currentHealth <= 0)
        {
            zombie.Disable();
            anim.SetTrigger("Death");
            isDead = true;
        }
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
