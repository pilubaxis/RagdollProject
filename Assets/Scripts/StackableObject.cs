using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StackableObject : MonoBehaviour
{
    public StackableObjectState state = StackableObjectState.AvaiableToStack;
    public Transform objectTransform = null;
    [SerializeField] private float lerpSpeedGoToStack = 4f;

    [Header("Conditions To Stack")]
    // The range that the object needs to stack in player
    [SerializeField] private float playerDistanceToStack = 1.5f;

    //The running time that the player needs to stay in range, to collect the stackable object
    [SerializeField] private float timeToStack = 1f;

    [SerializeField] private Vector3 rotationOffset;

    public UnityAction doWhenStack = null;

    protected Rigidbody rb = null;

    private float timer = 0;
    private Transform player = null;
    protected void Start()
    {
        if (objectTransform == null)
        {
            objectTransform = transform;
        }

        rb = objectTransform.GetComponent<Rigidbody>();

        if (player == null)
        {
            player = GameManager.instance.player;
        }
    }

    public void Update()
    {
        //if (player.GetComponent<StackObjectsManager>().)

        //when objects is waiting to be collected
        if (state == StackableObjectState.AvaiableToStack)
        {
            if (GetDistanceFromPlayer() <= playerDistanceToStack)
            {
                timer += Time.deltaTime;

                if (timer >= timeToStack)
                {
                    //Adding to stack
                    if (doWhenStack!= null)
                    {
                        doWhenStack.Invoke();
                    }

                    //Player ref
                    player.GetComponent<StackObjectsManager>().AddToStack(this);

                    rb.isKinematic = true;
                    state = StackableObjectState.Stacked;
                }
            }
        }
        else if (state == StackableObjectState.Stacked)
        {
            
        }
    }

    public void UpdateStackObjectPosition(Vector3 targetPos, float lerpDelay)
    {
        Vector3 currentPosition = rb.position;

        // Move only in the y-axis until it's close to the yThreshold
        if (Mathf.Abs(currentPosition.y - targetPos.y) > 1f)
        {
            // Lerp towards the target position in the y-axis only
            Vector3 newYPosition = new Vector3(currentPosition.x, targetPos.y, currentPosition.z);
            Vector3 lerpedPosition = Vector3.Lerp(currentPosition, newYPosition, lerpDelay * Time.deltaTime);
            rb.MovePosition(lerpedPosition);
        }
        else
        {
            // Lerp towards the target position in all axes
            Vector3 newPosition = Vector3.Lerp(currentPosition, targetPos, lerpDelay * Time.deltaTime);
            rb.MovePosition(newPosition);
        }
    }

    public void UpdateStackObjectRotation(Vector3 targetDirection, float lerpSpeedRot)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation *= Quaternion.Euler(rotationOffset);

        Quaternion lerpedRotation = Quaternion.Lerp(rb.rotation, targetRotation, lerpSpeedRot * Time.deltaTime);

    rb.MoveRotation(lerpedRotation);
    }

    private float GetDistanceFromPlayer()
    {
        return Vector3.Distance(objectTransform.position, player.position);
    }

    public void ResetObject()
    {
        timer = 0;
    }


    public enum StackableObjectState
    {
        AvaiableToStack,
        Stacked,
        Thrown
    }
}
