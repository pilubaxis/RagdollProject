using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollStackableObject : StackableObject
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        doWhenStack += DoOnStack;
        
    }

    public void DoOnStack()
    {
        rb.useGravity = false;
        //rb.MoveRotation(Quaternion.Euler(-90, rb.transform.rotation.y, rb.transform.rotation.z));
        //rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Apply the rotation
        //rb.transform.Rotate(new Vector3(-90, 0, 0));
        //DisableChildRigidbodies();
    }
    public void DoOnThrow()
    {
        rb.useGravity = true;
    }

    private void DisableChildRigidbodies()
    {
        Rigidbody[] childRigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childRigidbodies)
        {
            //rb.isKinematic = true; // Set child Rigidbody to kinematic to disable physics
            //rb.mass = 0;
            //rb.useGravity = false;
            //rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
