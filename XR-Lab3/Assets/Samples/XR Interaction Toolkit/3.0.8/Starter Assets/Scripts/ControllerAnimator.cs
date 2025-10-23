using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Component which reads input values and drives the thumbstick, trigger, and grip transforms
    /// to animate a controller model.
    /// </summary>
    public class ControllerAnimator : MonoBehaviour
    {
        [Header("Thumbstick")]
        [SerializeField]
        Transform m_ThumbstickTransform;

        [SerializeField]
        Vector2 m_StickRotationRange = new Vector2(30f, 30f);

        [SerializeField]
        XRInputValueReader<Vector2> m_StickInput = new XRInputValueReader<Vector2>("Thumbstick");

        [Header("Trigger")]
        [SerializeField]
        Transform m_TriggerTransform;

        [SerializeField]
        Vector2 m_TriggerXAxisRotationRange = new Vector2(0f, -15f);

        [SerializeField]
        XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");

        [Header("Grip")]
        [SerializeField]
        Transform m_GripTransform;

        [SerializeField]
        Vector2 m_GripRightRange = new Vector2(-0.0125f, -0.011f);

        [SerializeField]
        XRInputValueReader<float> m_GripInput = new XRInputValueReader<float>("Grip");
        
        [SerializeField]
        Animator handAnimator;

        [SerializeField]
        float m_GripThreshold = 0.5f;
        [SerializeField]
        float m_TriggerThreshold = 0.5f;

        void OnEnable()
        {
            // Keep API compatibility, but only warn if Animator is missing
            if (handAnimator == null)
            {
                enabled = false;
                Debug.LogWarning($"Controller Animator component missing Animator reference on {gameObject.name}", this);
                return;
            }
            m_StickInput?.EnableDirectActionIfModeUsed();
            m_TriggerInput?.EnableDirectActionIfModeUsed();
            m_GripInput?.EnableDirectActionIfModeUsed();
        }

        void OnDisable()
        {
            m_StickInput?.DisableDirectActionIfModeUsed();
            m_TriggerInput?.DisableDirectActionIfModeUsed();
            m_GripInput?.DisableDirectActionIfModeUsed();
        }

        void Update()
        {
            // Read input values
            float gripVal = m_GripInput != null ? m_GripInput.ReadValue() : 0f;
            float triggerVal = m_TriggerInput != null ? m_TriggerInput.ReadValue() : 0f;

            handAnimator.SetBool("grabbing", gripVal > m_GripThreshold);
            handAnimator.SetBool("pinching", triggerVal > m_TriggerThreshold);
        }
    }
}
