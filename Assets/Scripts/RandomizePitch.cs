using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomizePitch : MonoBehaviour
{
    [Range(0, 3)]
    public float minPitch = 0;

    [Range(0, 3)]
    public float maxPitch = 3;

    private AudioSource sound;

    // Use this for initialization
    void Awake()
    {
        sound = GetComponent<AudioSource>();
        sound.pitch = Random.Range(minPitch, maxPitch);
    }
}
