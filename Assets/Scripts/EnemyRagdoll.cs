using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    public Rigidbody bodyToReciveForce = null;
    [SerializeField] private StackableObject stackableObject = null;
    private CapsuleCollider hitColliderDetector = null;
    private Animator animator;
    private RagdollStackableObject ragdollStackableObject = null;
    [SerializeField] private string[] animationStates;
    [SerializeField] private bool startAsRagdoll = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        hitColliderDetector = GetComponent<CapsuleCollider>();
        ragdollStackableObject = GetComponent<RagdollStackableObject>();
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        PlayRandomAnimation();

        if (startAsRagdoll)
        {
            ActivateRagdoll(true, Vector3.zero);
        }
    }
    public void ActivateRagdoll(bool activate, Vector3 dir)
    {
        ragdollStackableObject.state = StackableObject.StackableObjectState.Available;
        animator.enabled = !activate;
        hitColliderDetector.enabled = !activate;
        stackableObject.enabled = activate;

        bodyToReciveForce.AddForce(dir, ForceMode.Impulse);
    }

    public void PlayRandomAnimation()
    {
        if (animationStates.Length == 0)
        {
            return;
        }
        int randomIndex = Random.Range(0, animationStates.Length);
        string randomAnimation = animationStates[randomIndex];

        animator.Play(randomAnimation);
    }
}
