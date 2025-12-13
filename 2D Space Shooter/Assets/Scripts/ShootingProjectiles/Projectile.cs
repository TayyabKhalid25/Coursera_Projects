using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to make projectiles move
/// </summary>
public class Projectile : MonoBehaviour
{
    [Tooltip("The distance this projectile will move each second.")]
    public float projectileSpeed = 3.0f;

    private Vector3 direction;

    private void Start()
    {
        direction = transform.up.normalized; // Capture the initial direction at spawn
    }

    private void Update()
    {
        transform.position += direction * projectileSpeed * Time.deltaTime;
    }
}