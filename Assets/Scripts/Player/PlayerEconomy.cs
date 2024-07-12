using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    public int coins = 0;
    

    public void AddCoin(int qnt)
    {
        coins += qnt;
        coins = Mathf.Clamp(coins, 0, coins);
        GameManager.instance.coinsText.text = "x " + coins.ToString();
    }
}
