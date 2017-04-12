using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurrofileUIHandler : MonoBehaviour {

    public GameObject LoginPanel, SignUpPanel, LoadParticle;

    public InputField loginUsername;
    public InputField loginPassword;

    public InputField signupEmail;
    public InputField signupUsername;
    public InputField signupPassword;

    public void Login()
    {
        Debug.Log(loginUsername.text.ToString() + " Logging In");
        string loginUser = loginUsername.text.ToString();
        string loginPass = loginPassword.text.ToString();
        AudioManager.instance.PlayRandomCatSound();
        if (loginUser != "" || loginPass != "")
        {
            LoadUp();
            PurrofileHandler.Instance.LogMeIn(loginUser, loginPass);
        }
        else
        {
            Debug.Log("Input Fields are missing Input!");
            ReloadLogIn();
        }
    }

    public void SignUp()
    {
        Debug.Log(loginUsername.text.ToString() + " Logging In");
        AudioManager.instance.PlayRandomCatSound();
        string signupUser = signupUsername.text.ToString();
        string signupPass = signupPassword.text.ToString();
        string signupE = signupEmail.text.ToString();
        if (signupUser == "" || signupPass == "" || signupE == "")
        {
            Debug.Log("Input Fields are missing Input!");
            ReloadSignUp();
        }
        else
            PurrofileHandler.Instance.SignMeUp(signupE, signupUser, signupPass);
    }

    public void LoadUp()
    {
        SignUpPanel.SetActive(false);
        LoginPanel.SetActive(false);
        LoadParticle.SetActive(true);
    }

    public void ReloadSignUp()
    {
        SignUpPanel.SetActive(true);
        LoadParticle.SetActive(false);
    }

    public void ReloadLogIn()
    {
        LoginPanel.SetActive(true);
        LoadParticle.SetActive(false);
    }
}
