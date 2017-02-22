using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurrofileHandler : MonoBehaviour {

    [SerializeField]
    private float ID;
    [SerializeField]
    private string _username;
    [SerializeField]
    private string _password;
    [SerializeField]
    private string _email;
    [SerializeField]
    private string _currentInventory;

    [SerializeField]
    private string _hash = "meowmix";
    [SerializeField]
    private string _defaultItemIDs = "1";

    // URLs please
    [SerializeField]
    private string _invupURL = "https://catmageddon.000webhostapp.com/catmageddonphp/purripareinventory.php";
    [SerializeField]
    private string _sighnupURL = "https://catmageddon.000webhostapp.com/catmageddonphp/catcreate.php";
    [SerializeField]
    private string _loginURL = "https://catmageddon.000webhostapp.com/catmageddonphp/catnect.php";
    [SerializeField]
    private string _eCheckURL = "https://catmageddon.000webhostapp.com/catmageddonphp/emailcheck.php";
    
    public List<float> profileItemId = new List<float>();

    public bool isLoggedIn = false;

    [ContextMenu("Log in using Values")]
    void Login()
    {
        StartCoroutine(LogInToDB(_username, _password));
    }

    [ContextMenu("Create users using Values")]
    void SignUp()
    {
        StartCoroutine(CreateUser(_username, _password, _email));
    }

    public IEnumerator LogInToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        // Gives info to the php script
        form.AddField("cat_hashPost", _hash);
        form.AddField("cat_userPost", username);
        form.AddField("cat_passPost", password);

        WWW www = new WWW(_loginURL, form);
        yield return www;
        Debug.Log(www.text);
        // Sets up current player
        if (www.text != "FAIL")
        {
            ID = float.Parse(www.text);
            StartCoroutine(SetupCurrentPlayer(username));
        }
    }

    public IEnumerator CreateUser(string username, string password, string email)
    {
        // Checks wether the email is available
        WWWForm form = new WWWForm();
        form.AddField("cat_emailToCheckPost", email);
        WWW www = new WWW(_eCheckURL, form);
        yield return www;
        if (www.text == "EMAIL_A")
        {
            StartCoroutine(CreateProfile(username, password, email));
        }
        else
        {
            Debug.Log("Email not available");
        }
    }

    IEnumerator CreateProfile(string username, string password, string email)
    {
        // Creates the profile in the server
        WWWForm form = new WWWForm();
        form.AddField("cat_userPost", username);
        form.AddField("cat_passPost", password);
        form.AddField("cat_emailPost", email);
        form.AddField("cat_defaultItemsPost", _defaultItemIDs);
        WWW www = new WWW(_sighnupURL, form);
        yield return www;
        Debug.Log(www.text);
        StartCoroutine(LogInToDB(username, password));
    }

    IEnumerator SetupCurrentPlayer(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("cat_userPost", username);

        WWW www = new WWW(_invupURL, form);
        yield return www;
        StartCoroutine(SetupInventory(www.text));
    }

    IEnumerator SetupInventory(string a)
    {
        _currentInventory = a;
        profileItemId.Clear();
        Debug.Log("Getting Items");
        string[] values = a.Split(',');
        foreach (string val in values)
        {
            profileItemId.Add(float.Parse(val));
        }
        profileItemId.Sort();
        yield return values;
    }

    public IEnumerator AddItem(string a)
    {
        yield return null;
    }
}
