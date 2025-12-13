using UnityEngine;

public class SelfOscillate : MonoBehaviour
{
    [Tooltip("Maximum angle to rotate in each direction (degrees)")]
    public float maxAngle = 15f;

    [Tooltip("Speed of oscillation in cycles per second")]
    public float speed = 1f;

    private Quaternion initialRotation;

    void Start()
    {
        // Save the object's starting rotation
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate oscillation angle using sine wave
        float angle = Mathf.Sin(Time.time * Mathf.PI * 2f * speed) * maxAngle;

        // Apply rotation around local X-axis, based on initial rotation
        transform.rotation = initialRotation * Quaternion.Euler(angle, 0f, 0f);
    }
}
