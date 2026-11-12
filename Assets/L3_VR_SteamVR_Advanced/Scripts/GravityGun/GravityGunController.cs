// Fix for Meta XR build issues. SteamVR should anyway only be used on standalone builds.
#if UNITY_STANDALONE 

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace L3_VR_SteamVR_Advanced.Scripts.GravityGun
{

    public class GravityGunController : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Input")]
        private SteamVR_Action_Vector2 rotateAction; // Map this to your thumbstick in SteamVR Input

        [SerializeField]
        [BoxGroup("Settings")]
        private float rotationSpeed = 180f;

        private Hand _holdingHand; // To track which hand to read input from

        [SerializeField] 
        [BoxGroup("Game object components")]
        [Required]
        private Interactable gunInteractable;
        
        [SerializeField] 
        [BoxGroup("Game object components")]
        [Required]
        private Transform snapTransform;
        
        [SerializeField] 
        [BoxGroup("Game object components")]
        [Required]
        private LineRenderer laserBeam;
        
        [SerializeField] 
        [BoxGroup("Settings")]
        private float snapTime = 1f;
        
        [SerializeField]
        [BoxGroup("Settings")]   
        private float throwForce = 15f;
        

        private bool _isGunGrabbed;
        private GravityGunObject _selectedObject;

        private void OnEnable()
        {
            // Listen to events related to grabbing & releasing the gravity gun.
            gunInteractable.onAttachedToHand   += OnGunGrabbedInHand;
            gunInteractable.onDetachedFromHand += OnGunReleasedFromHand;

            //  TODO 3.1 : Setup input for the gravity gun.
            //             You'll have to create at least 2 new actions and bindings first.
            //             With these, write input logic to drive the gravity gun logic.
            //             - On trigger touch ------> call `SnapObject` method.
            //             - On trigger click/pull -> call `ThrowObject` method.
            //             - On touch release ------> call `TossObject` method.
            //                  - Note: "touch release" means when the touch input goes from `true` to false`.
            //             - Use either the polling method (Update) or an event-based mechanism (OnEnable / OnDisable
            //             

            SteamVR_Actions._default.GrabPinch.onChange += OnGrabPinchChanged;
            SteamVR_Actions._default.TouchTrigger.onChange += OnTouchTriggerChanged;

        }

        private void OnDisable()
        {
            // Stop listening to events related to grabbing & releasing the gravity gun.
            gunInteractable.onAttachedToHand -= OnGunGrabbedInHand;
            gunInteractable.onDetachedFromHand -= OnGunReleasedFromHand;

            //  TODO 3.1 : Don't forget to unsubscribe from the events if you've implemented the event-based mechanism!
            SteamVR_Actions._default.GrabPinch.onChange -= OnGrabPinchChanged;
            SteamVR_Actions._default.TouchTrigger.onChange -= OnTouchTriggerChanged;
        }

        private void OnTouchTriggerChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool touchTriggerState)
        {
            if (!_isGunGrabbed)
            {
                return;
            }
            if (touchTriggerState)
            {
                SnapObject();
            }
            else
            {
                TossObject();
            }
        }

        private void OnGrabPinchChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool grabPinchState)
        {
            ThrowObject();
        }

        private void OnGunGrabbedInHand(Hand hand)
        {
            _isGunGrabbed = true;
            _holdingHand = hand;
        }

        private void OnGunReleasedFromHand(Hand hand)
        {
            _isGunGrabbed = false;
            _holdingHand = null;
            TossObject();
        }

        private void HandleObjectRotation()
        {
            // Only rotate if we are actually holding the gun and an object is currently snapped
            if (!_isGunGrabbed || _holdingHand == null || _selectedObject == null || !_selectedObject.IsSnapped)
                return;

            // Read the thumbstick/trackpad input for the hand currently holding the gun
            Vector2 rotationInput = rotateAction.GetAxis(_holdingHand.handType);

            if (rotationInput.sqrMagnitude > 0.01f)
            {
                // Kill the snap rotation tween in case the user starts rotating before the snap animation finishes
                DOTween.Kill("SnapRotAnim");

                // Calculate rotation amounts based on input and speed
                float xRotation = rotationInput.y * rotationSpeed * Time.deltaTime; // Up/Down on thumbstick = pitch
                float yRotation = -rotationInput.x * rotationSpeed * Time.deltaTime; // Left/Right on thumbstick = yaw

                // Apply the rotation relative to the gun's transform so it feels natural to the player's perspective
                _selectedObject.Root.transform.Rotate(snapTransform.right, xRotation, Space.World);
                _selectedObject.Root.transform.Rotate(snapTransform.up, yRotation, Space.World);
            }
        }


        private void Update()
        {
            ResetLaserBeam();

            HandleObjectRotation();

            // If the gun is not grabbed, or there is a selected object already snapped, skip.
            if (!CanSelectObject()) return;
            
            // Store here the result of the raycast hit object.
            GravityGunObject hitObject = null;
            var hitAnything = false;
            
            // Perform raycast and check for a valid hit.
            if (Physics.Raycast(snapTransform.position, snapTransform.forward, out var rayHit))
            {
                hitAnything = true;
                hitObject = rayHit.collider.GetComponent<GravityGunObject>();
            }

            // If we hit a new object, select it (highlight it). If we hit nothing of interest, just deselect the current object.
            if (hitObject != _selectedObject)
            {
                DeselectObject();
                
                if (hitObject != null)
                {
                    SelectObject(hitObject);
                }
            }
            
            DrawLaserBeam(hitAnything, rayHit);
        }

        private void ResetLaserBeam() => laserBeam.enabled = false;

        private void DrawLaserBeam(bool hitAnything, RaycastHit rayHit)
        {
            laserBeam.enabled = true;
            
            // Set beam start position.
            laserBeam.SetPosition(0, laserBeam.transform.position);
            
            // If there's a hit, draw end position at that point.
            if (hitAnything)
            {
                laserBeam.SetPosition(1, rayHit.point);
            }
            // Otherwise set the end point at some extended position.
            else
            {
                laserBeam.SetPosition(1, laserBeam.transform.position + laserBeam.transform.forward * 20f);
            }
        }
        
        private bool CanSelectObject() => _isGunGrabbed && (_selectedObject == null || !_selectedObject.IsSnapped);

        private void SelectObject(GravityGunObject newTargetObject)
        {
            if (newTargetObject == null) return;

            _selectedObject = newTargetObject;
            _selectedObject.HighlightMesh();
        }
        
        private void DeselectObject()
        {
            if (_selectedObject == null) return;

            _selectedObject.RemoveMeshHighlight();
            _selectedObject = null;
        }

        /// <summary>
        /// Snap the selected object to the desired location (gravity gun's snap position). Animate the position & rotation in time.
        /// </summary>
        private void SnapObject()
        {
            if(_selectedObject == null) return;
            
            _selectedObject.Rigidbody.isKinematic = true;
            _selectedObject.SetParent(snapTransform);
            _selectedObject.IsSnapped = true;
            
            // Animate object's position towards snapTranform's position using `DOMove`
            var moveTween = _selectedObject.Root.transform
                .DOMove(snapTransform.position, snapTime)
                .SetEase(Ease.OutBack)
                .SetId("SnapPosAnim");
            // Because the gun can move during the move animation, ensure the final position is updated.
            moveTween.OnUpdate(() =>
                {
                    var timeRemaining = moveTween.Duration() - moveTween.Elapsed();
                    moveTween.ChangeEndValue(snapTransform.position, timeRemaining, true);
                });
            
            // Animate object's rotation towards snapTranform's rotation using `DORotate`.
            var rotateTween = _selectedObject.Root.transform
                .DORotate(snapTransform.rotation.eulerAngles, snapTime)
                .SetEase(Ease.OutBack)
                .SetId("SnapRotAnim");
            // Because the gun can rotate during the move animation, ensure the final rotation is updated.
            rotateTween
                .OnUpdate(() =>
                {
                    var timeRemaining = rotateTween.Duration() - rotateTween.Elapsed();
                    rotateTween.ChangeEndValue(snapTransform.rotation.eulerAngles, timeRemaining, true);
                });

            // Remove highlight effect.
            _selectedObject.RemoveMeshHighlight();
        }

        /// <summary>
        /// Release object without applying a throw force.
        /// </summary>
        private void TossObject()
        {
            ReleaseObject(applyThrowForce: false);
        }

        /// <summary>
        /// Release object & apply an additional throw force.
        /// </summary>
        private void ThrowObject()
        {
            ReleaseObject(applyThrowForce: true);
        }

        private void ReleaseObject(bool applyThrowForce)
        {
            if(_selectedObject == null) return;
            
            _selectedObject.Rigidbody.isKinematic = false;
            _selectedObject.IsSnapped = false;
            _selectedObject.ResetParent();
            
            // Apply force to throw object.
            if (applyThrowForce)
            {
                _selectedObject.Rigidbody.AddForce(snapTransform.forward * throwForce, ForceMode.VelocityChange);
            }
            // Otherwise, if just tossed, give the object its inertial properties.
            else
            {
                _selectedObject.Rigidbody.linearVelocity = _selectedObject.KinematicVelocity;
            }
            
            InterruptSnapping();
            DeselectObject();
        }

        private void InterruptSnapping()
        {
            DOTween.Kill("SnapPosAnim");
            DOTween.Kill("SnapRotAnim");
        }
    }
}
#endif