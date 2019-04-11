using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFinish : MonoBehaviour
{
    private ParticleSystem[] particles;
    private AudioSource[] sounds;

    // Use this for initialization
    void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        sounds = GetComponentsInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFinished())
        {
            Destroy(gameObject);
        }
    }

    bool IsFinished()
    {
        foreach (ParticleSystem particle in particles)
        {
            if (particle.IsAlive())
                return false;
        }

        foreach (AudioSource sound in sounds)
        {
            if (sound.isPlaying)
                return false;
        }

        return true;
    }
}
