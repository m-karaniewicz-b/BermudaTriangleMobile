using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public const int MAXIMUM_SHOP_SIZE = 8;



}

public class ItemData
{
    internal float seed;
    public string name;
    public string description;
    public int price;


    public ItemData()
    {
        seed = Random.value;

        name = $"Generated item";
        description = $"Seed: {seed}";
        price = Random.Range(0, 150);
    }

}