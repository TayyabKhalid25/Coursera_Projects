using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for the Slider component

public class BossHealth : UIelement
{
    [Header("Settings")]
    public Health targetHealth = null;

    [Tooltip("Assign the Slider component from the Hierarchy here")]
    public Slider healthSlider;

    private void Start()
    {
        if (targetHealth == null && (GameManager.instance != null && GameManager.instance.player != null))
        {
            targetHealth = GameManager.instance.player.GetComponentInChildren<Health>();
        }

        // OPTIONAL: Initialize slider settings to ensure it works with 0-1 values
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;
        }

        UpdateUI();
    }

    public override void UpdateUI()
    {
        if (targetHealth != null && healthSlider != null)
        {
            // Calculate percentage (0.0 to 1.0)
            float fillRatio = (float)targetHealth.currentHealth / targetHealth.maximumHealth;

            // Apply to slider
            healthSlider.value = fillRatio;
        }
    }
}