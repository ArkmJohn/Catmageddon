using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPurrofile : MonoBehaviour {
    
    private static CurrentPurrofile instance = new CurrentPurrofile();
    public int currentID;
    public List<int> inventory;
    public string invStringHolder;
    public List<int> equipped;
    public int equippedTank;
    public string eqStringHolder;
    public string username;
    public int cash;
    public int level;
    public int xp;

    static CurrentPurrofile()
    {
    }

    private CurrentPurrofile()
    {
    }

    public static CurrentPurrofile Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetID(int id)
    {
        currentID = id;
    }

    public void SetUpCurrentPlayer(string stringInv, string stringEq, int cashI, int levelI, int xpI, string username)
    {
        invStringHolder = stringInv;
        eqStringHolder = stringEq;
        inventory = setupInvString(stringInv);
        equipped = setupInvString(stringEq);
        cash = cashI;
        level = levelI;
        xp = xpI;
        equippedTank = equipped[0];
        this.username = username;
        PurrofileHandler.Instance.GoToMainMenu();

    }

    List<int> setupInvString(string a)
    {
        List<int> temp = new List<int>();
        string[] values = a.Split(' ');
        foreach (string val in values)
        {
            temp.Add(int.Parse(val));
        }
        temp.Sort();

        return temp;
    }

    public void EquipTank(int a)
    {
        equippedTank = a;
    }
}
