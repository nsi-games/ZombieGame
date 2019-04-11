using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public enum MovementState
    {
        NULL,
        IDLE,
        WANDERING,
        WALKING,
        RUNNING
    }
    
    public float wanderSpeed = 0.35f;
    public float walkingSpeed = 0.52f;
    public float runningSpeed = 1f;
    public Transform target;
    public MovementState movementState;
    public int maxWalkType = 4;
    public int maxRunType = 2;
    
    public int walkType = 0;
    public int runType = 0;

    private bool isEnabled = true;

    NavMeshAgent agent;
    Animator anim;
    Vector3 prevPosition;

    void Awake()
    {
        walkType = Random.Range(0, maxWalkType);
        runType = Random.Range(0, maxRunType);
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        prevPosition = transform.position;
        anim.SetFloat("CycleOffset", Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && isEnabled)
            agent.SetDestination(target.position);
        else
            agent.Stop();

        switch (movementState)
        {
            case MovementState.NULL:
                break;
            case MovementState.IDLE:
                break;
            case MovementState.WANDERING:
                agent.speed = wanderSpeed;
                break;
            case MovementState.WALKING:
                agent.speed = walkingSpeed;
                break;
            case MovementState.RUNNING:
                agent.speed = runningSpeed;
                break;
            default:
                break;
        }

        Animation();
    }

    void Animation()
    {
        anim.SetInteger("MovementState", (int)movementState);
        anim.SetFloat("WalkType", walkType);
        anim.SetFloat("RunType", runType);
        prevPosition = transform.position;
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }
}
