using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f; 
    public float rotationSpeed = 5.0f;

    public Vector2 deadZoneSize = new Vector2(2f, 2f);

    private Vector3 currentVelocity;
    private Vector3 targetPosition;

    private void LateUpdate()
    {
        targetPosition = player.position;
        // Calculate the dead zone boundaries
        Vector3 deadZoneCenter = transform.position;
        float leftBound = deadZoneCenter.x - deadZoneSize.x / 2;
        float rightBound = deadZoneCenter.x + deadZoneSize.x / 2;
        float bottomBound = deadZoneCenter.z - deadZoneSize.y / 2;
        float topBound = deadZoneCenter.z + deadZoneSize.y / 2;

        // Check if the player is outside the dead zone
        if (player.position.x < leftBound || player.position.x > rightBound || player.position.z < bottomBound || player.position.z > topBound)
        {
            // Smoothly move the camera towards the target position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the dead zone in the editor
        Gizmos.color = Color.yellow;
        Vector3 deadZoneCenter = transform.position;
        Gizmos.DrawWireCube(new Vector3(deadZoneCenter.x, player.position.y, deadZoneCenter.z), new Vector3(deadZoneSize.x, 0, deadZoneSize.y));
    }
}
