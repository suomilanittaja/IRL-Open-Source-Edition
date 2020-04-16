using Photon.Pun;
using Photon.Realtime;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject quickStartButton; // button used joining and creating server
    [SerializeField]
    private GameObject quickCancelButton; // button to cancel joining
    [SerializeField]
    private int RoomSize; // number of max players in one Room

    public override void OnConnectedToMaster()
    {
      PhotonNetwork.AutomaticallySyncScene = true;
      quickStartButton.SetActive(true);
    }

    public void QuickStart()
    {
      quickStartButton.SetActive(false);
      quickCancelButton.SetActive(true);
      PhotonNetwork.JoinRandomRoom();
      Debug.Log("Quick Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
      Debug.Log("Failed to join room");
      CreateRoom();
    }

    void CreateRoom()
    {
      Debug.Log("Now creating room");
      int randomRoomNumber = Random.Range(0, 10000);
      RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
      PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
      Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
      Debug.Log("Failed to create room Trying again");
      CreateRoom();
    }

    public void QuickCancel()
    {
      quickCancelButton.SetActive(false);
      quickStartButton.SetActive(true);
      PhotonNetwork.LeaveRoom();
    }
}
