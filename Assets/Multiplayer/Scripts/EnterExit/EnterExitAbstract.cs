using IRL.EnterExit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Abstract used for building modules and call backs
/// </summary>
abstract public class EnterExitAbstract : MonoBehaviour
{
    /// <summary>
    /// On player Enter
    /// </summary>
    /// <param name="SeatIndex"></param>
    [SerializeField]
    abstract public void OnPlayerEnter(int SeatIndex);
    /// <summary>
    /// On player Exit
    /// </summary>
    /// <param name="SeatIndex"></param>
    [SerializeField]
    abstract public void OnPlayerExit(int SeatIndex);
    /// <summary>
    /// On OwnerShip Transfer
    /// </summary>
    /// <param name="SeatIndex"></param>
    [SerializeField]
    abstract public void OnOwnerShipTransfer(int SeatIndex);
    /// <summary>
    /// Reference
    /// </summary>
    /// <param name="SeatIndex"></param>
    [SerializeField]
    abstract public void WarmUp(EnterExit EnterExit);
}
