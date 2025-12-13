using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Trying to load scene...");
        Debug.Log("Scene count in build: " + SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene("SolarSys");
    }
}
