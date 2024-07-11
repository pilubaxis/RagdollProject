using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBehaviour : MonoBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Hit
    }

    public State currentState;
    public Animator animator;

    public UnityAction onIdleEnter;
    public UnityAction onIdleUpdate;
    public UnityAction onIdleExit;

    public UnityAction onWalkEnter;
    public UnityAction onWalkUpdate;
    public UnityAction onWalkExit;

    public UnityAction onHitEnter;
    public UnityAction onHitUpdate;
    public UnityAction onHitExit;

    private State previousState;

    private void Start()
    {
        previousState = currentState;
        EnterState(currentState);
    }

    public void Update()
    {
        if (previousState != currentState)
        {
            ExitState(previousState);
            EnterState(currentState);
            previousState = currentState;
        }

        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Walk:
                Walk();
                break;
            case State.Hit:
                Hit();
                break;
        }
    }

    private void EnterState(State state)
    {
        switch (state)
        {
            case State.Idle:
                onIdleEnter?.Invoke();
                break;
            case State.Walk:
                onWalkEnter?.Invoke();
                break;
            case State.Hit:
                onHitEnter?.Invoke();
                break;
        }
    }

    private void ExitState(State state)
    {
        switch (state)
        {
            case State.Idle:
                onIdleExit?.Invoke();
                break;
            case State.Walk:
                onWalkExit?.Invoke();
                break;
            case State.Hit:
                onHitExit?.Invoke();
                break;
        }
    }

    public void Idle()
    {
        onIdleUpdate?.Invoke();
    }

    public void Walk()
    {
        onWalkUpdate?.Invoke();
    }

    public void Hit()
    {
        onHitUpdate?.Invoke();
    }

    #region ANIMATION
    public string GetCurrentAnimationName()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return "Idle";
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            return "Walk";
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return "Hit";
        }
        else
        {
            return "Unknown";
        }
    }
    #endregion
}
