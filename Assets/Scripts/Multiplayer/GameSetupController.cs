using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviourPun
{
    public bool PlayerSpawned = false;

    public void CreatePlayer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Creating player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPlayer"), Vector3.zero, Quaternion.identity);
        PlayerSpawned = true;
    }
}
