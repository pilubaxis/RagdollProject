using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public Transform player = null;

    public List<ShopManager> shops = new List<ShopManager>();

    [Header("Canvas")]
    public TextMeshProUGUI coinsText = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public ShopManager CheckStoreAvailable()
    {
        ShopManager available = null;
        for (int i = 0; i < shops.Count; i++)
        {
            if (shops[i].IsShopActive())
            {
                available = shops[i];
            }
        }
        return available;
    }
}