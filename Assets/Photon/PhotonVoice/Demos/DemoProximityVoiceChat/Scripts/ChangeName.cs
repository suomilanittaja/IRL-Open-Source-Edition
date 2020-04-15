using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ChangeName : MonoBehaviour
{
    private void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        name = string.Format("ActorNumber {0}", photonView.OwnerActorNr);
    }
}