using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSpawner : MonoBehaviour
{
    public GameObject eyePrefab;

    // Use this for initialization
    void Start()
    {
        Instantiate(eyePrefab, transform, false);
    }
}
