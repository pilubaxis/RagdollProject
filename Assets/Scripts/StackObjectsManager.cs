using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackObjectsManager : MonoBehaviour
{
    public int stackLimit = 3;
    // Stack of objects
    [SerializeField] private Stack<StackableObject> stackObjects = new Stack<StackableObject>();

    [SerializeField] private float lerpSpeedPos = 0.1f;
    [SerializeField] private float lerpSpeedRot = 5f;

    // Vertical offset between each object in the stack
    [SerializeField] private float yOffset = 1.0f;

    [SerializeField] private Transform stackTransform = null;
    [SerializeField] private CameraMovement camMov;

    [SerializeField] private float inertiaIntensity = 2;
    [SerializeField] private float rotInertiaIntensity = 2;

    private Vector3 lastPosition;


    void Start()
    {
        if (stackTransform == null)
        {
            stackTransform = transform;
        }
    }

    void LateUpdate()
    {
        if (stackObjects.Count > 0)
        {
            MoveStackObjects();
        }
    }



    #region STACK OBJECTS

    public bool CheckStackIsEmpty()
    {
        return stackObjects.Count == 0;
    }
    public void AddToStack(StackableObject obj)
    {
        stackObjects.Push(obj);

        //change zoom camera
        camMov.zoom = Math.Clamp((stackObjects.Count - 2) * 0.1f, 0f , 1f);
    }

    public StackableObject RemoveFromStack()
    {
        //change zoom camera
        camMov.zoom = Math.Clamp((stackObjects.Count - 2) * 0.1f, 0f, 1f);

        StackableObject obj = stackObjects.Pop();
        obj.state = StackableObject.StackableObjectState.Thrown;
        return obj;
    }

    public StackableObject CheckNextFromStack()
    {
        return stackObjects.Peek();
    }

    public bool checkIfStackIsFull()
    {
        return stackObjects.Count >= stackLimit;
    }

    public void MoveStackObjects()
    {
        StackableObject[] stackArray = stackObjects.ToArray();
        Array.Reverse(stackArray);

        for (int i = 0; i < stackArray.Length; i++)
        {
            //Position
            StackableObject obj = stackArray[i];
            Vector3 yHeight = new Vector3(0, i * yOffset, 0);
            float rate = lerpSpeedPos * (1 + i * inertiaIntensity);
            Vector3 lastObjPos = i == 0 ? stackTransform.position : stackArray[i - 1].objectTransform.position + new Vector3(0, yOffset, 0);

            obj.UpdateStackObjectPosition(lastObjPos, rate);

            //Rotation

            //coeficient to rotate the desire angle to rotate
            Vector2 deltaXZ = new Vector2(obj.objectTransform.position.x - lastObjPos.x, obj.objectTransform.position.z - lastObjPos.z);

            Vector3 rot = new Vector3( deltaXZ.y * rotInertiaIntensity, 0, -deltaXZ.x * rotInertiaIntensity);



            obj.UpdateStackObjectRotation(Vector3.forward, rot, lerpSpeedRot);
        }


    }
    #endregion

    public Vector3 GetTopOfStackPos()
    {
        return stackTransform.position + new Vector3(0, stackObjects.Count * yOffset, 0); 
    }
}
