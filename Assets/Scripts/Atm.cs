using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atm : MonoBehaviour
{
    public GameObject Ui;   //Ui for ATM
    private PlayerController controll;   //Player controller script


    void OnTriggerEnter(Collider other)  //when something enter close to atm
    {
        if (other.gameObject.CompareTag("Player")) //check if it  is player
        {
            Ui.SetActive(true);
            controll.ToggleCursorLock(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Ui.SetActive(false);
            controll.ToggleCursorLock(true);
        }
    }


    void FixedUpdate()
    {
        if (controll == null) //find player
            controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (Ui == null)  //find ui
        {
            Ui = GameObject.FindGameObjectWithTag("AtmUi");
            Ui.SetActive(false);
        }
    }
}
