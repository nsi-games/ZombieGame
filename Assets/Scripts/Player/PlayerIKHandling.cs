using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class PlayerIKHandling : MonoBehaviour
{
    public float globalIKWeight;
    public PlayerWeaponManager pWeaponManager;
    
    [Header("IK Elbow")]
    public Transform leftElbowHint;
    public Transform rightElbowHint;

    [Header("IK Hand")]
    public float leftElbowHintWeight;
    public float rightElbowHintWeight;
    public Transform leftHandIKTarget;
    public Transform rightHandIKTarget;

    [Header("IK Look")]
    public float lookIKWeight;
    public float bodyWeight;
    public float headWeight;
    public float eyesWeight;
    public float clampWeight;

    public Transform lookPosition;

    private Player player;
    private Animator anim;

    void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) || 
           Input.GetKey(KeyCode.RightAlt) || 
           Input.GetKey(KeyCode.LeftAlt))
        {
            globalIKWeight = 0;
            bodyWeight = 0.1f;
            headWeight = 0.4f;
        }
        else
        {
            globalIKWeight = 1f;
            bodyWeight = 1f;
            headWeight = 1f;
        }

        if (globalIKWeight == 0)
        {
            pWeaponManager.Disarm();
        }
        else
        {
            pWeaponManager.Arm();
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtWeight(lookIKWeight * globalIKWeight, bodyWeight * globalIKWeight, headWeight * globalIKWeight, eyesWeight * globalIKWeight, clampWeight * globalIKWeight);
        anim.SetLookAtPosition(lookPosition.position);

        // Hand IK
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, globalIKWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, globalIKWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);

        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, globalIKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, globalIKWeight);

        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);

        // Elbow IK
        anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftElbowHintWeight * globalIKWeight);
        anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightElbowHintWeight * globalIKWeight);

        anim.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowHint.position);
        anim.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowHint.position);
    }

    public void SetHandTargets(Transform rightHandIKTarget, Transform leftHandIKTarget)
    {
        this.rightHandIKTarget = rightHandIKTarget;
        this.leftHandIKTarget = leftHandIKTarget;
    }
}
