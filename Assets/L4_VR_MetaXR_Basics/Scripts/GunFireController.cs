using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;
using static L4_VR_MetaXR_Basics.Scripts.Utils;

namespace L4_VR_MetaXR_Basics.Scripts
{
    /// <summary>
    /// Script used to control gun firing based on trigger presses.
    /// </summary>
    public class GunFireController : MonoBehaviour
    {

        [SerializeField]
        [BoxGroup("UI")]
        private TMP_Text modeText;

        [SerializeField]
        [BoxGroup("Gun components")]
        private GunFingerCurlController gunFingerCurlController;

        [SerializeField]
        [BoxGroup("Settings")]
        private GunFireMode gunFireMode = GunFireMode.SemiAutomatic;

        [SerializeField]
        [BoxGroup("Settings")]
        [Tooltip("Rate of fire in Rounds Per Minute (RPM)")]
        private float fireRateRPM = 600f;

        // In semi-automatic mode, this flag ensures the gun can only fire once per trigger press.
        private bool _canFire = true;

        // Tracks if the user's finger is currently holding the trigger down
        private bool _isTriggerHeld = false;

        // Tracks when the gun is allowed to shoot its next bullet based on the fire rate
        private float _nextFireTime = 0.0f;

        [Button]
        public void Change()
        {
            // Do not do this in edit mode :p.
            if (!Application.isPlaying) return;

            if (gunFireMode == GunFireMode.SemiAutomatic)
            {
                gunFireMode = GunFireMode.Automatic;
                if (modeText != null)
                {
                    modeText.text = "Automatic";
                }
            }
            else if (gunFireMode == GunFireMode.Automatic) // Added 'else' to prevent double-toggling
            {
                gunFireMode = GunFireMode.SemiAutomatic;
                if (modeText!=null)
                {
                    modeText.text = "Semi-Auto";
                }
            }
        }

        public event Action OnGunFired;

        private void OnEnable()
        {
            gunFingerCurlController.OnTriggerPressed += HandleTriggerPressed;
            gunFingerCurlController.OnTriggerReleased += HandleTriggerReleased;
        }

        private void OnDisable()
        {
            gunFingerCurlController.OnTriggerPressed -= HandleTriggerPressed;
            gunFingerCurlController.OnTriggerReleased -= HandleTriggerReleased;
        }

        private void Update()
        {
            // If in Automatic mode, the trigger is held, and enough time has passed since the last shot
            if (gunFireMode == GunFireMode.Automatic && _isTriggerHeld)
            {
                if (Time.time >= _nextFireTime)
                {
                    FireGun();
                    // Set the cooldown for the next bullet (60 seconds / RPM)
                    _nextFireTime = Time.time + (60.0f / fireRateRPM);
                }
            }
        }

        private void HandleTriggerPressed()
        {
            _isTriggerHeld = true;

            if (gunFireMode == GunFireMode.SemiAutomatic)
            {
                // Added a fire rate check to semi-auto as well to prevent macro/spam abuse
                if (_canFire && Time.time >= _nextFireTime)
                {
                    FireGun();
                    _canFire = false;
                    _nextFireTime = Time.time + (60f / fireRateRPM);
                }
            }
        }

        private void HandleTriggerReleased()
        {
            _isTriggerHeld = false;

            if (gunFireMode == GunFireMode.SemiAutomatic)
            {
                _canFire = true;
            }
        }

        private void FireGun()
        {
            Debug.Log("[GunFireController] Gun fired!");
            OnGunFired?.Invoke();
        }
    }
}