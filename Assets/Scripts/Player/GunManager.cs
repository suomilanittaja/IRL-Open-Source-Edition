using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunManager : MonoBehaviourPunCallbacks
{
    public PlayerController controller;
    public PickUp pickUp;
    public GameObject Ui;

    void Update()
    {
      if (controller == null)
      {
        controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        pickUp = GameObject.FindWithTag("Player").GetComponent<PickUp>();
      }

      if (controller.tabPressed == true)
      {
        Ui.SetActive(true);
      }

      if (controller.tabPressed == false)
      {
        Ui.SetActive(false);
      }
    }

    public void useGun()
    {
      if (controller.hasGun == true)
      {
        pickUp.usingGun = true;
      }
    }

    public void useHand()
    {
      pickUp.usingGun = false;
    }

}
