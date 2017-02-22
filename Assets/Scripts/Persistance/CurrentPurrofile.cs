using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPurrofile : MonoBehaviour {
    
    private static CurrentPurrofile instance = new CurrentPurrofile();

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
}
