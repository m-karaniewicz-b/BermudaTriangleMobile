using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionUI : MonoBehaviour
{
    private int indexInShop;

    public Button buyButton;
    public Image iconDisplay;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI descriptionDisplay;
    public TextMeshProUGUI priceDisplay;
    public GameObject soldOverlay;
    public GameObject availableOverlay;

    public static event Action<int> OnOptionSelected;

    public bool isSold;

    private void Awake()
    {
        isSold = false;
        CheckReferences();
    }

    public void CheckReferences()
    {
        if (buyButton == null ||
            iconDisplay == null ||
            nameDisplay == null ||
            descriptionDisplay == null ||
            priceDisplay == null ||
            soldOverlay == null ||
            availableOverlay == null)
            Debug.LogError($"Missing reference in {GetType()} named {gameObject.name}");
    }

    public void SetData(int index, string itemName, string itemDesc, int itemPrice, Sprite itemIcon)
    {
        SetSold(false);
        indexInShop = index;

        nameDisplay.text = itemName;
        descriptionDisplay.text = itemDesc;
        priceDisplay.text = itemPrice.ToString();

        if (itemIcon != null) iconDisplay.sprite = itemIcon;
    }

    public void Select()
    {
        SetSold(true);
        OnOptionSelected?.Invoke(indexInShop);
    }

    public void SetSold(bool sold)
    {
        SetAvailability(!sold);
        isSold = sold;
        soldOverlay.SetActive(sold);
    }

    public void SetAvailability(bool isAvailable)
    {
        if (!isSold)
        {
            availableOverlay.SetActive(!isAvailable);
            buyButton.enabled = isAvailable;
        }
    }
}
