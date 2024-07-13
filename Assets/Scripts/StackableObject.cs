using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StackableObject : MonoBehaviour
{
    public StackableObjectState state = StackableObjectState.Available;
    public Transform objectTransform = null;
    public Rigidbody rigidBody { get { return objectTransform.GetComponent<Rigidbody>();  } }

    [SerializeField] private float lerpSpeedGoToStack = 4f;

    [Header("Conditions To Stack")]
    // The range that the object needs to stack in player
    [SerializeField] private float playerDistanceToStack = 1.5f;

    //The period of time that the player needs to stay in range to stack 
    [SerializeField] private float timeToStack = 1f;

    // the period of time that the objects need to be available again
    [SerializeField] private float timeToBeAvailable = 2f;

    [SerializeField] private Vector3 rotationOffset;

    public UnityAction doWhenStack = null;
    public UnityAction doWhenThrow = null;
    public UnityAction doWhenDisposed = null;

    protected Rigidbody rb = null;

    private float timerAvailable = 0;
    private float timerThrown = 0;
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
            player = GameManager.Instance.player;
        }
    }

    public void Update()
    {
        //when objects is waiting to be collected
        switch (state)
        {
            case StackableObjectState.Available:
                if (GetDistanceFromPlayer() <= playerDistanceToStack)
                {
                    timerAvailable += Time.deltaTime;

                    if (timerAvailable >= timeToStack)
                    {
                        if (player.GetComponent<StackObjectsManager>().checkIfStackIsFull())
                        {
                            timerAvailable = 0;
                            return;
                        }
                        //Adding to stack
                        WhenStack();
                        //Player ref
                        player.GetComponent<StackObjectsManager>().AddToStack(this);
                    }
                }
                break;
            case StackableObjectState.Unavailable:
                break;
            case StackableObjectState.Stacked:
                break;
            case StackableObjectState.Thrown:

                timerThrown += Time.deltaTime;

                if (timerThrown >= timeToStack)
                {
                    WhenAvailable();
                }
                break;
        }
    }

    public void WhenStack()
    {
        if (doWhenStack != null)
        {
            doWhenStack.Invoke();
        }
        state = StackableObjectState.Stacked;
        rb.isKinematic = true;
        timerAvailable = 0;
    }

    public void WhenThrow()
    {
        if (doWhenThrow != null)
        {
            doWhenThrow.Invoke();
        }
        state = StackableObjectState.Thrown;
        rb.isKinematic = false;
    }

    public void WhenDisposed(int money)
    {
        if (doWhenDisposed!= null)
        {
            doWhenDisposed.Invoke();
        }
        state = StackableObjectState.Disposed;
        player.GetComponent<PlayerEconomy>().ManageCoin(money);
    }

    public void WhenAvailable()
    {
        state = StackableObjectState.Available;
        timerThrown = 0;
    }

    public void UpdateStackObjectPosition(Vector3 targetPos, float lerpDelay)
    {
        Vector3 currentPosition = rb.position;

        // Move only in the y-axis until it's close to the yThreshold
        if (Mathf.Abs(currentPosition.y - targetPos.y) > 1f)
        {
            // Lerp towards the target position in the y-axis only
            Vector3 newYPosition = new Vector3(currentPosition.x, targetPos.y, currentPosition.z);
            Vector3 lerpedPosition = Vector3.Lerp(currentPosition, newYPosition, lerpDelay/2 * Time.deltaTime);
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
        timerAvailable = 0;
    }


    public enum StackableObjectState
    {
        Unavailable,
        Available,
        Stacked,
        Thrown,
        Disposed
    }
}
