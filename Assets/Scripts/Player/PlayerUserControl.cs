using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerUserControl : MonoBehaviour
{
    public bool isAutoRunning;

    private Player player;
    private bool isCrouching;
    private bool isRunning;
    private bool isJumping;

    void Awake()
    {
        player = GetComponent<Player>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            isAutoRunning = !isAutoRunning;
        }
        if (Input.GetKeyDown(KeyCode.C)) isCrouching = !isCrouching;
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isJumping = Input.GetKeyDown(KeyCode.Space);
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(inputH, 0, inputV);

        if (inputH != 0 || inputV != 0) {
            isAutoRunning = false;
        }

        if(isAutoRunning) {
            movement = Vector3.forward;
            isRunning = true;
        }

        movement = transform.TransformDirection(movement);
        player.Move(movement, isCrouching, isRunning, isJumping);
    }
}
