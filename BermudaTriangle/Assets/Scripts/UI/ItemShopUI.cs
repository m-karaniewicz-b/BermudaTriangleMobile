using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemShopUI : MonoBehaviour
{

    public OptionUI[] optionDiplays;

    public static event Action<ItemData> ItemAcquired;

    private ItemData[] buyableItems;

    private void Awake()
    {
        OptionUI.OptionSelected += OptionSelected;
    }

    public void PopulateShop(ItemData[] items)
    {
        buyableItems = items;

        EmptyShop();

        for (int i = 0; i < items.Length; i++)
        {
            optionDiplays[i].gameObject.SetActive(true);
            optionDiplays[i].SetData(i,items[i].name,items[i].description,items[i].price);
        }
    }

    public void EmptyShop()
    {
        for (int i = 0; i < optionDiplays.Length; i++)
        {
            optionDiplays[i].gameObject.SetActive(false);
        }
    }

    public void OptionSelected(int optionIndex)
    {
        ItemAcquired?.Invoke(buyableItems[optionIndex]);
    }

    [Button(ButtonSizes.Medium), GUIColor(0.6f, 0.6f, 1)]
    private void GenerateTestItems()
    {
        int itemCount = UnityEngine.Random.Range(1, 9);

        ItemData[] testItems = new ItemData[itemCount];

        for (int i = 0; i < itemCount; i++)
        {
            testItems[i] = new ItemData();
        }

        PopulateShop(testItems);
    }

    
}

