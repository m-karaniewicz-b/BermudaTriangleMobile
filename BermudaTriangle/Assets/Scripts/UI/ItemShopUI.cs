using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Sirenix.OdinInspector;


public class ItemShopUI : MonoBehaviour
{
    public OptionUI[] optionDiplays;

    private void Awake()
    {
        OptionUI.OnOptionSelected += SelectOption;
    }

    public void UpdateShopUI(List<ItemData> items)
    {
        for (int i = 0; i < optionDiplays.Length; i++)
        {
            optionDiplays[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Count; i++)
        {
            optionDiplays[i].gameObject.SetActive(true);
            optionDiplays[i].SetData(i, items[i].name, items[i].description, items[i].costMoney, items[i].icon);
        }

        UpdateItemAvailability();
    }

    public void UpdateItemAvailability()
    {
        bool[] values = ItemManager.Instance.GetItemAvailabilities(); 

        for (int i = 0; i < values.Length; i++)
        {
            optionDiplays[i].SetAvailability(values[i]);
        }
    }

    private void SelectOption(int optionIndex)
    {
        ItemManager.Instance.GrantItem(ItemManager.Instance.GetItemFromBuyableListIndex(optionIndex));
        UpdateItemAvailability();
    }

}

