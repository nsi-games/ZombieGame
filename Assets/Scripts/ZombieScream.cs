using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZombieScream : MonoBehaviour
{
    public AudioClip[] zombieScreams;
    public float minSoundWaitTime = 5;
    public float maxSoundWaitTime = 100;
    public float soundTimer = 0;

    private float currentWaitTime = 0;
    private AudioSource sound;

    // Use this for initialization
    void Awake()
    {
        currentWaitTime = GetRandomWaitTime();
        sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        soundTimer += Time.deltaTime;
        if(soundTimer >= currentWaitTime)
        {
            soundTimer = 0f;
            currentWaitTime = GetRandomWaitTime();
            sound.clip = zombieScreams[Random.Range(0, zombieScreams.Length)];
            sound.Play();
        }
    }
    
    float GetRandomWaitTime()
    {
        return Random.Range(minSoundWaitTime, maxSoundWaitTime);
    }
}
