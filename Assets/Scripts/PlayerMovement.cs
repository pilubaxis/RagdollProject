using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform modelTransform;
    public float moveSpeed = 5f;
    public float moveRotSpeed = 10f;

    [Range(0,1)] public float moveSpeedFactorWhenHit = 0.3f;

    private Rigidbody rb;
    private PlayerBehaviour playerBehaviour;

    public Vector2 moveInput;

    public bool isMoving {
        get { return moveInput.magnitude > 0; }
    }

    public Vector3 playerDirection {
        get { return modelTransform.transform.forward; }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
        moveInput = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
        
    }

    private void FixedUpdate()
    {
        // Get the camera's forward and right vectors
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Project forward and right vectors onto the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate the desired move direction based on camera orientation
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // Move the player
        Vector3 move = moveDirection * moveSpeed * (playerBehaviour.isHitting ? moveSpeedFactorWhenHit : 1) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate the player model to face the move direction if there is any input
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, Time.fixedDeltaTime * moveRotSpeed);
        }
    }
}
