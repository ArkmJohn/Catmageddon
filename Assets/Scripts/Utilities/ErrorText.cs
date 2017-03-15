using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ErrorText : MonoBehaviour {



    public void SetWarning(string warningString)
    {
        GetComponent<Text>().text = warningString;
    }
}
