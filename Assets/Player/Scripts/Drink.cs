using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;

public class Drink : MonoBehaviourPunCallbacks
{
   [SerializeField] private string selectableTag = "Selectable";

   private Transform _selection;
   public Stats stats;
   public GameObject Text;
   private GameObject Beer;
   public Raycast rayScript;

   private void Update()
   {
     Beer = rayScript.rayHitted;
     if (rayScript.rayHitted.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
     {
     stats.Drink();
     print("key was pressed");
     PhotonNetwork.Destroy(Beer);
     }

     if (rayScript.rayHitted.CompareTag(selectableTag) && rayScript.hitDis <= 5)
     {
      Text.gameObject.SetActive(true);
     }
     else
      Text.gameObject.SetActive(false);
   }
}
