using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackObjectsManager : MonoBehaviour
{
    // Stack of objects
    [SerializeField] private Stack<StackableObject> stackObjects = new Stack<StackableObject>();

    [SerializeField] private float lerpSpeedPos = 0.1f;
    [SerializeField] private float lerpSpeedRot = 5f;

    // Vertical offset between each object in the stack
    [SerializeField] private float yOffset = 1.0f;

    [SerializeField] private Transform stackTransform = null;

    private Vector3 lastPosition;


    void Start()
    {
        if (stackTransform == null)
        {
            stackTransform = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stackObjects.Count > 0)
        {
            MoveStackObjects();
        }
    }



    #region STACK OBJECTS
    public void AddToStack(StackableObject obj)
    {
        stackObjects.Push(obj);
    }

    public StackableObject RemoveFromStack()
    {
        return stackObjects.Pop();
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
                obj.UpdateStackObjectRotation(dir, lerpSpeedRot);
                Debug.Log("HELP: " + dir);
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
