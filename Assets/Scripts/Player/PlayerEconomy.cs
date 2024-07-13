using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    public int coins = 0;

    private void Start()
    {
        coins = Mathf.Clamp(coins, 0, coins);
        GameManager.Instance.coinsText.text = "x " + coins.ToString();
    }

    public void ManageCoin(int qnt)
    {
        coins += qnt;
        coins = Mathf.Clamp(coins, 0, coins);
        GameManager.Instance.coinsText.text = "x " + coins.ToString();
    }


    public bool CheckIfHaveMoney(int qnt)
    {
        return coins >= qnt;
    }
}
