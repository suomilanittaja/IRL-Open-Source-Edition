using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;


namespace Photon.Voice.DemoVoiceUI
{
    public struct MicRef
    {
        public Recorder.MicType MicType;
        public string Name;
        public int PhotonId;

        public MicRef(string name, int id)
        {
            this.MicType = Recorder.MicType.Photon;
            this.Name = name;
            this.PhotonId = id;
        }

        public MicRef(string name)
        {
            this.MicType = Recorder.MicType.Unity;
            this.Name = name;
            this.PhotonId = -1;
        }

        public override string ToString()
        {
            return string.Format("Mic reference: {0}", this.Name);
        }
    }


    public class MicrophoneDropdownFiller : MonoBehaviour
    {
        public Recorder recorder;

        public Dropdown micDropdown;

        List<MicRef> micOptions;
        
        [SerializeField]
        private GameObject RefreshButton;

        [SerializeField]
        private GameObject ToggleButton;

        private void Awake()
        {
            this.RefreshMicrophones();
        }

        private void SetupMicDropdown()
        {
            this.micDropdown.ClearOptions();

            this.micOptions = new List<MicRef>();
            List<string> micOptionsStrings = new List<string>();

            foreach (string x in Microphone.devices)
            {
                this.micOptions.Add(new MicRef(x));
                micOptionsStrings.Add(string.Format("[Unity] {0}", x));
            }

            if (Recorder.PhotonMicrophoneEnumerator.IsSupported)
            {
                this.RefreshButton.SetActive(true);
                this.ToggleButton.SetActive(false);
                for (int i = 0; i < Recorder.PhotonMicrophoneEnumerator.Count; i++)
                {
                    string n = Recorder.PhotonMicrophoneEnumerator.NameAtIndex(i);
                    this.micOptions.Add(new MicRef(n, Recorder.PhotonMicrophoneEnumerator.IDAtIndex(i)));
                    micOptionsStrings.Add(string.Format("[Photon] {0}", n));
                }
            }
            else
            {
                this.ToggleButton.SetActive(true);
                this.RefreshButton.SetActive(!this.ToggleButton.GetComponentInChildren<Toggle>().isOn);
            }

            this.micDropdown.AddOptions(micOptionsStrings);
            this.micDropdown.onValueChanged.RemoveAllListeners();
            this.micDropdown.onValueChanged.AddListener(delegate { this.MicDropdownValueChanged(this.micOptions[this.micDropdown.value]); });

            this.SetCurrentValue();
        }

        private void MicDropdownValueChanged(MicRef mic)
        {
            this.recorder.MicrophoneType = mic.MicType;

            switch (mic.MicType)
            {
                case Recorder.MicType.Unity:
                    this.recorder.UnityMicrophoneDevice = mic.Name;
                    break;
                case Recorder.MicType.Photon:
                    this.recorder.PhotonMicrophoneDeviceId = mic.PhotonId;
                    break;
            }

            if (this.recorder.RequiresRestart)
            {
                this.recorder.RestartRecording();
            }
        }

        private void SetCurrentValue()
        {
            if (this.micOptions == null)
            {
                Debug.LogWarning("micOptions list is null");
                return;
            }
            for (int valueIndex = 0; valueIndex < this.micOptions.Count; valueIndex++)
            {
                MicRef val = this.micOptions[valueIndex];
                if (this.recorder.MicrophoneType == val.MicType)
                {
                    if (this.recorder.MicrophoneType == Recorder.MicType.Unity &&
                        val.Name.Equals(this.recorder.UnityMicrophoneDevice))
                    {
                        this.micDropdown.value = valueIndex;
                        break;
                    }
                    if (this.recorder.MicrophoneType == Recorder.MicType.Photon &&
                        val.PhotonId == this.recorder.PhotonMicrophoneDeviceId)
                    {
                        this.micDropdown.value = valueIndex;
                        break;
                    }
                }
            }
        }

        public void PhotonMicToggled(bool on)
        {
            this.micDropdown.gameObject.SetActive(!on);
            this.RefreshButton.SetActive(!on);
            if (on)
            {
                this.recorder.MicrophoneType = Recorder.MicType.Photon;
                this.recorder.RestartRecording();
            }
            else
            {
                this.RefreshMicrophones();
                this.MicDropdownValueChanged(this.micOptions[this.micDropdown.value]);
            }
        }

        public void RefreshMicrophones()
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            //Debug.Log("Refresh Mics");
            Recorder.PhotonMicrophoneEnumerator.Refresh();
            #endif
            this.SetupMicDropdown();
        }

        #if UNITY_EDITOR
        // sync. UI in case a change happens from the Unity Editor Inspector
        private void PhotonVoiceCreated()
        {
            this.SetCurrentValue();
        }
        #endif
    }
}