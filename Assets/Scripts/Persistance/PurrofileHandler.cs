using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PurrofileHandler : MonoBehaviour {

    private static PurrofileHandler instance = new PurrofileHandler();

    [SerializeField]
    private int ID;
    [SerializeField]
    private Text _usernameLG;
    [SerializeField]
    private Text _passwordLG;
    [SerializeField]
    private Text _usernameSU;
    [SerializeField]
    private Text _passwordSU;
    [SerializeField]
    private Text _emailSU;
    [SerializeField]
    private string _currentInventory;

    [SerializeField]
    private string _hash = "meowmix";
    [SerializeField]
    private string _defaultItemIDs = "1";

    public List<int> profileItemId = new List<int>();

    public bool isLoggedIn = false;

    #region URLs
    //[SerializeField]
    private string _findURL = "https://catmageddon.000webhostapp.com/CatmageddonPHP/Find.php";
    //[SerializeField]
    private string _editURL = "https://catmageddon.000webhostapp.com/CatmageddonPHP/Edit.php";
    //[SerializeField]
    private string _sighnupURL = "https://catmageddon.000webhostapp.com/CatmageddonPHP/Create.php";
    //[SerializeField]
    private string _loginURL = "https://catmageddon.000webhostapp.com/CatmageddonPHP/Login.php";
    //[SerializeField]
    private string _eCheckURL = "https://catmageddon.000webhostapp.com/CatmageddonPHP/emailcheck.php";
    #endregion

    /* OLD
    #region OLDVARIABLES
    // URLs please
    [SerializeField]
    private string _invupURL = "https://catmageddon.000webhostapp.com/CatmageddonPHP/purripareinventory.php";

    #endregion
    #region OLDCODE
    [ContextMenu("Log in using Values")]
    public void Login()
    {
        StartCoroutine(LogInToDB(_usernameLG.text, _passwordLG.text));
    }

    [ContextMenu("Create users using Values")]
    public void SignUp()
    {
        StartCoroutine(CreateUser(_usernameSU.text, _passwordSU.text, _emailSU.text));
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
            ID = int.Parse(www.text);
            StartCoroutine(SetupCurrentPlayer(username));
        }
        else
            SendWarning("Wrong username/password.");
    }

    public IEnumerator CreateUser(string username, string password, string email)
    {
        if (!IsEmailAddress(email))
        {
            SendWarning("Email ID is not valid");
            yield return null;
        }
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
            SendWarning("Email Not Available");
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
            profileItemId.Add(int.Parse(val));
        }
        profileItemId.Sort();
        yield return values;
    }

    public IEnumerator AddItem(string a)
    {
        yield return null;
    }

    #endregion
    */

    #region Instance and things
    static PurrofileHandler()
    {
    }

    private PurrofileHandler()
    {
    }

    public static PurrofileHandler Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region Unity.Callbacks
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
    }
    #endregion

    #region Public.Functions

    // Load MainMenu Scene
    public void GoToMainMenu()
    {
        SendWarning("Loading Main");
        SceneManager.LoadScene("MainMenu");
    }

    // Functions to call the IEnumerators
    public void LogMeIn(string InputUsername, string InputPassword)
    {
        StartCoroutine(Login(InputUsername, InputPassword));
    }

    public void SignMeUp(string InputEmail, string InputUsername, string InputPassword)
    {
        StartCoroutine(Signup(InputUsername, InputPassword, InputEmail));
    }
    
    // Changes a text to display a warning
    public void SendWarning(string myMessage)
    {
        ErrorText[] myWarningTexts = FindObjectsOfType<ErrorText>();

        if (myWarningTexts.Length == 0)
            return;

        foreach (ErrorText a in myWarningTexts)
        {
            a.SetWarning(myMessage);
        }

    }

    public void Win()
    {
        StartCoroutine(ChangeValues(ID, "cash", "100"));
        StartCoroutine(ChangeValues(ID, "experience", "10"));
    }

    public void Lose()
    {
        StartCoroutine(ChangeValues(ID, "cash", "50"));
        StartCoroutine(ChangeValues(ID, "experience", "5"));
    }

    #endregion

    #region Private.Functions
    #endregion

    #region IEnumerators.Network
    public IEnumerator Signup(string username, string password, string email)
    {
        if (CheckEmail.IsEmail(email))
        {
            // Checks wether the email is available
            WWWForm emailCheckForm = new WWWForm();
            emailCheckForm.AddField("cat_emailToCheckPost", email);
            WWW emailCheckWWW = new WWW(_eCheckURL, emailCheckForm);
            yield return emailCheckWWW;

            if (emailCheckWWW.text == "EMAIL_A")
            {
                // Create Here
                WWWForm form = new WWWForm();
                form.AddField("cat_userPost", username);
                form.AddField("cat_passPost", password);
                form.AddField("cat_emailPost", email);
                form.AddField("cat_defaultItemsPost", _defaultItemIDs);
                WWW www = new WWW(_sighnupURL, form);

                yield return www;
                StartCoroutine(Login(username, password));
                Debug.Log(www.text);
            }
            else
            {
                gameObject.GetComponent<PurrofileUIHandler>().ReloadSignUp();
                SendWarning("Email Not Available");
            }
        }
        else
        {
            gameObject.GetComponent<PurrofileUIHandler>().ReloadSignUp();
            SendWarning("Not a proper Email");
        }
    }

    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        // Inputs for the PHP script
        form.AddField("cat_hashPost", _hash);
        form.AddField("cat_userPost", username);
        form.AddField("cat_passPost", password);

        WWW www = new WWW(_loginURL, form);

        yield return www;

        Debug.Log(www.text);
        if (www.text != "FAIL")
        {
            ID = int.Parse(www.text);
            CurrentPurrofile.Instance.SetID(ID);
            StartCoroutine(Setup(ID, username));
        }
        else
        {
            SendWarning("Wrong username/password.");
            gameObject.GetComponent<PurrofileUIHandler>().ReloadLogIn();
        }

    }

    public IEnumerator Setup(int id, string username)
    {
        string inventory;
        string equipped;
        int cashCount = 0;
        int levelCount = 0;
        int xpCount = 0;

        // Gets inventory
        WWWForm inventoryForm = new WWWForm();
        inventoryForm.AddField("cat_id", ID);
        inventoryForm.AddField("cat_field", "inventory");

        WWW invWWW = new WWW(_findURL, inventoryForm);
        yield return invWWW;

        Debug.Log("You have the ids: " + invWWW.text);
        inventory = invWWW.text;

        // Gets the equipped inventory
        WWWForm equipForm = new WWWForm();
        equipForm.AddField("cat_id", ID);
        equipForm.AddField("cat_field", "equipment");

        WWW equipWWW = new WWW(_findURL, equipForm);
        yield return equipWWW;
        
        Debug.Log("You have equipped ids: " + equipWWW.text);
        equipped = equipWWW.text;

        // Get Cash
        WWWForm cashForm = new WWWForm();
        cashForm.AddField("cat_id", ID);
        cashForm.AddField("cat_field", "cash");

        WWW cashWWW = new WWW(_findURL, cashForm);
        yield return cashWWW;

        Debug.Log("We have " + cashWWW.text + " cash");
        int.TryParse(cashWWW.text, out cashCount);

        // Gets LVL
        WWWForm levelForm = new WWWForm();
        levelForm.AddField("cat_id", ID);
        levelForm.AddField("cat_field", "level");

        WWW levelWWW = new WWW(_findURL, levelForm);
        yield return levelWWW;

        Debug.Log("Our level is " + levelWWW.text);
        int.TryParse(levelWWW.text, out levelCount);

        // Gets the current XP
        WWWForm xpForm = new WWWForm();
        xpForm.AddField("cat_id", ID);
        xpForm.AddField("cat_field", "experience");

        WWW xpWWW = new WWW(_findURL, xpForm);
        yield return xpWWW;

        Debug.Log("Our xp is " + xpWWW.text);
        int.TryParse(xpWWW.text, out xpCount); 

        CurrentPurrofile.Instance.SetUpCurrentPlayer(inventory, equipped, cashCount, levelCount, xpCount, username);

    }

    public IEnumerator ChangeValues(int id, string field, string value)
    {
        switch (field)
        {
            case "inventory":
                // Update Locally
                CurrentPurrofile.Instance.inventory.Add(int.Parse(value));
                CurrentPurrofile.Instance.invStringHolder = CurrentPurrofile.Instance.invStringHolder + " " + value;
                // Update In the server
                WWWForm invEditForm = new WWWForm();
                invEditForm.AddField("cat_id", ID);
                invEditForm.AddField("cat_field", field);
                invEditForm.AddField("cat_value", CurrentPurrofile.Instance.invStringHolder);

                WWW invEdit = new WWW(_editURL, invEditForm);
                yield return invEdit;
                Debug.Log("Changed the Inventory by adding " + invEdit.text);
                break;

            case "equipment":
                // Update Locally
                CurrentPurrofile.Instance.equipped.Add(int.Parse(value));
                CurrentPurrofile.Instance.invStringHolder = CurrentPurrofile.Instance.eqStringHolder + " " + value;
                // Update In the server
                WWWForm eqEditForm = new WWWForm();
                eqEditForm.AddField("cat_id", ID);
                eqEditForm.AddField("cat_field", field);
                eqEditForm.AddField("cat_value", CurrentPurrofile.Instance.eqStringHolder);

                WWW eqEdit = new WWW(_editURL, eqEditForm);
                yield return eqEdit;
                Debug.Log("Changed the Equipment by adding " + eqEdit.text);
                break;

            case "cash":
                // Update Locally
                CurrentPurrofile.Instance.cash += int.Parse(value);
                // Update In the server
                WWWForm cashEditForm = new WWWForm();
                cashEditForm.AddField("cat_id", ID);
                cashEditForm.AddField("cat_field", field);
                cashEditForm.AddField("cat_value", CurrentPurrofile.Instance.cash);

                WWW cashEdit = new WWW(_editURL, cashEditForm);
                yield return cashEdit;
                Debug.Log("Changed the Cash by adding " + cashEdit.text);
                break;

            case "level":
                // Update Locally
                CurrentPurrofile.Instance.level += int.Parse(value);
                // Update In the server
                WWWForm levelEditForm = new WWWForm();
                levelEditForm.AddField("cat_id", ID);
                levelEditForm.AddField("cat_field", field);
                levelEditForm.AddField("cat_value", CurrentPurrofile.Instance.level);

                WWW levelEdit = new WWW(_editURL, levelEditForm);
                yield return levelEdit;
                Debug.Log("Changed the Level by adding " + levelEdit.text);
                break;

            case "experience":
                // Update Locally
                CurrentPurrofile.Instance.xp += int.Parse(value);
                // Update In the server
                WWWForm xpEditForm = new WWWForm();
                xpEditForm.AddField("cat_id", ID);
                xpEditForm.AddField("cat_field", field);
                xpEditForm.AddField("cat_value", CurrentPurrofile.Instance.xp);

                WWW xpEdit = new WWW(_editURL, xpEditForm);
                yield return xpEdit;
                Debug.Log("Changed the XP by adding " + xpEdit.text);
                break;
        }

        yield return null;

    }

    #endregion


}
