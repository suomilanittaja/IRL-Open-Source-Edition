using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviourPunCallbacks
{
    public Transform spawnPos;
    public GameObject Ui;
    public GameObject carPrefab;
    private PlayerController controll;

    void OnTriggerEnter (Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        Ui.SetActive(true);
        Cursor.lockState = CursorLockMode.None; //unlock cursor
        Cursor.visible = true; //make mouse visible
        controll.disableMouselook = true;
      }
    }

    void OnTriggerExit (Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        Ui.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; //lock cursor
        Cursor.visible = false; //disable visible mouse
        controll.disableMouselook = false;
      }
    }

    public void Spawn()
    {
      Cursor.lockState = CursorLockMode.Locked; //lock cursor
      Cursor.visible = false; //disable visible mouse
      Ui.SetActive(false);
      PhotonNetwork.Instantiate(carPrefab.name, spawnPos.transform.position, spawnPos.rotation);
      controll.disableMouselook = false;
    }

    void Update()
    {
      if (controll == null)
         controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
  }
