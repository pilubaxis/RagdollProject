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
    [Header("Line Renderer")]
    [SerializeField] private LineRenderer forceLine = null;
    [SerializeField] private Gradient colorLineWhenChargeThrow = null;
    [SerializeField] private Gradient colorLineDisable = null;
    [SerializeField] private float rotationImpulseIntensity = 4f;

    public GameObject throwButton = null;

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
        }

        if (!stackObjectsManager.CheckStackIsEmpty())
        {
            UpdateLineRenderer(true, isIncreasingForce);
            UpdateForceDirection();
            throwButton.SetActive(true);
        }
        else
        { 
            UpdateLineRenderer(false,false);
            throwButton.SetActive(false);
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

    private void UpdateLineRenderer(bool active, bool isThrowing)
    {
        // show line
        forceLine.gameObject.SetActive(active);

        if (active)
        {
            Transform obj = stackObjectsManager.CheckNextFromStack().objectTransform;
            forceLine.SetPosition(0, obj.transform.position);
            Vector3 endPoint = isThrowing ? 
                obj.transform.position + forceDirection * currentForceMagnitude :
                obj.transform.position + forceDirection * minForceMagnitude
                ;
            forceLine.SetPosition(1, endPoint);

            // line color
            forceLine.colorGradient = isThrowing ? colorLineWhenChargeThrow : colorLineDisable;
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
