namespace IRL.EnterExit.SeatPlayer
{
    using Photon.Pun;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class EnterExitPlayer : MonoBehaviour
    {
        #region publicVariables
        public PhotonView PhotonView;
        public GameObject Camera;
        public List<MonoBehaviour> PlayerMonos = new List<MonoBehaviour>();
        public bool InVehicle;
        #endregion
        #region NetworkControl
        public void Start()
        {
            if (this.GetComponent<PhotonView>().IsMine)
            {
                if (GetComponent<Rigidbody>())
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                }
                for (int i = 0; i < PlayerMonos.Count; i++)
                {
                    PlayerMonos[i].enabled = true;
                }
                Camera.SetActive(true);
            }
            else
            {
                if (GetComponent<Rigidbody>())
                {
                    GetComponent<Rigidbody>().isKinematic = true;
                }
                for (int i = 0; i < PlayerMonos.Count; i++)
                {
                    PlayerMonos[i].enabled = false;
                }
                Camera.SetActive(false);
            }
        }
        #endregion
    }
}
