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

    private Vector3 lastPosition;


    void Start()
    {
        if (stackTransform == null)
        {
            stackTransform = transform;
        }
    }

    void Update()
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
        camMov.zoom = Math.Clamp((stackObjects.Count - 5) * 0.1f, 0f , 1f);
    }

    public StackableObject RemoveFromStack()
    {
        //change zoom camera
        camMov.zoom = Math.Clamp((stackObjects.Count - 5) * 0.1f, 0f, 1f);

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

            StackableObject obj = stackArray[i];
            StackableObject lastObj = i > 0 ? stackArray[i - 1] : null;
            Vector3 position = stackTransform.position + new Vector3(0, i * yOffset, 0); 

            obj.UpdateStackObjectPosition(position, lerpSpeedPos * (stackArray.Length - i + 1));


            if (lastObj != null)
            {
                Vector3 dir = (lastObj.objectTransform.position - obj.objectTransform.position).normalized;
                //obj.UpdateStackObjectRotation(dir, lerpSpeedRot);
                obj.UpdateStackObjectRotation(Vector3.down, lerpSpeedRot);
            }
            else
            {
                obj.UpdateStackObjectRotation(Vector3.down, lerpSpeedRot);
            }
        }
    }
    #endregion

    public Vector3 GetTopOfStackPos()
    {
        return stackTransform.position + new Vector3(0, stackObjects.Count * yOffset, 0); 
    }
}
