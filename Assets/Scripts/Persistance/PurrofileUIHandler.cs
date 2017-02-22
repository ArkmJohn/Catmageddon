using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurrofileUIHandler : MonoBehaviour {

    public PurrofileHandler pHandler;
    public InputField loginUsername;
    public InputField loginPassword;

    public InputField signupEmail;
    public InputField signupUsername;
    public InputField signupPassword;

    void Awake()
    {
        pHandler = FindObjectOfType<PurrofileHandler>();
    }
    public void Login()
    {
        Debug.Log(loginUsername.text.ToString() + " Logging In");
        string loginUser = loginUsername.text.ToString();
        string loginPass = loginPassword.text.ToString();

        if (loginUser != "" || loginPass != "")
            StartCoroutine(pHandler.LogInToDB(loginUser, loginPass));
        else
            Debug.Log("Input Fields are missing Input!");
    }

    public void SignUp()
    {
        Debug.Log(loginUsername.text.ToString() + " Logging In");
        string signupUser = signupUsername.text.ToString();
        string signupPass = signupPassword.text.ToString();
        string signupE = signupEmail.text.ToString();

        StartCoroutine(pHandler.CreateUser(signupUser, signupPass, signupE));
    }
}
