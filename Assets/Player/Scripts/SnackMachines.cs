using UnityEngine;
using System.Collections;
using Photon.Pun;

public class SnackMachines : MonoBehaviourPunCallbacks
{

   public Money money;
   public GameObject Text;
   public GameObject lemonadePrefab;
   public Transform Spawn;


   void Start()
   {
     Text.gameObject.SetActive(false);
   }

   void OnTriggerEnter (Collider other)
   {
     Text.gameObject.SetActive(true);
   }

   void OnTriggerStay (Collider other)
   {
     if (money.money >= 20 && Input.GetKeyDown(KeyCode.F))
     {
       money.money -= 20;
       //Instantiate(Drink, new Vector3(354, 38, 581), Quaternion.identity);
       PhotonNetwork.Instantiate(lemonadePrefab.name, Spawn.transform.position, Spawn.rotation);
     }
   }

   void OnTriggerExit (Collider other)
   {
     Text.gameObject.SetActive(false);
   }

}
