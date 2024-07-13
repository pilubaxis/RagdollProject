using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrader : MonoBehaviour
{
    [SerializeField] private List<Renderer> bodyRenderers;
    [SerializeField] private StackObjectsManager stackObjectsManager;

    public void ChangeBodyMaterial(Color newColor)
    {
        foreach (Renderer renderer in bodyRenderers)
        {
            renderer.material.color = newColor;
        }
    }

    public Color GetPlayerColor()
    {
        return bodyRenderers[0].material.color;
    }

    public void IncreaseStack()
    {
        stackObjectsManager.stackLimit++;
    }
}
