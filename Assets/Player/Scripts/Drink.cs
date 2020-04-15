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
   public PhotonView PhotonView;

   private void Update()
   {
     Beer = rayScript.rayHitted;
     PhotonView = (PhotonView)Beer.GetComponent(typeof(PhotonView));
     if (rayScript.rayHitted.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.F) && rayScript.hitDis <= 5)
     {
       TransferOwnership();
       stats.Drink();
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
           PhotonNetwork.Destroy(Beer);
       }
       else
        PhotonNetwork.Destroy(Beer);
   }
}
