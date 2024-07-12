using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;

public class BodyContainer : MonoBehaviour
{
    [SerializeField] private int coinContainerValue = 1;
    [SerializeField] private TextMeshProUGUI coinText = null;
    void Start()
    {
        coinText.text = "x" + coinContainerValue.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        StackableObject obj = other.GetComponentInParent<StackableObject>();
        if (obj != null)
        {
            if (obj.objectTransform == other.transform)
            {
                obj.WhenDisposed(coinContainerValue);
            }
        }
    }
}
