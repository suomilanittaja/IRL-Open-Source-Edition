using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;

public class Drink : MonoBehaviourPunCallbacks
{
   [Header("Settings")]
   [SerializeField] private string selectableTag = "Selectable";
   [SerializeField] private string selectableTag2 = "Selectable";

   [Header("GameObjects")]
   public GameObject Text;
   private GameObject drink;

   [Header("Scripts")]
   public Stats stats;
   public Raycast rayScript;
   private PhotonView PhotonView;

   private Transform _selection;


   private void Update()
   {
     drink = rayScript.rayHitted;
     PhotonView = (PhotonView)drink.GetComponent(typeof(PhotonView));

     if (rayScript.rayHitted.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
     {
       TransferOwnership();
       stats.drunk();
     }

     if (rayScript.rayHitted.CompareTag(selectableTag2) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
     {
       TransferOwnership();
       stats.Drink();
     }

     if (rayScript.rayHitted.CompareTag(selectableTag) && rayScript.hitDis <= 5)
     {
       Text.gameObject.SetActive(true);
     }
     else
      Text.gameObject.SetActive(false);

   }

   public void TransferOwnership()
   {
       if (PhotonView.Owner.IsLocal == false)
       {
           Debug.Log("Requesting Ownership");
           PhotonView.RequestOwnership();
           PhotonNetwork.Destroy(drink);
       }
       else
        PhotonNetwork.Destroy(drink);
   }
}
