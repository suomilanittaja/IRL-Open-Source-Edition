using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ProximityVoiceTrigger : VoiceComponent
{
    private List<byte> groupsToAdd = new List<byte>();
    private List<byte> groupsToRemove = new List<byte>();

    [SerializeField] // TODO: make it readonly
    private byte[] subscribedGroups;

    private PhotonVoiceView photonVoiceView;
    private PhotonView photonView;

    public byte TargetInterestGroup
    {
        get
        {
            if (photonView != null)
            {
                return (byte)photonView.OwnerActorNr;
            }
            return 0;
        }
    }

    protected override void Awake()
    {
        photonVoiceView = GetComponentInParent<PhotonVoiceView>();
        photonView = GetComponentInParent<PhotonView>();
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void ToggleTransmission()
    {
        if (photonVoiceView.RecorderInUse != null)
        {
            byte group = TargetInterestGroup;
            if (photonVoiceView.RecorderInUse.InterestGroup != group)
            {
                if (this.Logger.IsInfoEnabled)
                {
                    this.Logger.LogInfo("Setting RecorderInUse's InterestGroup to {0}", group);
                }
                photonVoiceView.RecorderInUse.InterestGroup = group;
            }
            bool transmitEnabled = subscribedGroups != null && subscribedGroups.Length > 0;
            if (photonVoiceView.RecorderInUse.TransmitEnabled != transmitEnabled)
            {
                if (this.Logger.IsInfoEnabled)
                {
                    this.Logger.LogInfo("Setting RecorderInUse's TransmitEnabled to {0}", transmitEnabled);
                }
                photonVoiceView.RecorderInUse.TransmitEnabled = transmitEnabled;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ProximityVoiceTrigger trigger = other.GetComponent<ProximityVoiceTrigger>();
        if (trigger != null)
        {
            byte group = trigger.TargetInterestGroup;
            if (this.Logger.IsDebugEnabled)
            {
                this.Logger.LogDebug("OnTriggerEnter {0}", group);
            }
            if (group == this.TargetInterestGroup)
            {
                return;
            }
            if (group == 0)
            {
                return;
            }
            if (!groupsToAdd.Contains(group))
            {
                groupsToAdd.Add(group);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ProximityVoiceTrigger trigger = other.GetComponent<ProximityVoiceTrigger>();
        if (trigger != null)
        {
            byte group = trigger.TargetInterestGroup;
            if (this.Logger.IsDebugEnabled)
            {
                this.Logger.LogDebug("OnTriggerExit {0}", group);
            }
            if (group == this.TargetInterestGroup)
            {
                return;
            }
            if (group == 0)
            {
                return;
            }
            if (groupsToAdd.Contains(group))
            {
                groupsToAdd.Remove(group);
            }
            if (!groupsToRemove.Contains(group))
            {
                groupsToRemove.Add(group);
            }
        }
    }

    private void Update()
    {
        if (!PhotonVoiceNetwork.Instance.Client.InRoom)
        {
            subscribedGroups = null;
        }
        else
        {
            if (groupsToAdd.Count > 0 || groupsToRemove.Count > 0)
            {
                byte[] toAdd = null;
                byte[] toRemove = null;
                if (groupsToAdd.Count > 0)
                {
                    toAdd = groupsToAdd.ToArray();
                }
                if (groupsToRemove.Count > 0)
                {
                    toRemove = groupsToRemove.ToArray();
                }
                if (this.Logger.IsInfoEnabled)
                {
                    this.Logger.LogInfo("Trying to change groups, to_be_removed#:{0} to_be_added#={1}", groupsToRemove.Count, groupsToAdd.Count);
                }
                if (PhotonVoiceNetwork.Instance.Client.OpChangeGroups(toRemove, toAdd))
                {
                    if (subscribedGroups != null)
                    {
                        List<byte> list = new List<byte>();
                        for (int i = 0; i < subscribedGroups.Length; i++)
                        {
                            list.Add(subscribedGroups[i]);
                        }
                        for (int i = 0; i < groupsToRemove.Count; i++)
                        {
                            if (list.Contains(groupsToRemove[i]))
                            {
                                list.Remove(groupsToRemove[i]);
                            }
                        }
                        for (int i = 0; i < groupsToAdd.Count; i++)
                        {
                            if (!list.Contains(groupsToAdd[i]))
                            {
                                list.Add(groupsToAdd[i]);
                            }
                        }
                        subscribedGroups = list.ToArray();
                    }
                    else
                    {
                        subscribedGroups = toAdd;
                    }
                    groupsToAdd.Clear();
                    groupsToRemove.Clear();
                }
                else if (this.Logger.IsErrorEnabled)
                {
                    this.Logger.LogError("Error changing groups");
                }
            }
            ToggleTransmission();
        }
    }
}
