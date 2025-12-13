using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class which controlls player aiming and shooting
/// </summary>
public class StraightSpawner : MonoBehaviour
{
    [Header("GameObject/Component References")]
    [Tooltip("The projectiles to be fired.")]
    public List<GameObject> projectilePrefabs = null;
    [Tooltip("The transform in the heirarchy which holds projectiles if any")]
    public Transform projectileHolder = null;

    [Header("Input Settings, Actions, & Controls")]
    [Tooltip("Whether this shooting controller is controled by the player")]
    public bool isPlayerControlled = false;
    public InputAction fireAction;

    [Header("Firing Settings")]
    [Tooltip("The minimum time between projectiles being fired.")]
    public float fireRate = 0.05f;

    [Tooltip("The maximum diference between the direction the" +
        " shooting controller is facing and the direction projectiles are launched.")]
    public float projectileSpread = 1.0f;

    // The last time this component was fired
    private float lastFired = Mathf.NegativeInfinity;

    [Header("Effects")]
    [Tooltip("The effect to create when this fires")]
    public GameObject fireEffect;

    /// <summary>
    /// Standard Unity function called whenever the attached gameobject is enabled
    /// </summary>
    void OnEnable()
    {
        fireAction.Enable();
    }

    /// <summary>
    /// Standard Unity function called whenever the attached gameobject is disabled
    /// </summary>
    void OnDisable()
    {
        fireAction.Disable();
    }

    /// <summary>
    /// Description:
    /// Standard unity function that runs every frame
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    private void Update()
    {
        Fire();
    }

    /// <summary>
    /// Description:
    /// Standard unity function that runs when the script starts
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    private void Start()
    {
        if (fireAction.bindings.Count == 0 && isPlayerControlled)
        {
            Debug.LogWarning("The Fire Input Action does not have a binding set but is set to be player controlled! Make sure that it has a binding or the shooting controller will not shoot!");
        }
    }

    /// <summary>
    /// Description:
    /// Fires a projectile if possible
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    public void Fire()
    {
        // If the cooldown is over fire a projectile
        if ((Time.timeSinceLevelLoad - lastFired) > fireRate)
        {
            // Launches a projectile
            SpawnProjectile();

            if (fireEffect != null)
            {
                GameObject projectileEffect = Instantiate(fireEffect, transform.position, transform.rotation, null);
                // Keep the heirarchy organized
                if (projectileHolder == null && GameObject.Find("ProjectileHolder") != null)
                {
                    projectileHolder = GameObject.Find("ProjectileHolder").transform;
                }
                if (projectileHolder != null)
                {
                    projectileEffect.transform.SetParent(projectileHolder);
                }
            }

            // Restart the cooldown
            lastFired = Time.timeSinceLevelLoad;
        }
    }

    /// <summary>
    /// Description:
    /// Spawns a projectile and sets it up
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    public void SpawnProjectile()
    {
        // Check that the prefab list is valid and not empty
        if (projectilePrefabs != null && projectilePrefabs.Count > 0)
        {
            // Pick a random projectile from the list
            int randomIndex = Random.Range(0, projectilePrefabs.Count);
            GameObject selectedPrefab = projectilePrefabs[randomIndex];

            if (selectedPrefab != null)
            {
                // Create the projectile
                GameObject projectileGameObject = Instantiate(selectedPrefab, transform.position, transform.rotation);

                // Apply spread to Z-axis (for 2D rotation)
                Vector3 rotationEulerAngles = projectileGameObject.transform.rotation.eulerAngles;
                rotationEulerAngles.z += Random.Range(-projectileSpread, projectileSpread);
                projectileGameObject.transform.rotation = Quaternion.Euler(rotationEulerAngles);

                if (projectileHolder != null)
                {
                    projectileGameObject.transform.SetParent(projectileHolder);
                }
            }
        }
    }
}
