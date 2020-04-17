using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;

public class Eat : MonoBehaviourPunCallbacks
{
   [Header("Settings")]
   [SerializeField] private string selectableTag = "Selectable";

   [Header("GameObjects")]
   public GameObject Text;
   private GameObject Food;

   [Header("Scripts")]
   public Stats stats;
   public Raycast rayScript;
   private PhotonView PhotonView;

   private Transform _selection;


   private void Update()
   {
     Food = rayScript.rayHitted;
     PhotonView = (PhotonView)Food.GetComponent(typeof(PhotonView));
     if (rayScript.rayHitted.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
     {
       TransferOwnership();
       stats.Eat();
       print("key was pressed");
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
           PhotonNetwork.Destroy(Food);
       }
       else
        PhotonNetwork.Destroy(Food);
   }
}
