using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorOptionUI : MonoBehaviour
{
    public bool isLocked = true;
    public Color color;
    public int price;

    [SerializeField] private Image sprite = null;
    public Button button = null;
    [SerializeField] private TextMeshProUGUI costText = null;
    private Transform player;

    void Start()
    {
        player = GameManager.Instance.player;
    }

    public void SetUpButton()
    {
        sprite.color = color;
        costText.text = "$"+price;
        button.onClick.AddListener(InteractColor);
    }

    public void InteractColor()
    {
        PlayerEconomy eco = player.GetComponent<PlayerEconomy>();
        PlayerUpgrader upgrader = player.GetComponent<PlayerUpgrader>();

        if (isLocked)
        {
            if (eco.CheckIfHaveMoney(price))
            {
                eco.ManageCoin(-price);
                upgrader.ChangeBodyMaterial(color);
                isLocked = false;
                costText.gameObject.SetActive(false);
            }
        }
        else
        {
            upgrader.ChangeBodyMaterial(color);
        }
    }
}
