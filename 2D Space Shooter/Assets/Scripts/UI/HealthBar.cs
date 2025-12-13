using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health playerHealth; // Link your existing Health script
    public Image fillImage;     // Drag the HealthBarFill image here

    void Update()
    {
        if (playerHealth != null && fillImage != null)
        {
            float fill = (float)playerHealth.currentHealth / playerHealth.maximumHealth;
            fillImage.fillAmount = fill;
        }
        else if (fillImage != null) fillImage.fillAmount = 0f; // Reset if playerHealth is not set
    }
}
