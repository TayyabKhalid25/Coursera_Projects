using UnityEngine;
using System.Collections.Generic;

public class StartGame : MonoBehaviour
{
    public GameObject startMenuUI; // Link your start UI Canvas
    public GameObject player;      // Optional player object to enable at start
    public GameObject UIManager;

    private List<GameObject> gameUIObjects = new List<GameObject>(); // Holds all tagged GameUI elements

    void Start()
    {
        Time.timeScale = 0f; // Pause game initially

        if (UIManager != null) UIManager.SetActive(false); // Disable UIManager if it exists 
        startMenuUI.SetActive(true);
        if (player != null) player.SetActive(false);

        // Find all objects tagged as "GameUI" and store them
        GameObject[] foundUI = GameObject.FindGameObjectsWithTag("GameUI");
        foreach (GameObject ui in foundUI)
        {
            gameUIObjects.Add(ui);
            ui.SetActive(false); // Make sure they're off initially
        }
    }

    public void StartGamey()
    {
        Time.timeScale = 1f; // Resume game
        
        if (UIManager != null) UIManager.SetActive(true); // Enable UIManager if it exists 
        startMenuUI.SetActive(false);
        if (player != null) player.SetActive(true);
        
        // Enable all GameUI objects
        foreach (GameObject ui in gameUIObjects)
        {
            ui.SetActive(true);
        }
        Destroy(this.gameObject); // Optionally destroy this script's GameObject
    }
}
