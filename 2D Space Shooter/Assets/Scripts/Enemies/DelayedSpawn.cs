using UnityEngine;
using System.Collections;

public class DelayedSpawn : MonoBehaviour
{
    [Tooltip("Delay before activating all children, in seconds.")]
    public float delayInSeconds = 1f;

    private void Start()
    {
        StartCoroutine(ActivateChildrenAfterDelay());
    }

    private IEnumerator ActivateChildrenAfterDelay()
    {
        float elapsed = 0f;

        while (elapsed < delayInSeconds)
        {
            elapsed += Time.deltaTime;
            yield return null; // wait for next frame
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
