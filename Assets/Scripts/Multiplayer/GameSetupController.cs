using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      CreatePlayer();
    }

    private void CreatePlayer()
    {
      Debug.Log("Creating player");
      PhotonNetwork.Instantiate(Path.Combine("PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }
}
