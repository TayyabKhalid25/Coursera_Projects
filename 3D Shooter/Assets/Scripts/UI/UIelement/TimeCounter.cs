using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles updating the high score display
/// </summary>
public class TimeCounter : UIelement
{
    [Header("References")]
    [Tooltip("The text that displays the Time Remaining")]
    public Text displayText = null;

    private void OnEnable()
    {
        GameManager.OnTimeUpdated += DisplayTime;
    }

    private void OnDisable()
    {
        GameManager.OnTimeUpdated -= DisplayTime;
    }

    /// <summary>
    /// Description:
    /// Updates the display text with the time remaining
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    public void DisplayTime(float timeRemaining = 0)
    {
        if (displayText != null)
        {
            // Format time to 00:00 (optional)
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            displayText.text = "Time Left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    /// <summary>
    /// Description:
    /// Updates the UI element according to this class
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    public override void UpdateUI()
    {
        // This calls the base update UI function from the UIelement class
        base.UpdateUI();

        // The remaining code is only called for this sub-class of UIelement and not others
        // DisplayTime();
    }
}
