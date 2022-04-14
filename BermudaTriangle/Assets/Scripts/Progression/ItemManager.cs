using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public const int MAXIMUM_SHOP_SIZE = 8;
    
    [SerializeField] private List<PremadeItemData> premadeItemObjects;
    [SerializeField] private ItemShopUI shopUI;

    [HideInInspector] public List<ItemData> ownedItems;
    private List<ItemData> buyableItems;


    private void Awake()
    {
        GameManager.Instance.OnUpgradeMenuStart += PopulateUpgradeShop;

        ownedItems = new List<ItemData>();
        buyableItems = new List<ItemData>();
    }

    public void GrantItem(ItemData item)
    {
        PayItemCosts(item);
        ownedItems.Add(item);
        UpgradeManager.Instance.ApplyItem(item);
    }

    private void PayItemCosts(ItemData item)
    {
        GameManager.Instance.SetMoney(GameManager.Instance.moneyTotal - item.costMoney);
        GameManager.Instance.SetLivesCurrent(GameManager.Instance.livesCurrent - item.costLivesCurrent);
        GameManager.Instance.SetLifeContainers(GameManager.Instance.lifeContainers - item.costLifeContainers);
    }

    private void PopulateUpgradeShop()
    {
        int itemCount = 8;//UnityEngine.Random.Range(1, 9);
        
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
            vals[i] = (buyableItems[i].costMoney <= GameManager.Instance.moneyTotal &&
                buyableItems[i].costLifeContainers < GameManager.Instance.lifeContainers &&
                buyableItems[i].costLivesCurrent < GameManager.Instance.livesCurrent);
        }
        return vals;
    }
}

