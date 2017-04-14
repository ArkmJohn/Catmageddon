using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowSelect : MonoBehaviour {

    [Tooltip("Objects that will be used to glow. Note that all objects must be a child of the object that has this script.")]
    public List<GameObject> glowObjects;
    
    public void GlowObject(int index)
    {
        foreach (Transform child in transform)
        {
            Behaviour childHalo = child.gameObject.GetComponent("Halo") as Behaviour;
            childHalo.enabled = false;
        }

        GameObject toBeHalo = transform.GetChild(index).gameObject;
        if (toBeHalo.GetComponent("Halo") as Behaviour == null)
        {
            Debug.LogError("No Halo Attached to the script");
        }
        else
        {
            Behaviour halo = toBeHalo.GetComponent("Halo") as Behaviour;
            halo.enabled = true;
        }
    }
}
