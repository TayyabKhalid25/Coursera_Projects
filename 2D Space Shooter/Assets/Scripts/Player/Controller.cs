using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class controls player movement
/// </summary>
public class Controller : MonoBehaviour
{
    [Header("GameObject/Component References")]
    [Tooltip("The Rigidbody2D component to use in \"Astroids Mode\".")]
    public Rigidbody2D myRigidbody = null;

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move.")]
    public float moveSpeed = 10.0f;
    [Tooltip("The speed at which the player rotates in asteroids movement mode")]
    public float rotationSpeed = 60f;

    [Header("Input Actions & Controls")]
    [Tooltip("The input action(s) that map to player movement")]
    public InputAction moveAction;
    [Tooltip("The input action(s) that map to where the controller looks")]
    public InputAction lookAction;
    [Tooltip("The input action that maps to dodge button")]
    public InputAction dodgeAction;

    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.2f;
    public float iFrameDuration = 0.2f;
    public float dodgeCooldown = 0.5f;

    private Vector2 moveInput;
    private bool isDodging = false;
    private bool canDodge = true;
    private Health healthComponent;

    private Coroutine speedBoostCoroutine;

    
    /// <summary>
    /// Enum which stores different aiming modes
    /// </summary>
    public enum AimModes { AimTowardsMouse, AimForwards };

    [Tooltip("The aim mode in use by this player:\n" +
        "Aim Towards Mouse: Player rotates to face the mouse\n" +
        "Aim Forwards: Player aims the direction they face (doesn't face towards the mouse)")]
    public AimModes aimMode = AimModes.AimTowardsMouse;

    /// <summary>
    /// Enum to handle different movement modes for the player
    /// </summary>
    public enum MovementModes { MoveHorizontally, MoveVertically, FreeRoam, Astroids };

    [Tooltip("The movmeent mode used by this controller:\n" +
        "Move Horizontally: Player can only move left/right\n" +
        "Move Vertically: Player can only move up/down\n" +
        "FreeRoam: Player can move in any direction and can aim\n" +
        "Astroids: Player moves forward/back in the direction they are facing and rotates with horizontal input")]
    public MovementModes movementMode = MovementModes.FreeRoam;


    // Whether the player can aim with the mouse or not
    private bool canAimWithMouse
    {
        get
        {
            return aimMode == AimModes.AimTowardsMouse;
        }
    }

    // Whether the player's X coordinate is locked (Also assign in rigidbody)
    private bool lockXCoordinate
    {
        get
        {
            return movementMode == MovementModes.MoveVertically;
        }
    }
    // Whether the player's Y coordinate is locked (Also assign in rigidbody)
    public bool lockYCoordinate
    {
        get
        {
            return movementMode == MovementModes.MoveHorizontally;
        }
    }

    /// <summary>
    /// Standard Unity function called whenever the attached gameobject is enabled
    /// </summary>
    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        dodgeAction.Enable();
    }

    /// <summary>
    /// Standard Unity function called whenever the attached gameobject is disabled
    /// </summary>
    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        dodgeAction.Disable();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once when the script starts before Update
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void Start()
    {
        if (moveAction.bindings.Count == 0 || lookAction.bindings.Count == 0 || dodgeAction.bindings.Count == 0)
        {
            Debug.LogWarning("An Input Action does not have a binding set! Make sure that each Input Action has a binding set or the controller will not work!");
        }
        healthComponent = GetComponent<Health>();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once per frame
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    void Update()
    {
        HandleInput();
        if (!isDodging && myRigidbody.linearVelocity.magnitude > 0.001f || myRigidbody.angularVelocity != 0f)
        {
            myRigidbody.linearVelocity = Vector2.zero;
            myRigidbody.angularVelocity = 0f;
        }
    }

    private void HandleInput()
    {
        // Dodge trigger
        if (dodgeAction.triggered && !isDodging && canDodge && moveInput != Vector2.zero)
        {
            Debug.Log("Dodging!");
            StartCoroutine(PerformDodge(moveInput.normalized));
        }

        Vector2 lookPosition = GetLookPosition();

        // Read movement input
        if (moveAction.bindings.Count == 0 || lookAction.bindings.Count == 0 || dodgeAction.bindings.Count == 0)
        {
            Debug.LogError("One or more Input Actions do not have bindings set.");
            return;
        }

        // Assign class-level moveInput
        moveInput = moveAction.ReadValue<Vector2>();

        float horizontalMovement = moveInput.x;
        float verticalMovement = moveInput.y;
        Vector3 movementVector = new Vector3(horizontalMovement, verticalMovement, 0);

        MovePlayer(movementVector);
        LookAtPoint(lookPosition);
    }

    IEnumerator PerformDodge(Vector2 direction)
    {
        isDodging = true;
        canDodge = false;
        Vector3 dodgeDirection = new Vector3(direction.x, direction.y, 0).normalized;

        if (healthComponent != null)
        {
            healthComponent.SetTemporaryInvincibility(iFrameDuration);
            healthComponent.teamId = -1;
        }

        float halfDuration = dodgeDuration / 2f;
        float timeElapsed = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion endRotation = initialRotation * Quaternion.Euler(0f, 60f, 0f);
        // Phase 1: Rotate from 0° → 60°
        while (timeElapsed < halfDuration)
        {
            float t = timeElapsed / halfDuration; // 0 to 1

            transform.rotation = Quaternion.Slerp(initialRotation, endRotation, t);
             myRigidbody.linearVelocity = direction * dodgeSpeed;
            //transform.position = transform.position + (dodgeDirection * Time.deltaTime * (moveSpeed + dodgeSpeed));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Phase 2: Rotate from 60° → 0°
        timeElapsed = 0f;
        while (timeElapsed < halfDuration)
        {
            float t = timeElapsed / halfDuration; // 0 to 1

            transform.rotation = Quaternion.Slerp(endRotation, initialRotation, t);
             myRigidbody.linearVelocity = direction * dodgeSpeed;
            //transform.position = transform.position + (dodgeDirection * Time.deltaTime * (moveSpeed + dodgeSpeed));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Clean up
        myRigidbody.linearVelocity = Vector2.zero;
        myRigidbody.angularVelocity = 0f;
        isDodging = false;
        healthComponent.teamId = 0;

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

    


    /// <summary>
    /// Description:
    /// Updates the position the player is looking at
    /// Inputs: 
    /// none
    /// Returns: 
    /// Vector2
    /// </summary>
    /// <returns>Vector2: The position the player should look at</returns>
    public Vector2 GetLookPosition()
    {
        Vector2 result = transform.up;
        if (aimMode != AimModes.AimForwards)
        {
            if (lookAction.bindings.Count == 0)
            {
                Debug.LogError("The Look Input Action does not have a binding set! It must have a binding set in order for the player to look around!");
            }
            result = lookAction.ReadValue<Vector2>();
        }
        else
        {
            result = transform.up;
        }
        return result;
    }

    /// <summary>
    /// Description:
    /// Moves the player
    /// Inputs: 
    /// Vector3 movement
    /// Returns: 
    /// void (no return)
    /// </summary>
    /// <param name="movement">The direction to move the player</param>
    private void MovePlayer(Vector3 movement)
    {
        // Set the player's posiiton accordingly

        // Move according to astroids setting
        if (movementMode == MovementModes.Astroids)
        {

            // If no rigidbody is assigned, assign one
            if (myRigidbody == null)
            {
                myRigidbody = GetComponent<Rigidbody2D>();
            }

            // Move the player using physics
            Vector2 force = transform.up * movement.y * Time.deltaTime * moveSpeed;
            Debug.Log(force);
            myRigidbody.AddForce(force);

            // Rotate the player around the z axis
            Vector3 newRotationEulars = transform.rotation.eulerAngles;
            float zAxisRotation = transform.rotation.eulerAngles.z;
            float newZAxisRotation = zAxisRotation - rotationSpeed * movement.x * Time.deltaTime;
            newRotationEulars = new Vector3(newRotationEulars.x, newRotationEulars.y, newZAxisRotation);
            transform.rotation = Quaternion.Euler(newRotationEulars);

        }
        // Move according to the other settings
        else
        {
            // Don't move in the x if the settings stop us from doing so
            if (lockXCoordinate)
            {
                movement.x = 0;
            }
            // Don't move in the y if the settings stop us from doing so
            if (lockYCoordinate)
            {
                movement.y = 0;
            }
            // Move the player's transform
            transform.position = transform.position + (movement * Time.deltaTime * moveSpeed);
        }
    }

    /// <summary>
    /// Description: 
    /// Rotates the player to look at a point
    /// Inputs: 
    /// Vector3 point
    /// Returns: 
    /// void (no return)
    /// </summary>
    /// <param name="point">The screen space position to look at</param>
    private void LookAtPoint(Vector3 point)
    {
        if (Time.timeScale > 0 && !isDodging)
        {
            // Rotate the player to look at the mouse.
            Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;

            if (canAimWithMouse)
            {
                transform.up = lookDirection;
            }
            else
            {
                if (myRigidbody != null)
                {
                    myRigidbody.freezeRotation = true;
                }
            }
        }
    }

    public void ApplySpeedBoost(float amount, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            // Refresh the timer
            StopCoroutine(speedBoostCoroutine);
        }
        else
        {
            // Only apply once
            moveSpeed += amount;
        }

        speedBoostCoroutine = StartCoroutine(RemoveSpeedBoost(amount, duration));
    }

    private IEnumerator RemoveSpeedBoost(float amount, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        moveSpeed -= amount;
        speedBoostCoroutine = null;
    }
}
