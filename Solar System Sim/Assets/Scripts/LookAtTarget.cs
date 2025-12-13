using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [Tooltip("The object this GameObject will look at when selected")]
    public GameObject currentTarget;

    [Tooltip("The camera controller used for movement and rotation")]
    public CameraController cameraController;

    void Update()
    {
        // Left click: Raycast and set current target to hit object
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            if (hits.Length > 0)
            {
                cameraController.isLookingAtTarget = true;
                currentTarget = hits[0].collider.gameObject;
                Debug.Log("Looking at: " + currentTarget.name);
            }
        }
        // Right click: Clear target
        else if (Input.GetMouseButtonDown(1))
        {
            cameraController.isLookingAtTarget = false;
            currentTarget = null;
            Debug.Log("Target cleared");
        }

        // Rotate towards target if set
        if (currentTarget != null)
        {
            transform.LookAt(currentTarget.transform);
        }
    }
}
