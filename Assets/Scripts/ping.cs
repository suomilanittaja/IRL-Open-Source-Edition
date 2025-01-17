using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ping : MonoBehaviourPun
{
    public float Deley;

    // Update is called once per frame
    void Update()
    {
        PhotonNetwork.GetPing();            //get ping from the server
        Deley = PhotonNetwork.GetPing();    
    }
}
