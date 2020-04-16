using Photon.Pun;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex;

    public override void OnEnable()
    {
      PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
      PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
      Debug.Log("joined room");
      StartGame();
    }

    private void StartGame()
    {
      if (PhotonNetwork.IsMasterClient)
      {
        Debug.Log("starting game");
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
      }
    }
}
