using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : CharacterBehaviour
{
    public PlayerMovement playerMov = null;
    private float animationCheckDelay = 0.5f;
    private float animationCheckTimer = 0f;
    private bool animationCheckInProgress = false;
    public bool isHitting { get { return currentState == State.Hit; } }


    void Start()
    {
        onWalkEnter += OnRunStart;
        onIdleEnter += OnStop;
        onHitEnter += OnPunch;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (currentState != State.Hit)
        {
            currentState = playerMov.isMoving ? State.Walk : State.Idle;
        }
        else
        {
            // Start animation check if not already in progress
            if (!animationCheckInProgress)
            {
                animationCheckTimer += Time.deltaTime;

                // Check animation state after delay
                if (animationCheckTimer >= animationCheckDelay)
                {
                    if (GetCurrentAnimationName() != "Hit")
                    {
                        currentState = playerMov.isMoving ? State.Walk : State.Idle;
                        animator.SetBool("Run", playerMov.isMoving);
                    }
                    animationCheckInProgress = false;
                    animationCheckTimer = 0f;
                }
            }
        }
            
    }

    public void OnRunStart()
    {
        animator.SetBool("Run", true);
    }

    public void OnStop()
    {
        animator.SetBool("Run", false);
    }

    public void OnPunch()
    {
        animator.SetTrigger("Hit");
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        ShopManager shop = GameManager.Instance.CheckStoreAvailable();
        if (shop)
        {
            shop.OpenStoreUI(true);
            return;
        }
        if (context.started)
        {
            currentState = State.Hit;
        }
        
    }
}
