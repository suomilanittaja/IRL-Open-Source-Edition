using UnityEngine;
using System.Collections;

public class SnackMachines : MonoBehaviour
{

   //public Vector3 Buyed;
   public Money money;
   public GameObject Text;
   public GameObject Drink;

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
       Instantiate(Drink, new Vector3(354, 38, 581), Quaternion.identity);
     }
   }

   void OnTriggerExit (Collider other)
   {
     Text.gameObject.SetActive(false);
   }

}
