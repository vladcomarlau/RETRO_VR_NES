using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace L2_VR_SteamVR_Basics.Scripts
{
    /// <summary>
    /// Controls the boxing bag's punch strength display and effects.
    /// </summary>

    [RequireComponent(typeof(AudioSource))]
    public class BoxingBagPunchStrengthController : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField] 
        [BoxGroup("UI components")]
        [Required]
        private TextMeshProUGUI worldSpaceText;
        
        [SerializeField] 
        [BoxGroup("Settings")]
        private float ignoreTime = 0.2f;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Note on 'DOTween':
        //  - 'DOTween' is a tweening library (short from "inbetweening", terminology from keyframed animations).
        //  - It allows for smooth animations or transitions over time, applied to transforms, colors, UI elements, and more.
        //  - You can use 'DOTween' to create timers, chain actions, or animate properties without writing manual update logic.
        //  - For anyone interested to learn more: https://dotween.demigiant.com

        private void OnCollisionEnter(Collision other)
        {
            // If the timer (tween) is playing, ignore this collision.
            if (DOTween.IsTweening(GetInstanceID()))
            {
                return;
            }

            // TODO 3.1 : Compute the punch strength.
            //            Hint: Use the provided collision's 'relativeVelocity' magnitude parameter.
            var strength = other.relativeVelocity.magnitude;

            // TODO 3.2 : Update the world space UI text element with the punch strength value.
            worldSpaceText.text = strength.ToString();

            // TODO 3.3 : Add a punch sound effect. Vary its volume based on the punch strength.
            //            Hints: - Experiment first to see what punch strength values you achieve and adjust the volume based on that.
            //                   - Play the sound effect at the location of the collision's contact. See 'other.contacts'.
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.volume = Mathf.Clamp(strength / 10.0f, 0.0f, 1.0f);  //volumul e intre 0.0f-1.0
            audioSource.Play();


            // TODO 3.4 : Add some particle FX at the location of the punch.
            Vector3 contactPoint = other.contacts[0].point;

            GameObject particleObj = new GameObject("PunchImpactParticles");
            particleObj.transform.position = contactPoint;

            ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();

            // 1. FIX THE PINK ERROR: Assign a basic built-in shader
            ParticleSystemRenderer renderer = particleObj.GetComponent<ParticleSystemRenderer>();
            renderer.material = new Material(Shader.Find("Sprites/Default"));

            var main = ps.main;
            main.startSize = 0.05f;
            main.startSpeed = 0.5f;
            main.startLifetime = 0.5f;

            // 2. MAKE IT LOOK LIKE SOMETHING: Change the color to yellow/orange sparks
            main.startColor = new Color(1.0f, 0.8f, 0.0f); // Yellow-orange

            var shape = ps.shape;
            shape.enabled = false;

            Destroy(particleObj, 2.0f);

            // Start a timer using 'DOTween'. 'SetId' is useful for referencing this particular tween.
            // 'ignoreTime' helps avoid multiple collisions being registered in quick succession (jitter).
            DOVirtual
                .DelayedCall(ignoreTime, () => { /* code called on timer end*/ }, false)
                .SetId(GetInstanceID());
        }
    }
}
