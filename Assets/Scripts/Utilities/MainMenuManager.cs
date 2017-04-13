using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public List<Item> items = new List<Item>();
    public List<Item> tanks = new List<TankObject>().ConvertAll(x => (Item)x);
    public List<Item> cats = new List<Cat>().ConvertAll(x => (Item)x);
    public List<Item> hats = new List<Hat>().ConvertAll(x => (Item)x);
    public List<Item> inventory = new List<Item>();

    public List<Text> cashText = new List<Text>();
    public List<Text> usernameText = new List<Text>();
    public List<Text> levelText = new List<Text>();
    public List<Text> xpText = new List<Text>();

    public GameObject catHolder, tankHolder, hatHolder, myCamera;
    public List<GameObject> cameraPos;
    public MenuState myState;

    public float camSpeed, camRotateSpeed;
    //public GameObject 

    // Use this for initialization
    void Start() {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        ControlCamera();
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
        foreach (Item a in items)
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

    public void EquipTank(int tankID)
    {
        if (CurrentPurrofile.Instance != null)
        {
            CurrentPurrofile.Instance.EquipTank(tankID);
        }
        FindObjectOfType<NetworkManager>().defaultTank = tankID;

    }

    public void EquipHat(int hatID)
    {
        if (CurrentPurrofile.Instance != null)
        {
            CurrentPurrofile.Instance.equippedHat = hatID;
        }
        FindObjectOfType<NetworkManager>().defaultHat = hatID;

    }

    public void EquipCat(int catID)
    {
        if (CurrentPurrofile.Instance != null)
        {
            CurrentPurrofile.Instance.equippedCat = catID;
        }
        FindObjectOfType<NetworkManager>().defaultCat = catID;

    }

    public void ChangeExpactions(int playerCount)
    {
        NetworkManager.Instance.expectedPlayerCount = playerCount;
    }

    public void ConnectToGame(string RoomType)
    {
        NetworkManager.Instance.ConnectToRoom(RoomType);
    }

    public void PushButton()
    {
        AudioManager.instance.PlayRandomCatSound();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeState(int state)
    {
        myState = (MenuState)state;
    }

    public void ControlCamera()
    {
        switch (myState)
        {
            case MenuState.MAIN:
                if (myCamera.transform.position != cameraPos[0].transform.position)
                {
                    Debug.Log("Moving Camera");

                    myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraPos[0].transform.position, camSpeed * Time.deltaTime);
                    myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, cameraPos[0].transform.rotation, camSpeed * Time.deltaTime);
                }
                myCamera.transform.Rotate(new Vector3(0, camRotateSpeed * Time.deltaTime, 0));
                break;

            case MenuState.GLASS:
                if (myCamera.transform.position != cameraPos[1].transform.position || myCamera.transform.rotation != cameraPos[1].transform.rotation)
                {
                    Debug.Log("Moving Camera");

                    myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraPos[1].transform.position, camSpeed * Time.deltaTime);
                    myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, cameraPos[1].transform.rotation, camSpeed * Time.deltaTime);
                }
                break;

            case MenuState.LAZER:
                if (myCamera.transform.position != cameraPos[2].transform.position || myCamera.transform.rotation != cameraPos[2].transform.rotation)
                {
                    Debug.Log("Moving Camera");

                    myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraPos[2].transform.position, camSpeed * Time.deltaTime);
                    myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, cameraPos[2].transform.rotation, camSpeed * Time.deltaTime);
                }
                break;

            case MenuState.TANK:
                if (myCamera.transform.position != cameraPos[3].transform.position || myCamera.transform.rotation != cameraPos[3].transform.rotation)
                {
                    Debug.Log("Moving Camera");

                    myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraPos[3].transform.position, camSpeed * Time.deltaTime);
                    myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, cameraPos[3].transform.rotation, camSpeed * Time.deltaTime);
                }
                break;


            case MenuState.PLAY:
                if (myCamera.transform.position != cameraPos[4].transform.position || myCamera.transform.rotation != cameraPos[4].transform.rotation)
                {
                    myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraPos[4].transform.position, camSpeed * Time.deltaTime);
                    myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, cameraPos[4].transform.rotation, camSpeed * Time.deltaTime);
                }
                break;

            case MenuState.HAT:
                if (myCamera.transform.position != cameraPos[5].transform.position || myCamera.transform.rotation != cameraPos[5].transform.rotation)
                {
                    myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraPos[5].transform.position, camSpeed * Time.deltaTime);
                    myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, cameraPos[5].transform.rotation, camSpeed * Time.deltaTime);
                }
                break;

            case MenuState.CAT:
                if (myCamera.transform.position != cameraPos[6].transform.position || myCamera.transform.rotation != cameraPos[6].transform.rotation)
                {
                    myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, cameraPos[6].transform.position, camSpeed * Time.deltaTime);
                    myCamera.transform.rotation = Quaternion.RotateTowards(myCamera.transform.rotation, cameraPos[6].transform.rotation, camSpeed * Time.deltaTime);
                }
                break;

            default:

                break;
        }
    }

    public void GoToCredits()
    {
        Application.LoadLevel("Credits");
    }

    public enum MenuState
    {
        MAIN,
        GLASS,
        LAZER,
        TANK,
        PLAY,
        HAT,
        CAT
    }
}
