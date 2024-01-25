using UnityEngine;

/// <summary>
/// Controls the rotation of a joint based on the rotation of an animation target.
/// It allows for the option to invert this rotation.
/// </summary>
public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private bool invertRotation;

    [SerializeField]
    private ConfigurableJoint thisJoint;

    [SerializeField]
    private Transform animationTarget;

    private Quaternion initialRotationInverse;

    /// <summary>
    /// Initialize the initial rotation.
    /// </summary>
    void Start()
    {
        initialRotationInverse = Quaternion.Inverse(animationTarget.localRotation);
    }

    /// <summary>
    /// Updates the target rotation of the joint every frame, considering whether
    /// the rotation should be inverted.
    /// </summary>
    void LateUpdate()
    {
        Quaternion targetRotation = animationTarget.localRotation * initialRotationInverse;
        thisJoint.targetRotation = invertRotation ? Quaternion.Inverse(targetRotation) : targetRotation;
    }
}
