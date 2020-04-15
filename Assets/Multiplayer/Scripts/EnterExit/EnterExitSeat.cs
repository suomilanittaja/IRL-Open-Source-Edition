namespace IRL.EnterExit.Seats
{
    using Photon.Realtime;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Serializable]
    public class EnterExitSeat
    {
        #region SeatData
        [SerializeField]
        public Transform Chair;
        [SerializeField]
        public Transform ExitLocation;
        [SerializeField]
        public Transform Playertransform;
        [SerializeField]
        public bool IsOccupied;
        [SerializeField]
        public bool TakeOwnership;
        [SerializeField]
        public int ViewID;
        [SerializeField]
        public Player Player;
        [SerializeField]
        public List<MonoBehaviour> SeatMonos = new List<MonoBehaviour>();
        [SerializeField]
        public bool m_editorVisable;
        [SerializeField]
        public string Name;
        #endregion
    }
}
