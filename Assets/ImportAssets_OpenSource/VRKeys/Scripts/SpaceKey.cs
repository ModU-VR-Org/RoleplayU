﻿/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 */

using UnityEngine;
using System.Collections;
//using System.Windows.Forms;
//using WindowsInput;
using UnityEngine.UI;
using TMPro;

namespace VRKeys {

    

	/// <summary>
	/// Space key.
	/// </summary>
	public class SpaceKey : Key {

        public TMP_InputField inputField;

        public override void HandleTriggerEnter (Collider other) {
            keyboard.AddCharacter (" ", " ");
            //SendKeys.Send("w");
            //WindowsInput.InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
            //inputField.

            //ActivateFor(0.3f);
		}

		//public override void UpdateLayout (Layout translation) {
		//	label.text = translation.spaceButtonLabel;
		//}
	}
}