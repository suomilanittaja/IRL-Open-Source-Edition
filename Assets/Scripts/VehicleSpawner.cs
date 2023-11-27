using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviourPunCallbacks
{
    public Transform spawnPos;  //car spawning position
    public GameObject Ui;       //car spawning ui
    public GameObject carPrefab; //car prefab
    private PlayerController controll; //player controller


    void OnTriggerEnter (Collider other)  //when something is close to vehicle spawner
    {
      if (other.gameObject.CompareTag("Player"))    //check if it is player
      {
        Ui.SetActive(true);
        controll.ToggleCursorLock(false);
      }
    }

    void OnTriggerExit (Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        Ui.SetActive(false);
        controll.ToggleCursorLock(true);
      }
    }

    public void Spawn() //spawn car
    {
      controll.ToggleCursorLock(true);
      Ui.SetActive(false);
      PhotonNetwork.Instantiate(carPrefab.name, spawnPos.transform.position, spawnPos.rotation);
    }

    void FixedUpdate()
    {
      if (controll == null)
         controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
      
      if (Ui == null)
        {
           Ui = GameObject.FindGameObjectWithTag("CarShopUi");
           Ui.SetActive(false);
        }
    }
  }
