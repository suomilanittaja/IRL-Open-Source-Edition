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
   private PlayerController controll;


   void Start()
   {
     Ui.gameObject.SetActive(false);
   }

   void OnTriggerEnter (Collider other)
   {
     if (other.gameObject.CompareTag("Player"))
     {
       controll.disableMouselook = true;
       Ui.gameObject.SetActive(true);
       Cursor.lockState = CursorLockMode.None; //unlock cursor
       Cursor.visible = true; //make mouse visible
     }
   }

   void OnTriggerExit (Collider other)
   {
     Ui.gameObject.SetActive(false);
     controll.disableMouselook = false;
     Cursor.lockState = CursorLockMode.Locked; //unlock cursor
     Cursor.visible = false; //make mouse visible
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
       controll.disableMouselook = false;
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
       controll.disableMouselook = false;
     }
   }

   void Update()
   {
     if (controll == null)
        controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
   }
}
