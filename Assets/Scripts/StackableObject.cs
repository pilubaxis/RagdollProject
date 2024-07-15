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

    public Vector3 rotationOffset;

    [Header("Conditions to Available")]

    // the period of time that the objects need to be available again
    [SerializeField] private float timeToBeAvailable = 2f;

    [Header("Disposed")]
    [SerializeField] private float timeToValidateDispoed = 1f;

    [Header("Going to stack")]
    [SerializeField] float goingToStackSpeed = 7f;
    public float moveDelay = 1f;

    public Vector3 previousPosition;

    public UnityAction doWhenStack = null;
    public UnityAction doWhenThrow = null;
    public UnityAction doWhenDisposed = null;

    protected Rigidbody rb = null;

    private float timerAvailable = 0;
    private float timerThrown = 0;
    private float timerDisposed = 0;
    private Transform player = null;
    private int moneyToGetWhenExplode = 0;

    private bool isDisposed = false;
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
                    if (!player.GetComponent<StackObjectsManager>().checkIfStackIsFull())
                    {
                        timerAvailable += Time.deltaTime;
                    }

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
            case StackableObjectState.Disposed:
                if (isDisposed)
                {
                    timerDisposed += Time.deltaTime;

                    if (timerDisposed >= timeToValidateDispoed)
                    {
                        player.GetComponent<PlayerEconomy>().ManageCoin(moneyToGetWhenExplode);
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    public void WhenAvailable()
    {
        state = StackableObjectState.Available;
        timerThrown = 0;
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
        moneyToGetWhenExplode = money;
        isDisposed = true;
        state = StackableObjectState.Disposed;
    }

    public void UpdateStackObjectPosition(Vector3 targetPos, float rate)
    {
        Vector3 currentPosition = rb.position;

        if (Mathf.Abs(currentPosition.y - targetPos.y) > 0.5f)
        {
            Vector3 newYPosition = new Vector3(currentPosition.x, targetPos.y, currentPosition.z);
            Vector3 newPos = Vector3.MoveTowards(currentPosition, newYPosition, goingToStackSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
        }
        else
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, goingToStackSpeed/4 * Time.deltaTime);

            if (state == StackableObjectState.GoingToStack)
            {
                rb.MovePosition(newPos);
                if (Vector3.Distance(rb.position, targetPos) < 0.01f)
                {
                    state = StackableObjectState.Stacked;
                }
            }
            else if (state == StackableObjectState.Stacked)
            {
                rb.MovePosition(Vector3.Lerp(currentPosition, targetPos, rate));
            }

        }
    }

    public void UpdateStackObjectRotation(Vector3 targetDirection, Vector3 delta, float lerpSpeedRot)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation *= Quaternion.Euler(delta);

        targetRotation *= Quaternion.Euler( rotationOffset);

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
        GoingToStack,
        Stacked,
        Thrown,
        Disposed
    }
}
