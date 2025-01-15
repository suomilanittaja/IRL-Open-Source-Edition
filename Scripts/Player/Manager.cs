using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool died = false;
    public GameObject Respawn;

    

    public void Update()
    {
        Respawn = GameObject.FindGameObjectWithTag("Respawn");
        if (died == true)
      {
            Respawn.gameObject.SetActive(true);
      }
    }

    public void Pressed()
    {
      Respawn.gameObject.SetActive(false);
    }
}
