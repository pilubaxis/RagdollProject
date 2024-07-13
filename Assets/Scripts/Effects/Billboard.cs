using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        FaceCamera();
    }

    private void FaceCamera()
    {
        if (mainCamera != null)
        {
            Vector3 direction = mainCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
