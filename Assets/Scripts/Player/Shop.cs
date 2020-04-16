using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Shop : MonoBehaviourPunCallbacks
{

   public Money money;
   public GameObject Ui;
   public GameObject lemonadePrefab;
   public GameObject gunPrefab;
   public Transform Spawn;


   void Start()
   {
     Ui.gameObject.SetActive(false);
   }

   void OnTriggerEnter (Collider other)
   {
     if (other.gameObject.CompareTag("Player"))
     {
       Ui.gameObject.SetActive(true);
       Cursor.lockState = CursorLockMode.None; //unlock cursor
       Cursor.visible = true; //make mouse visible
     }
   }

   void OnTriggerExit (Collider other)
   {
     Ui.gameObject.SetActive(false);
   }

   public void lemonade()
   {
     if (money.money >= 20)
     {
       money.money -= 20;
       PhotonNetwork.Instantiate(lemonadePrefab.name, Spawn.transform.position, Spawn.rotation);
       Ui.gameObject.SetActive(false);
       Cursor.lockState = CursorLockMode.Locked; //lock cursor
       Cursor.visible = false; //disable visible mouse
     }
   }

   public void gun()
   {
     if (money.money >= 50)
     {
       money.money -= 50;
       PhotonNetwork.Instantiate(gunPrefab.name, Spawn.transform.position, Spawn.rotation);
       Ui.gameObject.SetActive(false);
       Cursor.lockState = CursorLockMode.Locked; //lock cursor
       Cursor.visible = false; //disable visible mouse
     }
   }
}
