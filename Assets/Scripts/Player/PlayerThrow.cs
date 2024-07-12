using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private float minForceMagnitude = 5;
    [SerializeField] private float maxForceMagnitude = 15;
    [SerializeField] private float forceIncreaseRate = 1f;
    [SerializeField] private float yIncrease = 1f;
    [SerializeField] private LineRenderer forceLine = null;
    [SerializeField] private float rotationImpulseIntensity = 4f;

    private float currentForceMagnitude;
    private bool isIncreasingForce = false;

    private Vector3 forceDirection; // players facing direction with y variation

    private PlayerMovement playerMovement = null;
    private StackObjectsManager stackObjectsManager = null;

    public UnityAction onStartThrow = null;
    public UnityAction onEndThrow = null;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        stackObjectsManager = GetComponent<StackObjectsManager>();
    }

    private void Update()
    {
        if (isIncreasingForce)
        {
            IncreaseForceMagnitude();
            UpdateForceDirection();
            UpdateLineRenderer(true);
            Debug.DrawRay(transform.position, forceDirection * currentForceMagnitude, Color.red);
        }
    }

    private void IncreaseForceMagnitude()
    {
        currentForceMagnitude += forceIncreaseRate * Time.deltaTime;
        currentForceMagnitude = Mathf.Clamp(currentForceMagnitude, minForceMagnitude, maxForceMagnitude);
    }

    private void UpdateForceDirection()
    {
        Vector3 playerDirection = playerMovement.playerDirection;
        forceDirection = new Vector3(playerDirection.x, playerDirection.y + yIncrease, playerDirection.z).normalized;
    }

    private void UpdateLineRenderer(bool show)
    {
        forceLine.gameObject.SetActive(show);

        if (!stackObjectsManager.CheckStackIsEmpty())
        {
            Transform obj = stackObjectsManager.CheckNextFromStack().objectTransform;
            forceLine.SetPosition(0, obj.transform.position);
            Vector3 endPoint = obj.transform.position + forceDirection * currentForceMagnitude;
            forceLine.SetPosition(1, endPoint);
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (stackObjectsManager.CheckStackIsEmpty()) return;

        if (context.started)
        {
            isIncreasingForce = true;
        }
        else if (context.canceled)
        {
            StackableObject obj = stackObjectsManager.RemoveFromStack();
            obj.WhenThrow();
            ExecuteThrow(obj.objectTransform.GetComponent<Rigidbody>());
            isIncreasingForce = false;
            currentForceMagnitude = minForceMagnitude;
            UpdateLineRenderer(false);
        }
    }

    public void ExecuteThrow(Rigidbody rb)
    {
        rb.AddForce(forceDirection * currentForceMagnitude, ForceMode.Impulse);

        //Random rot inpulse (unique throw)
        Vector3 randomRotationImpulse = new Vector3(
        Random.Range(-1f, 1f),
        Random.Range(-1f, 1f),
        Random.Range(-1f, 1f)
    ) * rotationImpulseIntensity;

        rb.AddTorque(randomRotationImpulse, ForceMode.Impulse);
    }
}
