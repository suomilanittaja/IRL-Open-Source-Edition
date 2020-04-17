using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Shop : MonoBehaviourPunCallbacks
{

   public Money money;
   public GameObject Ui;
   public GameObject lemonadePrefab;
   public GameObject macaroniCasserolePrefab;
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
       Ui.gameObject.SetActive(true);
       controll.UnLock();
     }
   }

   void OnTriggerExit (Collider other)
   {
     Ui.gameObject.SetActive(false);
     controll.Lock();
   }

   public void lemonade()
   {
     if (money.money >= 10)
     {
       money.money -= 10;
       PhotonNetwork.Instantiate(lemonadePrefab.name, Spawn.transform.position, Spawn.rotation);
       Ui.gameObject.SetActive(false);
       controll.Lock();
     }
   }

   public void macaroniCasserole()
   {
     if (money.money >= 15)
     {
       money.money -= 15;
       PhotonNetwork.Instantiate(macaroniCasserolePrefab.name, Spawn.transform.position, Spawn.rotation);
       Ui.gameObject.SetActive(false);
       controll.Lock();
     }
   }

   public void gun()
   {
     if (money.money >= 50)
     {
       money.money -= 50;
       PhotonNetwork.Instantiate(gunPrefab.name, Spawn.transform.position, Spawn.rotation);
       Ui.gameObject.SetActive(false);
       controll.Lock();
     }
   }

   void Update()
   {
     if (controll == null)
        controll = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
   }
}
