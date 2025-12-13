using UnityEngine;
using System.Collections;

public class EndLevelTrigger : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.instance != null) GameManager.instance.LevelCleared(); 
    }

}
