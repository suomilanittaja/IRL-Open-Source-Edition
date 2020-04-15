// ----------------------------------------------------------------------------
// <copyright file="VoiceDemoUI.cs" company="Exit Games GmbH">
// Photon Voice Demo for PUN - Copyright (C) Exit Games GmbH
// </copyright>
// <summary>
// UI manager class for the PUN Voice Demo
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------

using System;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

#pragma warning disable 0649 // Field is never assigned to, and will always have its default value

namespace ExitGames.Demos.DemoPunVoice
{
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.UI;
    using Client.Photon;

#if !UNITY_EDITOR && UNITY_PS4
    using Sony.NP;
#endif

    public class VoiceDemoUI : MonoBehaviour
    {
        [SerializeField]
        private Text punState;
        [SerializeField]
        private Text voiceState;
        
        private PhotonVoiceNetwork punVoiceNetwork;

        private Canvas canvas;

        [SerializeField]
        private Button punSwitch;
        private Text punSwitchText;
        [SerializeField]
        private Button voiceSwitch;
        private Text voiceSwitchText;
        [SerializeField]
        private Button calibrateButton;
        private Text calibrateText;

        [SerializeField]
        private Text voiceDebugText;

        public Recorder recorder;

        [SerializeField]
        private GameObject inGameSettings;

        [SerializeField]
        private GameObject globalSettings;

        [SerializeField]
        private Text devicesInfoText;

        private GameObject debugGO;

        private bool debugMode;

        private float volumeBeforeMute;

        private DebugLevel previousDebugLevel;

        public bool DebugMode
        {
            get
            {
                return debugMode;
            }
            set
            {
                debugMode = value;
                debugGO.SetActive(debugMode);
                voiceDebugText.text = String.Empty;
                if (debugMode)
                {
                    previousDebugLevel = punVoiceNetwork.Client.LoadBalancingPeer.DebugOut;
                    punVoiceNetwork.Client.LoadBalancingPeer.DebugOut = DebugLevel.ALL;
                }
                else
                {
                    punVoiceNetwork.Client.LoadBalancingPeer.DebugOut = previousDebugLevel;
                }
                if (DebugToggled != null)
                {
                    DebugToggled(debugMode);
                }
            }
        }

        public delegate void OnDebugToggle(bool debugMode);

        public static event OnDebugToggle DebugToggled;

        [SerializeField]
        private int calibrationMilliSeconds = 2000;

        private void Awake()
        {
            punVoiceNetwork = PhotonVoiceNetwork.Instance;
        }

        private void OnEnable()
        {
            ChangePOV.CameraChanged += OnCameraChanged;
            BetterToggle.ToggleValueChanged += BetterToggle_ToggleValueChanged;
            CharacterInstantiation.CharacterInstantiated += CharacterInstantiation_CharacterInstantiated;
            punVoiceNetwork.Client.StateChanged += VoiceClientStateChanged;
            PhotonNetwork.NetworkingClient.StateChanged += PunClientStateChanged;
        }

        private void OnDisable()
        {
            ChangePOV.CameraChanged -= OnCameraChanged;
            BetterToggle.ToggleValueChanged -= BetterToggle_ToggleValueChanged;
            CharacterInstantiation.CharacterInstantiated -= CharacterInstantiation_CharacterInstantiated;
            punVoiceNetwork.Client.StateChanged -= VoiceClientStateChanged;
            PhotonNetwork.NetworkingClient.StateChanged -= PunClientStateChanged;
        }

        private void CharacterInstantiation_CharacterInstantiated(GameObject character)
        {
            if (recorder) // probably using a global recorder
            {
                return;
            }
            PhotonVoiceView photonVoiceView = character.GetComponent<PhotonVoiceView>();
            if (photonVoiceView.IsRecorder)
            {
                recorder = photonVoiceView.RecorderInUse;
            }
        }

        private void InitToggles(Toggle[] toggles)
        {
            if (toggles == null) { return; }
            for (int i = 0; i < toggles.Length; i++)
            {
                Toggle toggle = toggles[i];
                switch (toggle.name)
                {
                    case "Mute":
                        toggle.isOn = AudioListener.volume <= 0.001f;
                        break;

                    case "VoiceDetection":
                        toggle.isOn = recorder != null && recorder.VoiceDetection;
                        break;

                    case "DebugVoice":
                        toggle.isOn = DebugMode;
                        break;

                    case "Transmit":
                        toggle.isOn = recorder != null && recorder.TransmitEnabled;
                        break;

                    case "DebugEcho":
                        toggle.isOn = recorder != null && recorder.DebugEchoMode;
                        break;

                    case "AutoConnectAndJoin":
                        toggle.isOn = punVoiceNetwork.AutoConnectAndJoin;
                        break;

                    case "AutoLeaveAndDisconnect":
                        toggle.isOn = punVoiceNetwork.AutoLeaveAndDisconnect;
                        break;
                }
            }
        }

        private void BetterToggle_ToggleValueChanged(Toggle toggle)
        {
            switch (toggle.name)
            {
                case "Mute":
                    //AudioListener.pause = toggle.isOn;
                    if (toggle.isOn)
                    {
                        volumeBeforeMute = AudioListener.volume;
                        AudioListener.volume = 0f;
                    }
                    else
                    {
                        AudioListener.volume = volumeBeforeMute;
                        volumeBeforeMute = 0f;
                    }
                    break;
                case "Transmit":
                    if (recorder)
                    {
                        recorder.TransmitEnabled = toggle.isOn;
                    }
                    break;
                case "VoiceDetection":
                    if (recorder)
                    {
                        recorder.VoiceDetection = toggle.isOn;
                    }
                    break;
                case "DebugEcho":
                    if (recorder)
                    {
                        recorder.DebugEchoMode = toggle.isOn;
                    }
                    break;
                case "DebugVoice":
                    DebugMode = toggle.isOn;
                    break;
                case "AutoConnectAndJoin":
                    punVoiceNetwork.AutoConnectAndJoin = toggle.isOn;
                    break;
                case "AutoLeaveAndDisconnect":
                    punVoiceNetwork.AutoLeaveAndDisconnect = toggle.isOn;
                    break;
            }
        }

        private void OnCameraChanged(Camera newCamera)
        {
            canvas.worldCamera = newCamera;
        }

        private void Start()
        {
            canvas = GetComponentInChildren<Canvas>();
            if (punSwitch != null)
            {
                punSwitchText = punSwitch.GetComponentInChildren<Text>();
                punSwitch.onClick.AddListener(PunSwitchOnClick);
            }
            if (voiceSwitch != null)
            {
                voiceSwitchText = voiceSwitch.GetComponentInChildren<Text>();
                voiceSwitch.onClick.AddListener(VoiceSwitchOnClick);
            }
            if (calibrateButton != null)
            {
                calibrateButton.onClick.AddListener(CalibrateButtonOnClick);
                calibrateText = calibrateButton.GetComponentInChildren<Text>();
            }
            if (punState != null)
            {
                debugGO = punState.transform.parent.gameObject;
            }
            volumeBeforeMute = AudioListener.volume;
            previousDebugLevel = punVoiceNetwork.Client.LoadBalancingPeer.DebugOut;
            if (globalSettings != null)
            {
                globalSettings.SetActive(true);
                InitToggles(globalSettings.GetComponentsInChildren<Toggle>());
            }
            if (devicesInfoText != null)
            {
                if (Microphone.devices == null || Microphone.devices.Length == 0)
                {
                    devicesInfoText.enabled = true;
                    devicesInfoText.color = Color.red;
                    devicesInfoText.text = "No microphone device detected!";
                }
                else if (Microphone.devices.Length == 1)
                {
                    devicesInfoText.text = string.Format("Mic.: {0}", Microphone.devices[0]);
                }
                else
                {
                    devicesInfoText.text = string.Format("Multi.Mic.Devices:\n0. {0} (active)\n", Microphone.devices[0]);
                    for (int i = 1; i < Microphone.devices.Length; i++)
                    {
                        devicesInfoText.text = string.Concat(devicesInfoText.text, string.Format("{0}. {1}\n", i, Microphone.devices[i]));
                    }
                }
            }

            #if !UNITY_EDITOR && UNITY_PS4
            UserProfiles.LocalUsers localUsers = new UserProfiles.LocalUsers();
            UserProfiles.GetLocalUsers(localUsers);
            int userID = localUsers.LocalUsersIds[0].UserId.Id;

            punVoiceNetwork.PS4UserID = userID;
            #endif
        }

        private void PunSwitchOnClick()
        {
            if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
            {

                PhotonNetwork.Disconnect();
            }
            else if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Disconnected ||
                PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.PeerCreated)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void VoiceSwitchOnClick()
        {
            if (punVoiceNetwork.ClientState == Photon.Realtime.ClientState.Joined)
            {
                punVoiceNetwork.Disconnect();
            }
            else if (punVoiceNetwork.ClientState == Photon.Realtime.ClientState.PeerCreated
                     || punVoiceNetwork.ClientState == Photon.Realtime.ClientState.Disconnected)
            {
                punVoiceNetwork.ConnectAndJoinRoom();
            }
        }

        private void CalibrateButtonOnClick()
        {
            if (recorder && !recorder.VoiceDetectorCalibrating)
            {
                recorder.VoiceDetectorCalibrate(calibrationMilliSeconds);
            }
        }

        private void Update()
        {
            // editor only two-ways binding for toggles
#if UNITY_EDITOR
            InitToggles(globalSettings.GetComponentsInChildren<Toggle>());
#endif
            if (recorder != null && recorder.LevelMeter != null)
            {
                voiceDebugText.text = string.Format("Amp: avg. {0:0.000000}, peak {1:0.000000}", recorder.LevelMeter.CurrentAvgAmp, recorder.LevelMeter.CurrentPeakAmp);
            }
        }

        private void PunClientStateChanged(Photon.Realtime.ClientState fromState, Photon.Realtime.ClientState toState)
        {
            punState.text = string.Format("PUN: {0}", toState);
            switch (toState)
            {
                case Photon.Realtime.ClientState.PeerCreated:
                case Photon.Realtime.ClientState.Disconnected:
                    punSwitch.interactable = true;
                    punSwitchText.text = "PUN Connect";
                    break;
                case Photon.Realtime.ClientState.Joined:
                    punSwitch.interactable = true;
                    punSwitchText.text = "PUN Disconnect";
                    break;
                default:
                    punSwitch.interactable = false;
                    punSwitchText.text = "PUN busy";
                    break;
            }
            UpdateUiBasedOnVoiceState(punVoiceNetwork.ClientState);
        }

        private void VoiceClientStateChanged(Photon.Realtime.ClientState fromState, Photon.Realtime.ClientState toState)
        {
            UpdateUiBasedOnVoiceState(toState);
        }

        private void UpdateUiBasedOnVoiceState(Photon.Realtime.ClientState voiceClientState)
        {
            voiceState.text = string.Format("PhotonVoice: {0}", voiceClientState);
            switch (voiceClientState)
            {
                case Photon.Realtime.ClientState.Joined:
                    voiceSwitch.interactable = true;
                    inGameSettings.SetActive(true);
                    voiceSwitchText.text = "Voice Disconnect";
                    InitToggles(inGameSettings.GetComponentsInChildren<Toggle>());
                    if (recorder != null)
                    {
                        calibrateButton.interactable = !recorder.VoiceDetectorCalibrating;
                        calibrateText.text = recorder.VoiceDetectorCalibrating ? "Calibrating" : string.Format("Calibrate ({0}s)", calibrationMilliSeconds / 1000);
                    }
                    else
                    {
                        calibrateButton.interactable = false;
                        calibrateText.text = "Unavailable";
                    }
                    break;
                case Photon.Realtime.ClientState.PeerCreated:
                case Photon.Realtime.ClientState.Disconnected:
                    if (PhotonNetwork.InRoom)
                    {
                        voiceSwitch.interactable = true;
                        voiceSwitchText.text = "Voice Connect";
                        voiceDebugText.text = String.Empty;
                    }
                    else
                    {
                        voiceSwitch.interactable = false;
                        voiceSwitchText.text = "Voice N/A";
                        voiceDebugText.text = String.Empty;
                    }
                    calibrateButton.interactable = false;
                    voiceSwitchText.text = "Voice Connect";
                    calibrateText.text = "Unavailable";
                    inGameSettings.SetActive(false);
                    break;
                default:
                    voiceSwitch.interactable = false;
                    voiceSwitchText.text = "Voice busy";
                    break;
            }
        }
    }



}