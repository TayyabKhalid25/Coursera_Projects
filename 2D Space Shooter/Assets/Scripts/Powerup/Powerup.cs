using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType { SpeedBoost, Health, FireRate, InstaKill }
    public PowerupType type;
    public float amount = 10f;
    public float duration = 5f;
    public GameObject effectPrefab;
    public UIManager uiManager;

    private bool successful = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            successful = false;
            ApplyPowerup(other);
            if (successful) 
            {
                Destroy(gameObject); // remove the powerup from the scene
                if (effectPrefab != null)
                {
                    Instantiate(effectPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

    void ApplyPowerup(Collider2D other)
    {        
        Controller playerController = other.GetComponent<Controller>();
        Health playerHealth = other.GetComponent<Health>();
        ShootingController shootingController = other.GetComponent<ShootingController>();
        switch (type)
        {
            case PowerupType.SpeedBoost:
                if (playerController == null) return; // Ensure player has a Controller component
                playerController.ApplySpeedBoost(amount, duration);
                if (uiManager != null) uiManager.ActivatePowerup(UIManager.PowerupType.Speed, duration);
                successful = true; // Indicate that the powerup was successfully applied
                break;
            case PowerupType.Health:
                if (playerHealth == null) return; // Ensure player has a Health component
                playerHealth.ReceiveHealing((int)amount);
                successful = true; // Indicate that the powerup was successfully applied
                break;
            case PowerupType.FireRate:
                if (shootingController == null) return; // Ensure player has a ShootingController component
                shootingController.ApplyFireRateBoost(amount, duration);
                if (uiManager != null) uiManager.ActivatePowerup(UIManager.PowerupType.FireRate, duration);
                successful = true; // Indicate that the powerup was successfully applied
                break;
            case PowerupType.InstaKill:
                if (shootingController == null) return; // Ensure player has a ShootingController component
                shootingController.ApplyInstaKill(duration);
                if (uiManager != null) uiManager.ActivatePowerup(UIManager.PowerupType.InstaKill, duration);
                successful = true; // Indicate that the powerup was successfully applied
                break;
        }
    }
}
