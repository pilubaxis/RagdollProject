using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private float distanceToActivatePopUp = 1.5f;
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject shopPopUp;
    private Transform player = null;
    private float playerDistance;
    void Start()
    {
        player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        popUp.SetActive(playerDistance < distanceToActivatePopUp);
    }

    public void OpenStoreUI(bool open)
    {
        shopPopUp.SetActive(open);
    }

    public bool IsShopActive()
    {
        return popUp.activeSelf;
    }
}
