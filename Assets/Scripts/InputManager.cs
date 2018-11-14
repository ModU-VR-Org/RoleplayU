using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles player controller input locally, triggers functionality on the local Avatar

public class InputManager : MonoBehaviour
{
    public delegate void InputEvent();

    public ControllerInputManager controllerLeft;
    public ControllerInputManager controllerRight;

    private void OnEnable()
    {
        controllerLeft.Enable();
        controllerRight.Enable();
    }

    [System.Serializable]
    public class ControllerInputManager
    {
        public GameObject controller;

        [HideInInspector]
        public SteamVR_TrackedController trackedController;

        public event InputEvent OnTriggerClicked;
        public event InputEvent OnTriggerUnClicked;

        public event InputEvent OnGripClicked;
        public event InputEvent OnGripUnClicked;

        public event InputEvent OnPadClicked;
        public event InputEvent OnPadUnClicked;

        public float XAxis
        {
            get { return trackedController.controllerState.rAxis0.x; }
        }
        public float YAxis
        {
            get { return trackedController.controllerState.rAxis0.y; }
        }

        public void Enable()
        {
            trackedController = controller.GetComponent<SteamVR_TrackedController>();

            trackedController.TriggerClicked += TriggerClicked;
            trackedController.TriggerUnclicked += TriggerUnClicked;

            trackedController.Gripped += GripClicked;
            trackedController.Ungripped += GripUnClicked;

            trackedController.PadClicked += PadClicked;
            trackedController.PadUnclicked += PadUnClicked;
        }

        private void TriggerClicked(object sender, ClickedEventArgs args)
        {
            if (OnTriggerClicked != null)
                OnTriggerClicked();
        }
        private void TriggerUnClicked(object sender, ClickedEventArgs args)
        {
            if (OnTriggerUnClicked != null)
                OnTriggerUnClicked();
        }

        private void GripClicked(object sender, ClickedEventArgs args)
        {
            if (OnGripClicked != null)
                OnGripClicked();
        }
        private void GripUnClicked(object sender, ClickedEventArgs args)
        {
            if (OnGripUnClicked != null)
                OnGripUnClicked();
        }

        private void PadClicked(object sender, ClickedEventArgs args) 
        {
            if (OnPadClicked != null)
                OnPadClicked();
        }
        private void PadUnClicked(object sender, ClickedEventArgs args) 
        {
            if (OnPadUnClicked != null)
                OnPadUnClicked();
        }
    }
}