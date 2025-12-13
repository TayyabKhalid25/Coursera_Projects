using UnityEngine;

public class RotateAround2D : MonoBehaviour
{
    [Tooltip("The object to rotate around")]
    public Transform target;

    [Tooltip("Speed in degrees per second")]
    public float rotationSpeed = 30f;


    void Update()
    {

        if (target != null)
        {
            transform.RotateAround(target.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
