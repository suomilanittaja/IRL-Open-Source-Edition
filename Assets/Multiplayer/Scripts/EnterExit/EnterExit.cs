namespace IRL.EnterExit
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Photon.Pun;
    using Photon.Realtime;
    using IRL.EnterExit.SeatPlayer;
    using IRL.EnterExit.Seats;
    [RequireComponent(typeof(PhotonView))]
    public class EnterExit : MonoBehaviourPunCallbacks
    {
        #region PrivateVariables
        private bool RunSlowDown;
        private Collider CurrentCollider;
        private PhotonView LocalPlayer;
        private EnterExitPlayer enterExitPlayer;
        #endregion
        #region PublicVariables
        public KeyCode EnterKey = KeyCode.E;
        private KeyCode ExitKey = KeyCode.E;
        public bool IsLocal;
        public bool AutoSlowStop = false;
        public bool CanExit;
        public bool IsEntering;
        public int LocalPlayerSeatIndex;
        public float SlowDownRatio = 1;
        public float TimeBetweenStates = 3;
        public PhotonView PhotonView;
        public EnterExitAbstract EnterExitModule;
        [SerializeField]
        public List<EnterExitSeat> EnterExitSeats = new List<EnterExitSeat>();
        #endregion
        #region  UnityCalls
        /// <summary>
        /// On trigger stay
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<EnterExitPlayer>())
            {
                if (other.GetComponent<EnterExitPlayer>().PhotonView.IsMine)
                {
                    if (CurrentCollider != other)
                    {
                        CurrentCollider = other;
                        LocalPlayer = other.GetComponent<EnterExitPlayer>().PhotonView;
                        enterExitPlayer = other.GetComponent<EnterExitPlayer>();
                    }
                }
            }
        }
        /// <summary>
        /// On Trigger Exit
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (CurrentCollider == other)
            {
                CurrentCollider = null;
                LocalPlayer = null;
                enterExitPlayer = null;
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            EnterExitState();
        }
        /// <summary>
        /// Fixed update
        /// </summary>
        public void FixedUpdate()
        {
            VehicleSlowDown();
        }
        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            Prepare();
        }
        #endregion
        #region Preperation
        /// <summary>
        /// Preperation for enter exit
        /// </summary>
        public void Prepare()
        {
            for (int SeatIndex = 0; SeatIndex < EnterExitSeats.Count; SeatIndex++)
            {
                for (int MonoIndex = 0; MonoIndex < EnterExitSeats[SeatIndex].SeatMonos.Count; MonoIndex++)
                {
                    EnterExitSeats[MonoIndex].SeatMonos[MonoIndex].enabled = false;
                }
                if (EnterExitSeats[SeatIndex].Chair)
                {
                    EnterExitSeats[SeatIndex].Chair.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("No seat found");
                }
            }
            if (!PhotonView)
            {
                if (GetComponent<PhotonView>())
                {
                    PhotonView = GetComponent<PhotonView>();
                    OwnerShipTransferPreWarm();
                }
            }
            if (PhotonView)
            {
                if (EnterExitModule)
                {
                    EnterExitModule.WarmUp(this);
                }
                OwnerShipTransferPreWarm();
            }
        }
        //Setup logic for Ownership transfering
        public void OwnerShipTransferPreWarm()
        {
            if (PhotonView.OwnershipTransfer != OwnershipOption.Takeover)
            {
                PhotonView.OwnershipTransfer = OwnershipOption.Takeover;
                Debug.Log("Had to change Ownership to takeover, please change ownership on this vehicle to takeover");
            }
        }
        /// <summary>
        /// Enter Vehicle
        /// </summary>
        public void EnterVehicle(int Seat)
        {
            if (LocalPlayer)
            {
                enterExitPlayer.InVehicle = true;
                AssignPlayerInSeat(Seat, LocalPlayer.ViewID, LocalPlayer.Owner);
            }
            else
            {
                Debug.Log("Player does not have a photon view assigned in the EnterExitPlayer");
            }
        }
        /// <summary>
        /// Vehicle slow down
        /// </summary>
        public void VehicleSlowDown()
        {
            if (AutoSlowStop && RunSlowDown)
            {
                if (GetComponent<Rigidbody>())
                {
                    Rigidbody Rigidbody = GetComponent<Rigidbody>();
                    if (Mathf.Approximately(Rigidbody.velocity.z, 0))
                    {
                        Rigidbody.isKinematic = true;
                        RunSlowDown = false;
                    }
                    else
                    {
                        float delta = Time.fixedDeltaTime * SlowDownRatio;
                        Rigidbody.drag += Rigidbody.drag * delta;
                    }
                }
            }
        }
        /// <summary>
        /// Assign player in seat based on Seat Index
        /// </summary>
        /// <param name="SeatIndex"></param>
        /// <param name="ViewID"></param>
        public void AssignPlayerInSeat(int SeatIndex, int ViewID, Player Player)
        {
            if (EnterExitSeats.Count >= SeatIndex)
            {
                if (EnterExitSeats[SeatIndex].IsOccupied == false)
                {
                    EnterExitSeats[SeatIndex].IsOccupied = true;
                    EnterExitSeats[SeatIndex].Player = Player;
                    EnterExitSeats[SeatIndex].ViewID = ViewID;
                    if (CurrentCollider.transform.parent)
                    {
                        EnterExitSeats[SeatIndex].Playertransform = CurrentCollider.transform.parent;
                    }
                    else
                    {
                        EnterExitSeats[SeatIndex].Playertransform = CurrentCollider.transform;
                    }
                    LocalPlayerSeatIndex = SeatIndex;
                    for (int MonoIndex = 0; MonoIndex < EnterExitSeats[SeatIndex].SeatMonos.Count; MonoIndex++)
                    {
                        EnterExitSeats[MonoIndex].SeatMonos[MonoIndex].enabled = true;
                    }
                    EnterExitSeats[SeatIndex].Chair.gameObject.SetActive(true);
                    if (EnterExitSeats[SeatIndex].TakeOwnership)
                    {
                        TransferOwnership();
                        if (EnterExitModule)
                        {
                            EnterExitModule.OnOwnerShipTransfer(SeatIndex);
                        }
                        if (AutoSlowStop)
                        {
                            if (GetComponent<Rigidbody>())
                            {
                                GetComponent<Rigidbody>().isKinematic = false;
                            }
                        }
                    }
                    if (EnterExitSeats[SeatIndex].ExitLocation)
                    {
                        EnterExitSeats[SeatIndex].Playertransform.position = EnterExitSeats[SeatIndex].ExitLocation.position;
                        EnterExitSeats[SeatIndex].Playertransform.rotation = EnterExitSeats[SeatIndex].ExitLocation.rotation;
                    }
                    else
                    {
                        Debug.Log("Missing exit position on Seat " + SeatIndex);
                    }
                    PhotonView.RPC("DisableInteractor", RpcTarget.AllBuffered, EnterExitSeats[SeatIndex].ViewID, false);
                    if (EnterExitModule)
                    {
                        EnterExitModule.OnPlayerEnter(SeatIndex);
                    }
                    IsLocal = true;
                    CurrentCollider = null;
                    LocalPlayer = null;
                    enterExitPlayer.InVehicle = false;
                    enterExitPlayer = null;
                }
                else
                {
                    Debug.Log("Car seat is occupied");
                }
            }
            else
            {
                Debug.Log("Seat does not exist, make sure list is correct");
            }
            GlobalSyncSeatState();
        }
        /// <summary>
        /// Remove player in seat based on Seat Index
        /// </summary>
        /// <param name="SeatIndex"></param>
        public void RemovePlayerInSeat(int SeatIndex)
        {
            if (EnterExitSeats.Count >= SeatIndex)
            {
                if (EnterExitSeats[SeatIndex].IsOccupied == true)
                {
                    if (EnterExitModule)
                    {
                        EnterExitModule.OnPlayerExit(SeatIndex);
                    }
                    EnterExitSeats[SeatIndex].IsOccupied = false;
                    if (EnterExitSeats[SeatIndex].Playertransform)
                    {
                        if (EnterExitSeats[SeatIndex].ExitLocation)
                        {
                            EnterExitSeats[SeatIndex].Playertransform.position = EnterExitSeats[SeatIndex].ExitLocation.position;
                            EnterExitSeats[SeatIndex].Playertransform.rotation = EnterExitSeats[SeatIndex].ExitLocation.rotation;
                        }
                        else
                        {
                            Debug.Log("Missing exit position on Seat " + SeatIndex);
                        }
                    }
                    else
                    {
                        Debug.Log("Player unexpectedly does not exist");
                    }
                    PhotonView.RPC("DisableInteractor", RpcTarget.AllBuffered, EnterExitSeats[SeatIndex].ViewID, true);
                    if (EnterExitSeats[SeatIndex].Playertransform)
                    {
                        if (EnterExitSeats[SeatIndex].ExitLocation)
                        {
                            EnterExitSeats[SeatIndex].Playertransform.position = EnterExitSeats[SeatIndex].ExitLocation.position;
                            EnterExitSeats[SeatIndex].Playertransform.rotation = EnterExitSeats[SeatIndex].ExitLocation.rotation;
                        }
                        else
                        {
                            Debug.Log("Missing exit position on Seat " + SeatIndex);
                        }
                    }
                    else
                    {
                        Debug.Log("Player unexpectedly does not exist");
                    }
                    EnterExitSeats[SeatIndex].Player = null;
                    EnterExitSeats[SeatIndex].Playertransform = null;
                    EnterExitSeats[SeatIndex].ViewID = 0;
                    IsLocal = false;
                    for (int MonoIndex = 0; MonoIndex < EnterExitSeats[SeatIndex].SeatMonos.Count; MonoIndex++)
                    {
                        EnterExitSeats[MonoIndex].SeatMonos[MonoIndex].enabled = false;
                    }
                    EnterExitSeats[SeatIndex].Chair.gameObject.SetActive(false);
                    if (EnterExitSeats[SeatIndex].TakeOwnership)
                    {
                        RunSlowDown = true;
                    }
                }
                else
                {
                    Debug.Log("Car seat was not occupied");
                }

            }
            GlobalSyncSeatState();
        }
        #endregion EnterExit System
        #region TimedEvents
        /// <summary>
        /// State reset system
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        IEnumerator StateReset(float Time)
        {
            yield return new WaitForSeconds(Time);
            CanExit = true;
        }
        #endregion
        #region Photon Callbacks
        /// <summary>
        /// On player left room deal with seat & player
        /// </summary>
        /// <param name="other"></param>
        public override void OnPlayerLeftRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int SeatIndex = 0; SeatIndex < EnterExitSeats.Count; SeatIndex++)
                {
                    if (EnterExitSeats[SeatIndex].IsOccupied)
                    {
                        if (EnterExitSeats[SeatIndex].Player == other)
                        {
                            EnterExitSeats[SeatIndex].Player = null;
                            EnterExitSeats[SeatIndex].Playertransform = null;
                            EnterExitSeats[SeatIndex].ViewID = 0;
                            EnterExitSeats[SeatIndex].IsOccupied = false;
                        }
                    }
                }
                PlayerSyncSeatState(other);
            }
        }
        /// <summary>
        /// on player join room, update that player
        /// </summary>
        /// <param name="other"></param>
        public override void OnPlayerEnteredRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PlayerSyncSeatState(other);
            }
        }
        #endregion
        #region NetworkRequiredSync
        /// <summary>
        /// transfers ownership
        /// </summary>
        public void TransferOwnership()
        {
            if (PhotonView.Owner.IsLocal == false)
            {
                Debug.Log("Requesting Ownership");
                PhotonView.RequestOwnership();
            }
        }
        /// <summary>
        /// Enter exit state
        /// </summary>
        public void EnterExitState()
        {
            if (!IsLocal)
            {
                if (LocalPlayer)
                {
                    if (LocalPlayer.IsMine)
                    {
                        if (Input.GetKeyDown(EnterKey))
                        {
                            int Seat = GetAvaliableSeat();
                            if (Seat != -1)
                            {
                                EnterVehicle(Seat);
                            }
                            else
                            {
                                Debug.Log("No seats Avaliable");
                            }
                            CanExit = false;
                            if (IsLocal)
                            {
                                StartCoroutine(StateReset(TimeBetweenStates));
                            }
                        }
                    }
                }
            }
            else
            {
                if (CanExit)
                {
                    if (EnterExitSeats[LocalPlayerSeatIndex].Player.IsLocal)
                    {
                        if (Input.GetKeyDown(ExitKey))
                        {
                            RemovePlayerInSeat(LocalPlayerSeatIndex);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Get first avaliable seat
        /// if no seat Avaliable -1 will be returned
        /// </summary>
        /// <returns></returns>
        public int GetAvaliableSeat()
        {
            for (int SeatIndex = 0; SeatIndex < EnterExitSeats.Count; SeatIndex++)
            {
                if (!EnterExitSeats[SeatIndex].IsOccupied)
                {
                    return SeatIndex;
                }
            }
            return -1;
        }
        /// <summary>
        /// Disables the photon player on all clients
        /// </summary>
        /// <param name="ViewID"></param>
        /// <param name="State"></param>
        [PunRPC]
        void DisableInteractor(int ViewID, bool State)
        {
            PhotonView FoundView = PhotonView.Find(ViewID);
            if (FoundView)
            {
                FoundView.transform.gameObject.SetActive(State);
            }
            else
            {
                Debug.Log("photon view could not be parsed");
            }
        }
        [PunRPC]
        void SyncSeatActiveState(int ViewID, int SeatIndex)
        {
            EnterExitSeats[SeatIndex].IsOccupied = true;
            EnterExitSeats[SeatIndex].ViewID = ViewID;
        }
        [PunRPC]
        void SyncSeatDisableState(int SeatIndex)
        {
            EnterExitSeats[SeatIndex].IsOccupied = false;
            EnterExitSeats[SeatIndex].ViewID = 0;
        }
        #endregion
        #region SyncStates
        /// <summary>
        /// send all seat syncs to other players (takes Player)
        /// </summary>
        public void PlayerSyncSeatState(Player Player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int SeatIndex = 0; SeatIndex < EnterExitSeats.Count; SeatIndex++)
                {
                    if (EnterExitSeats[SeatIndex].IsOccupied)
                    {
                        PhotonView.RPC("SyncSeatActiveState", Player, EnterExitSeats[SeatIndex].ViewID, SeatIndex);
                    }
                    else
                    {
                        PhotonView.RPC("SyncSeatDisableState", Player, SeatIndex);
                    }
                }
            }
        }
        /// <summary>
        /// send all seat syncs to other players (global other sync)
        /// </summary>
        public void GlobalSyncSeatState()
        {
            for (int SeatIndex = 0; SeatIndex < EnterExitSeats.Count; SeatIndex++)
            {
                if (EnterExitSeats[SeatIndex].IsOccupied)
                {
                    PhotonView.RPC("SyncSeatActiveState", RpcTarget.OthersBuffered, EnterExitSeats[SeatIndex].ViewID, SeatIndex);
                }
                else
                {
                    PhotonView.RPC("SyncSeatDisableState", RpcTarget.OthersBuffered, SeatIndex);
                }
            }
        }
        #endregion
    }
}
