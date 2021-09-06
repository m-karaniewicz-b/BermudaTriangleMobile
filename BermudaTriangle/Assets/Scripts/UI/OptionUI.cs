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

    public static event Action<int> OptionSelected;

    private void Awake()
    {
        CheckReferences();
    }

    public void CheckReferences()
    {
        if (buyButton == null || 
            iconDisplay == null || 
            nameDisplay == null || 
            descriptionDisplay == null || 
            priceDisplay == null ||  
            soldOverlay == null)
            Debug.LogError($"Missing reference in {GetType()} named {gameObject.name}");
    }

    public void SetData(int index, string name, string description, int price)
    {
        indexInShop = index;
        nameDisplay.text = name;
        descriptionDisplay.text = description;
        priceDisplay.text = price.ToString();
    }

    public void Select()
    {
        SetSelectedOverlayActive(true);
        OptionSelected?.Invoke(indexInShop);
    }

    private void SetSelectedOverlayActive(bool sold)
    {
        soldOverlay.SetActive(sold);
        buyButton.enabled = !sold;
    }
}
