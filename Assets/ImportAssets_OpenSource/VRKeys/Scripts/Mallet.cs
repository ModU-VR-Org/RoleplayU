/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 */

using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using Valve.VR;

namespace VRKeys {

	/// <summary>
	/// Attaches to the end of the mallet that collides with the keys.
	/// </summary>
	public class Mallet : MonoBehaviour {
		public AudioClip clipToPlay;

		public enum MalletHand {
			Left,
			Right,
			Both,
			None
		}

		public MalletHand hand;

		public HandCollider handCollider;

		public bool isMovingDownward {
			get { return _isMovingDownward; }
			private set { _isMovingDownward = value; }
		}

		private bool _isMovingDownward = false;

		private AudioSource audioSource;

		private Controller controller;
        public DeviceInput deviceInput;

        private Vector3 prevPos = Vector3.zero;

		private void Awake () {
			audioSource = GetComponent<AudioSource> ();

			switch (UnityEngine.XR.XRSettings.loadedDeviceName) {
				//case "Oculus":
				//	controller = gameObject.AddComponent<OculusController> ();
				//	break;

				case "OpenVR":
					controller = gameObject.AddComponent<OpenVRController> ();
					break;
			}
		}

		private void FixedUpdate () {
			Vector3 curVel = (transform.position - prevPos) / Time.fixedDeltaTime;

			isMovingDownward = (curVel.y <= 0f);

			prevPos = transform.position;
		}

		/// <summary>
		/// Called by the key that hit it if the collision was successful.
		/// </summary>
		/// <param name="key">Key.</param>
		public void HandleTriggerEnter (Key key) {
			audioSource.PlayOneShot (clipToPlay);

			//if (transform.parent.transform.parent != null) {
			//	controller.TriggerPulse ();
			//}
            //print("BUTTON PRESSED");
            hapticsOn = true;
            Invoke("HoldHaptics", 0.3f);
            
        }

        public bool hapticsOn;

        private void Update()
        {
            if (hapticsOn)
            {
                if (deviceInput != null && deviceInput.device != null)
                {
                    //print("FIRE HAPTICS");
                    deviceInput.device.TriggerHapticPulse(200);

                }
            }
        }

        private void HoldHaptics()
        {
            hapticsOn = false;
        }


        /// <summary>
        /// Get the attached Controller class for input abstractions.
        /// </summary>
        /// <returns>Controller.</returns>
        public Controller Controller () {
			return controller;
		}
	}
}