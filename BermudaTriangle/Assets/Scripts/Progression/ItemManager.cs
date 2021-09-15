﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public const int MAXIMUM_SHOP_SIZE = 8;

    public List<PremadeItemData> premadeItemObjects = new List<PremadeItemData>();

    [HideInInspector] public List<ItemData> ownedItems = new List<ItemData>();
    private List<ItemData> buyableItems = new List<ItemData>();

    [SerializeField] private ItemShopUI shopUI;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        GameManager.OnUpgradeMenuStart += PopulateUpgradeShop;
    }

    public void GrantItem(ItemData item)
    {
        PayItemCosts(item);
        ownedItems.Add(item);
        UpgradeManager.instance.ApplyItem(item);
    }

    private void PayItemCosts(ItemData item)
    {
        GameManager.instance.SetMoney(GameManager.moneyTotal - item.costMoney);
        GameManager.instance.SetLivesCurrent(GameManager.livesCurrent - item.costLivesCurrent);
        GameManager.instance.SetLifeContainers(GameManager.lifeContainers - item.costLifeContainers);
    }

    private void PopulateUpgradeShop()
    {
        int itemCount = 4;//UnityEngine.Random.Range(1, 9);
        
        itemCount = Mathf.Clamp(itemCount,0,MAXIMUM_SHOP_SIZE);

        List<ItemData> newItems = new List<ItemData>();

        for (int i = 0; i < itemCount; i++)
        {
            newItems.Add(GenerateItem());
        }

        buyableItems = new List<ItemData>(newItems);

        shopUI.UpdateShopUI(buyableItems);
    }

    private ItemData GenerateItem()
    {
        ItemData newItem = new ItemData();
        newItem = premadeItemObjects[Random.Range(0,premadeItemObjects.Count)];

        if (newItem == null) Debug.LogError("Generated item error.");
        return newItem;
    }

    public ItemData GetItemFromBuyableListIndex(int index)
    {
        return buyableItems[index];
    }

    public bool[] GetItemAvailabilities()
    {
        bool[] vals = new bool[buyableItems.Count];
        for (int i = 0; i < buyableItems.Count; i++)
        {
            vals[i] = (buyableItems[i].costMoney <= GameManager.moneyTotal &&
                buyableItems[i].costLifeContainers < GameManager.lifeContainers &&
                buyableItems[i].costLivesCurrent < GameManager.livesCurrent);
        }
        return vals;
    }
}

