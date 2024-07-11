using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public float forceMagnitude = 10f;
    public GameObject hitFX = null;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        EnemyRagdoll ragdoll = other.GetComponent<EnemyRagdoll>();

        // Get the direction vector from the player to the other object
        Vector3 playerPosition = transform.parent.parent.parent.position;
        Vector3 direction = (other.transform.position - playerPosition).normalized + Vector3.up;

        //Middle
        Vector3 fxPos = other.transform.position + Vector3.up;

        Vector3 rotatedDirection = direction *forceMagnitude;

        if (ragdoll)
        {
            ragdoll.ActivateRagdoll(true, rotatedDirection);
            Instantiate(hitFX, fxPos, Quaternion.identity);
        }

        if (rb && other.CompareTag("Hitable"))
        {
            rb.AddForce(rotatedDirection, ForceMode.Impulse);
            Instantiate(hitFX, fxPos, Quaternion.identity);
        }
    }
}
