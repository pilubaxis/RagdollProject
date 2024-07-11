using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    public Rigidbody bodyToReciveForce = null;
    [SerializeField] private StackableObject stackableObject = null;
    private CapsuleCollider hitColliderDetector = null;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        hitColliderDetector = GetComponent<CapsuleCollider>();
    }
    public void ActivateRagdoll(bool activate, Vector3 dir)
    {
        animator.enabled = !activate;
        hitColliderDetector.enabled = !activate;
        stackableObject.enabled = activate;

        bodyToReciveForce.AddForce(dir, ForceMode.Impulse);
    }
}
