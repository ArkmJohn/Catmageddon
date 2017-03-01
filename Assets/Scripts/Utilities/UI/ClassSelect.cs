using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Button))]
public class ClassSelect : MonoBehaviour {

    public CTTStart myStart;

    public void SelectTank(int id)
    {
        myStart.tankID = id;

    }
}
