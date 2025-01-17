using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool died = false;
    public GameObject respawnObject;

    

    public void Update()
    {
        respawnObject = GameObject.FindGameObjectWithTag("Respawn");
        if (died == true)
        {
            respawnObject.gameObject.SetActive(true);
        }
    }

    public void Pressed()
    {
      respawnObject.gameObject.SetActive(false);
    }
}
