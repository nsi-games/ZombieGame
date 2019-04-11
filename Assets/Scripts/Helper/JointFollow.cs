using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointFollow : MonoBehaviour
{
    public Transform jointToFollow;
    public Vector3 offset;
    public bool includeRotation = false;

    public bool isEnabled = true;

    // Update is called once per frame
    void LateUpdate()
    {
        if (jointToFollow != null && isEnabled)
        {
            transform.position = jointToFollow.position + jointToFollow.TransformDirection(offset);
            if (includeRotation)
                transform.rotation = jointToFollow.rotation;
        }
    }
}
