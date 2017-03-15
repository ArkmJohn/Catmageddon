using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public List<Item> items = new List<Item>();
    public List<Item> tanks = new List<TankObject>().ConvertAll(x => (Item)x);
    public List<Item> cats = new List<Cat>().ConvertAll(x => (Item)x);
    public List<Item> hats = new List<Hat>().ConvertAll(x => (Item)x);
    public List<Item> inventory = new List<Item>();

    public List<Text> cashText = new List<Text>();
    public List<Text> usernameText = new List<Text>();
    public List<Text> levelText = new List<Text>();
    public List<Text> xpText = new List<Text>();

    public GameObject catHolder, tankHolder, hatHolder;

    //public GameObject 

	// Use this for initialization
	void Start () {
        Setup();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup()
    {
        UpdateTexts(cashText, "C: " + CurrentPurrofile.Instance.cash.ToString());
        UpdateTexts(usernameText, CurrentPurrofile.Instance.username);
        UpdateTexts(levelText, "Level: " + CurrentPurrofile.Instance.level.ToString());
        UpdateTexts(xpText, CurrentPurrofile.Instance.xp.ToString() + "/ 100");
        RemoveBought();
        SplitItems();
        SetUpHolders();

    }

    public void UpdateTexts(List<Text> textToChange, string value)
    {
        foreach (Text text in textToChange)
        {
            text.text = value;
        }
    }

    void RemoveBought()
    {
        foreach (Item a in items)
        {
            if (CurrentPurrofile.Instance.inventory.Contains(a.id))
            {
                inventory.Add(a);
                //items.Remove(a);
            }
        }

    }

    void SplitItems()
    {
        foreach(Item a in items)
        {
            if (!inventory.Contains(a))
            {
                switch (a.GetType().ToString())
                {
                    case "Cat":
                        if (!cats.Contains(a))
                            cats.Add(a);
                        break;

                    case "TankObject":
                        if (!tanks.Contains(a))
                            tanks.Add(a);
                        break;

                    case "Hat":
                        if (!hats.Contains(a))
                            hats.Add(a);
                        break;
                }
            }
        }
    }

    public void SetUpHolders()
    {
        // Set up cats
        foreach (Cat a in cats)
        {
            catHolder.GetComponent<ItemContainer>().AddItem(a);
        }

        // Set up Tanks
        foreach (TankObject a in tanks)
        {
            tankHolder.GetComponent<ItemContainer>().AddItem(a);
        }

        // Set up Hats
        foreach (TankObject a in tanks)
        {
            tankHolder.GetComponent<ItemContainer>().AddItem(a);
        }
    }
}
