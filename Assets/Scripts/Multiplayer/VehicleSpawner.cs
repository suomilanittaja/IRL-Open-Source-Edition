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
        controll.UnLock();
      }
    }

    void OnTriggerExit (Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        Ui.SetActive(false);
        controll.Lock();
      }
    }

    public void Spawn()
    {
      controll.Lock();
      Ui.SetActive(false);
      PhotonNetwork.Instantiate(carPrefab.name, spawnPos.transform.position, spawnPos.rotation);
    }

    void Update()
    {
      if (controll == null)
         controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
  }
