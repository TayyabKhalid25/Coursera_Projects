using UnityEngine;

public class ScrollBG : MonoBehaviour
{
    public Transform playerCamera;
    public Material backgroundMaterial;
    public Vector2 parallaxFactor = new Vector2(0.1f, 0.1f);

    private Vector2 lastPlayerPos;
    private Vector2 currentOffset = Vector2.zero;

    void Start()
    {
        if (playerCamera != null)
        {
            lastPlayerPos = playerCamera.position;
            // Set the background's initial position to follow the playerCamera
            transform.position = new Vector3(playerCamera.position.x, playerCamera.position.y, transform.position.z);
        }
    }

    void Update()
    {
        if (playerCamera != null && backgroundMaterial != null)
        {
            // Calculate how much the playerCamera moved
            Vector2 playerDelta = (Vector2)playerCamera.position - lastPlayerPos;

            // Scroll the UVs for parallax effect
            currentOffset += playerDelta * parallaxFactor;
            backgroundMaterial.SetVector("_ScrollOffset", new Vector4(currentOffset.x, currentOffset.y, 0, 0));

            // Move the background object with the playerCamera (so it stays in view)
            transform.position = new Vector3(playerCamera.position.x, playerCamera.position.y, transform.position.z);

            // Update last position
            lastPlayerPos = playerCamera.position;
        }
    }
}
