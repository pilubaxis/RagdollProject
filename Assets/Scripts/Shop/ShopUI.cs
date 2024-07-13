using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopUI : MonoBehaviour
{
    [Header("Character Color Change")]
    [SerializeField] private int ChangeColorCost = 5;
    [SerializeField] private GameObject optionPrefab = null;
    [SerializeField] private Transform colorOptionParent = null;
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private List<ColorOptionUI> colorOptions;

    [Header("Stack Limit")]
    [SerializeField] private int IncreaseStackLimitCost = 10;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI stackCount = null;
    [SerializeField] private Image playerColor = null;

    private Transform player;

    private void Awake()
    {
        if (player == null)
        {
            player = GameManager.Instance.player;
        }

        UpdatePlayerInfo();
    }

    private void Start()
    {
        UpdateColorOptions();
    }

    public void UpdateColorOptions()
    {
        for (int i = 0; i < colors.Count; i ++)
        {
            ColorOptionUI option = Instantiate(optionPrefab, Vector3.zero, Quaternion.identity, colorOptionParent).GetComponent<ColorOptionUI>();
            colorOptions.Add(option);

            option.color = colors[i];
            option.price = ChangeColorCost;
            option.SetUpButton();
            option.button.onClick.AddListener(UpdatePlayerInfo);
        }
    }

    public void AddNewStack()
    {
        PlayerEconomy economy = player.GetComponent<PlayerEconomy>();
        if (economy.CheckIfHaveMoney(IncreaseStackLimitCost))
        {
            economy.ManageCoin(-IncreaseStackLimitCost);
            player.GetComponent<PlayerUpgrader>().IncreaseStack();
            UpdatePlayerInfo();
        }
    }

    public void UpdatePlayerInfo()
    {
        stackCount.text = player.GetComponent<StackObjectsManager>().stackLimit.ToString();
        playerColor.color = player.GetComponent<PlayerUpgrader>().GetPlayerColor();
    }
}
