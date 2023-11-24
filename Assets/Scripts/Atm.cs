using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atm : MonoBehaviour
{
    public GameObject Ui;
    private PlayerController controll;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Ui.SetActive(true);
            controll.UnLock();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Ui.SetActive(false);
            controll.Lock();
        }
    }


    void FixedUpdate()
    {
        if (controll == null)
            controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (Ui == null)
        {
            Ui = GameObject.FindGameObjectWithTag("AtmUi");
            Ui.SetActive(false);
        }
    }
}
