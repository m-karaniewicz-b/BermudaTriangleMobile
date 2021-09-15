using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New premade item", menuName = "Data/Premade Item")]
public class PremadeItemData : ScriptableObject
{
    public ItemData item;

    public static implicit operator ItemData(PremadeItemData it) => it.item;

    [Button]
    private void ResetAllValues()
    {
        item = new ItemData();
    }
}
