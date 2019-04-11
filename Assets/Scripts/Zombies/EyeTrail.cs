using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrail : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    // Use this for initialization
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
