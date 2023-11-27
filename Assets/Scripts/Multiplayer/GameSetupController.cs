using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviourPun
{

    public void CreatePlayer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Creating player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }
}
