using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDis : MonoBehaviour
{

    public GameObject buttonObject;

    public void disable()
    { 
        buttonObject.SetActive(false);    //set button gameobject false
    }
}
