using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour {

    public GameObject ItemHolderPrefab;
    public GameObject itemInputHolder;
    public List<Item> itemsHeld = new List<Item>();

    public void AddItem(Item itemToHold)
    {
        if (!itemsHeld.Contains(itemToHold))
        {
            itemsHeld.Add(itemToHold);
            GameObject itemHolder = Instantiate(ItemHolderPrefab, itemInputHolder.transform);
            itemHolder.GetComponent<ItemHolder>().nameText.text = itemToHold.name;
            itemHolder.GetComponent<ItemHolder>().costText.text = itemToHold.Cost.ToString();
            if (itemToHold.Icon != null)
            {
                itemHolder.GetComponent<ItemHolder>().icon.sprite = itemToHold.Icon.sprite;
            }
        }
    }
}
