using UnityEngine;
using Valve.VR;

namespace L3_VR_SteamVR_Advanced.Scripts.Input
{
    public class SteamVRInputActionsTesting : MonoBehaviour
    {
        // TODO 1 : Setup input for the already-defined `GrabPinch` action.
        //          Write a message in the console which signifies this input is correctly read.
        //          Use either the polling method or an event-based mechanism.

        void OnEnable()
        {
            SteamVR_Actions._default.GrabPinch.onChange += OnGrabPinchChanged;

        }

        private void OnDisable()
        {
            SteamVR_Actions._default.GrabPinch.onChange -= OnGrabPinchChanged;
        }

        private void OnGrabPinchChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool grabPinchState)
        {
            Debug.Log($"[SteamVRInputActionsTesting] Polling: grabPinchState = {grabPinchState}");
        }

        // Update()
        //{

        //  var grabPinchState = SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.Any);
        //Debug.Log($"[SteamVRInputActionsTesting] Polling: grabPinchState = {grabPinchState}");
        // Debug.Log($"TouchTrigger": { })

        //}




        // TODO 2 : Setup input for the `TouchTrigger` action (you'll have to first create it & bind it accordingly)
        //          Write a message in the console which signifies this input is correctly read.
        //          Use either the polling method or an event-based mechanism
        //          

        private void Update()
        {
            Debug.Log($"TouchTrigger: {SteamVR_Actions._default.TouchTrigger.GetState(SteamVR_Input_Sources.Any)}");

        }
    }
}